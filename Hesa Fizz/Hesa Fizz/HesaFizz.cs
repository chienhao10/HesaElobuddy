using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace Hesa_Fizz
{
    public class HesaFizz
    {
        #region Variables
        private static Menu FizzMenu, FizzMenuCombo, FizzMenuHarass, FizzMenuMisc, FizzMenuDrawing, FizzMenuSkin, FizzMenuAbout;
        private static Geometry.Polygon.Rectangle RRectangle { get; set; }
        private static bool JumpBack { get; set; }
        private static Vector3 LastHarassPos { get; set; }
        private static int lastSkinSelected = 0;

        private static Vector2 BarOffset = new Vector2(0, 15);
        private const int BarWidth = 104;
        private const int LineThickness = 10;

        private static Spell.Targeted Q = new Spell.Targeted(SpellSlot.Q, 550);
        private static Spell.Active W = new Spell.Active(SpellSlot.W, (uint)Player.Instance.GetAutoAttackRange());
        private static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 370, SkillShotType.Circular, 250, int.MaxValue, 330)
        {
            AllowedCollisionCount = int.MaxValue
        };
        private static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 1275, SkillShotType.Linear, 250, 1200, 80)
        {
            AllowedCollisionCount = 0
        };

        public static SpellSlot Ignite;
        public static SpellSlot Flash;

        #endregion

        #region Menu Settings

        #region Combo
        public static bool UseQCombo => FizzMenuCombo["UseQCombo"].Cast<CheckBox>().CurrentValue;
        public static bool UseWCombo => FizzMenuCombo["UseWCombo"].Cast<CheckBox>().CurrentValue;
        public static bool UseECombo => FizzMenuCombo["UseECombo"].Cast<CheckBox>().CurrentValue;
        public static bool UseRCombo => FizzMenuCombo["UseRCombo"].Cast<CheckBox>().CurrentValue;
        public static bool UseRECombo => FizzMenuCombo["UseRECombo"].Cast<CheckBox>().CurrentValue;
        public static int UseRChanceCombo => FizzMenuCombo["UseRChanceCombo"].Cast<ComboBox>().CurrentValue;
        public static bool UseFCombo => FizzMenuCombo["UseFCombo"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region Harass
        public static bool UseQHarass => FizzMenuHarass["UseQHarass"].Cast<CheckBox>().CurrentValue;
        public static bool UseWHarass => FizzMenuHarass["UseWHarass"].Cast<CheckBox>().CurrentValue;
        public static bool UseEHarass => FizzMenuHarass["UseEHarass"].Cast<CheckBox>().CurrentValue;
        public static int UseEHarassMode => FizzMenuHarass["UseEHarassMode"].Cast<ComboBox>().CurrentValue;
        #endregion

        #region Misc
        public static bool UseEMisc => FizzMenuMisc["UseEMisc"].Cast<CheckBox>().CurrentValue;
        public static int UseWMisc => FizzMenuMisc["UseWMisc"].Cast<ComboBox>().CurrentValue;
        public static bool KSQ => FizzMenuMisc["KSQ"].Cast<CheckBox>().CurrentValue;
        public static bool KSE => FizzMenuMisc["KSE"].Cast<CheckBox>().CurrentValue;
        public static bool KSR => FizzMenuMisc["KSR"].Cast<CheckBox>().CurrentValue;
        public static bool KSIgnite => FizzMenuMisc["KSIgnite"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region Drawing
        public static bool DrawQ => FizzMenuDrawing["DrawQ"].Cast<CheckBox>().CurrentValue;
        public static bool DrawE => FizzMenuDrawing["DrawE"].Cast<CheckBox>().CurrentValue;
        public static bool DrawR => FizzMenuDrawing["DrawR"].Cast<CheckBox>().CurrentValue;
        public static bool DrawRPred => FizzMenuDrawing["DrawRPred"].Cast<CheckBox>().CurrentValue;
        public static bool DrawDamage => FizzMenuDrawing["DrawDamage"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region Skin
        public static bool SkinEnabled => FizzMenuSkin["SkinEnabled"].Cast<CheckBox>().CurrentValue;
        public static int SelectedSkin => FizzMenuSkin["SelectedSkin"].Cast<ComboBox>().CurrentValue;
        #endregion

        #endregion

        #region Initializers
        public HesaFizz()
        {
            Chat.Print(string.Format("<b><font color=\"#FFFFFF\"></font></b><b><font color=\"#00e5e5\">Hesa Rizz V.{0}</font></b><b><font color=\"#FFFFFF\"></font></b><b><font color=\"#FFFFFF\"> Loaded!</font></b>", Environment.Version));
            InitializeMenu();
            InitializeEvents();
        }
        private void InitializeMenu()
        {
            FizzMenu = MainMenu.AddMenu("Hesa Fizz", "hesaFizz");
            //Combo
            FizzMenuCombo = FizzMenu.AddSubMenu("Combo", "hesaFizzCombo");
            FizzMenuCombo.Add("UseQCombo", new CheckBox("Use Q", true));
            FizzMenuCombo.Add("UseWCombo", new CheckBox("Use W", true));
            FizzMenuCombo.Add("UseECombo", new CheckBox("Use E", true));
            FizzMenuCombo.Add("UseRCombo", new CheckBox("Use R", true));
            FizzMenuCombo.Add("UseRECombo", new CheckBox("Use R then E if killable", true));
            FizzMenuCombo.Add("UseRChanceCombo", new ComboBox("Minimum R Hit Chance", 2, new string[] { "Low", "Medium", "High" }));
            FizzMenuCombo.Add("UseFCombo", new CheckBox("Use F if killable", false));
            //Harass
            FizzMenuHarass = FizzMenu.AddSubMenu("Harass", "hesaFizzHarass");
            FizzMenuHarass.Add("UseQHarass", new CheckBox("Use Q", true));
            FizzMenuHarass.Add("UseWHarass", new CheckBox("Use W", true));
            FizzMenuHarass.Add("UseEHarass", new CheckBox("Use E", true));
            FizzMenuHarass.Add("UseEHarassMode", new ComboBox("E Mode", 0, new string[] { "Back to Position", "Engage On Enemy" }));
            //Misc
            FizzMenuMisc = FizzMenu.AddSubMenu("Misc", "hesaFizzMisc");
            FizzMenuMisc.Add("UseEMisc", new CheckBox("Avoid Tower Shot using E", true));
            FizzMenuMisc.Add("UseWMisc", new ComboBox("Use W Order:", 0, new string[] { "Before Q", "After Q" }));
            FizzMenuMisc.Add("KSQ", new CheckBox("KS with Q", true));
            FizzMenuMisc.Add("KSE", new CheckBox("KS with E", true));
            FizzMenuMisc.Add("KSR", new CheckBox("KS with R", false));
            FizzMenuMisc.Add("KSIgnite", new CheckBox("KS with Ignite", true));
            //Drawing
            FizzMenuDrawing = FizzMenu.AddSubMenu("Drawing", "hesaFizzDrawing");
            FizzMenuDrawing.Add("DrawQ", new CheckBox("Draw Q Range", true));
            FizzMenuDrawing.Add("DrawE", new CheckBox("Draw E Range", true));
            FizzMenuDrawing.Add("DrawR", new CheckBox("Draw R Range", true));
            FizzMenuDrawing.Add("DrawRPred", new CheckBox("Draw R Prediction", true));
            FizzMenuDrawing.Add("DrawDamage", new CheckBox("Draw Combo Damage", true));
            //Skin
            FizzMenuSkin = FizzMenu.AddSubMenu("Skin", "hesaFizzSkin");
            FizzMenuSkin.Add("SkinEnabled", new CheckBox("Enable Skin Changer", false));
            FizzMenuSkin.Add("SelectedSkin", new ComboBox("Selected Skin:", 0, new string[] { "Default", "Atlantean Fizz", "Tundra Fizz", "Fisherman Fizz", "Void Fizz", "Chroma Yellow", "Chroma Dark Blue", "Chroma Red", "Cottontail Fizz", "Super Galaxy Fizz" }));
            //About
            FizzMenuAbout = FizzMenu.AddSubMenu("About", "hesaFizzAbout");
            FizzMenuAbout.AddGroupLabel("Fizz by Hesa!");
            FizzMenuAbout.AddSeparator();
            FizzMenuAbout.AddLabel("Hi guys, this is my simple fizz script, i will improve it with time.");
            FizzMenuAbout.AddLabel("Please report issues and do request on my Elobuddy thread.");
            FizzMenuAbout.AddSeparator();
            FizzMenuAbout.AddLabel("Paypal: h3xc0r3@gmail.com");
        }
        private void InitializeEvents()
        {
            Ignite = Player.Instance.GetSpellSlotFromName("SummonerDot");
            Flash = Player.Instance.GetSpellSlotFromName("SummonerFlash");
            RRectangle = new Geometry.Polygon.Rectangle(Player.Instance.Position, Player.Instance.Position, R.Width);
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Drawing.OnDraw += DrawingOnOnDraw;
            Drawing.OnEndScene += DrawingOnEndScene;
        }
        #endregion

        #region Functions
        private void DrawingOnEndScene(EventArgs args)
        {
            if (!DrawDamage) return;

            foreach (var enemy in EntityManager.Enemies.Where(ene => ene.IsInRange(Player.Instance, 1750) && ene.Type == Player.Instance.Type && ene.IsVisible))
            {
                var damage = DamageOnPlayer(enemy) * .85;
                var tempOffset = 0;
                if (enemy.BaseSkinName == "Annie") tempOffset -= 12;
                if (enemy.BaseSkinName == "Jhin") tempOffset -= 14;

                var damagePercentage = ((enemy.TotalShieldHealth() - 0.9 * damage) > 0 ? (enemy.TotalShieldHealth() - damage) : 0) / (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);
                var currentHealthPercentage = enemy.TotalShieldHealth() / (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);

                var startPoint = new Vector2((int)(enemy.HPBarPosition.X + BarOffset.X + damagePercentage * BarWidth), (int)(enemy.HPBarPosition.Y + BarOffset.Y + tempOffset) - 5);
                var endPoint = new Vector2((int)(enemy.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 1, (int)(enemy.HPBarPosition.Y + BarOffset.Y + tempOffset) - 5);

                Drawing.DrawLine(startPoint, endPoint, LineThickness, Color.Aqua);
            }
        }
        
        private void DrawingOnOnDraw(EventArgs args)
        {
            if (DrawQ) Drawing.DrawCircle(Player.Instance.Position, Q.Range, Q.IsReady() ? Color.Aqua : Color.Red);
            if (DrawE) Drawing.DrawCircle(Player.Instance.Position, E.Range, E.IsReady() ? Color.Aqua : Color.Red);
            if (DrawR) Drawing.DrawCircle(Player.Instance.Position, R.Range, R.IsReady() ? Color.Aqua : Color.Red);
            //
            var target = TargetSelector.SelectedTarget;
            if (target == null) target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (target != null && DrawRPred && R.IsReady() && target.IsValidTarget())
            {
                RRectangle.Start = Player.Instance.Position.To2D();
                RRectangle.End = R.GetPrediction(target).CastPosition.To2D();
                RRectangle.UpdatePolygon();
                RRectangle.Draw(Color.CornflowerBlue, 3);
            }
        }

        private void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name == "FizzPiercingStrike")
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    Core.DelayAction(() => {
                        W.Cast();
                    }, (int)(sender.Spellbook.CastEndTime - Game.Time) + Game.Ping / 2 + 250);
                else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && UseEHarassMode == 0)
                    Core.DelayAction(() => {
                        JumpBack = true;
                    }, (int)(sender.Spellbook.CastEndTime - Game.Time) + Game.Ping / 2 + 250);

            if (args.SData.Name == "fizzjumptwo" || args.SData.Name == "fizzjumpbuffer")
            {
                LastHarassPos = Vector3.Zero;
                JumpBack = false;
            }
        }
        
        private void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is Obj_AI_Turret && args.Target.IsMe && E.IsReady() && UseEMisc) E.Cast(Game.CursorPos);
        }

        private void Game_OnUpdate(EventArgs args)
        {
            if (Player.Instance.CanCast)
            {
                Killsteal();

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
                else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
            }

            if(SkinEnabled)
            {
                if(lastSkinSelected != SelectedSkin)
                {
                    lastSkinSelected = SelectedSkin;
                    Player.SetSkin(Player.Instance.CharData.BaseSkinName, lastSkinSelected);
                }
            }
            else
            {
                if(lastSkinSelected != 0)
                {
                    lastSkinSelected = 0;
                    Player.SetSkin(Player.Instance.CharData.BaseSkinName, lastSkinSelected);
                }
            }
        }

        private static float DamageOnPlayer(Obj_AI_Base target)
        {
            var damage = 0f;
            if (Q.IsReady()) damage += Player.Instance.GetSpellDamage(target, SpellSlot.Q);
            if (W.IsReady()) damage += Player.Instance.GetSpellDamage(target, SpellSlot.W);
            if (E.IsReady()) damage += Player.Instance.GetSpellDamage(target, SpellSlot.E);
            if (R.IsReady()) damage += Player.Instance.GetSpellDamage(target, SpellSlot.R);
            return damage;
        }

        private void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (!target.IsValidTarget()) return;
            if (LastHarassPos == null) LastHarassPos = ObjectManager.Player.ServerPosition;
            if (JumpBack) E.Cast(LastHarassPos);
            // Use W Before Q
            if (UseWHarass && W.IsReady() && UseWMisc == 0 && (Q.IsReady() || Player.Instance.IsInAutoAttackRange(target))) W.Cast();
            if (UseQHarass && Q.IsReady()) Q.Cast(target);
            if (UseEHarass && E.IsReady() && UseEHarassMode == 1) E.Cast(target);
        }

        private void Combo()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (!target.IsValidTarget()) return;

            if (UseRECombo && CanKillWithUltCombo(target) && Q.IsReady() && W.IsReady() && E.IsReady() && R.IsReady() && ((!UseFCombo && Player.Instance.Distance(target) < Q.Range + E.Range * 2) || (UseFCombo && Player.Instance.Spellbook.CanUseSpell(Flash) == SpellState.Ready && Player.Instance.Distance(target) < Q.Range + E.Range + 625 * 2)))
            {
                if(Player.Instance.Distance(target) < Q.Range + E.Range * 2)
                {
                    SmartRCast(target);
                    Core.DelayAction(() =>
                    {
                        E.Cast(Player.Instance.ServerPosition.Extend(target.ServerPosition, E.Range - 1).To3D());
                        E.Cast(Player.Instance.ServerPosition.Extend(target.ServerPosition, E.Range - 1).To3D());
                        W.Cast();
                        Q.Cast(target);
                    }, 100);
                }
                else
                {
                    Player.Instance.Spellbook.CastSpell(Flash, target);
                    Core.DelayAction(() =>
                    {
                        SmartRCast(target);
                        Core.DelayAction(() =>
                        {
                            E.Cast(Player.Instance.ServerPosition.Extend(target.ServerPosition, E.Range - 1).To3D());
                            E.Cast(Player.Instance.ServerPosition.Extend(target.ServerPosition, E.Range - 1).To3D());
                            W.Cast();
                            Q.Cast(target);
                        }, 100);
                    }, 100);
                }
            }
            else
            {
                if (UseRCombo && R.IsReady())
                {
                    if (Player.Instance.GetSpellDamage(target, SpellSlot.R) > target.Health) SmartRCast(target);
                    if (DamageOnPlayer(target) > target.Health) SmartRCast(target);
                    if ((Q.IsReady() || E.IsReady())) SmartRCast(target);
                    if (Player.Instance.IsInAutoAttackRange(target)) SmartRCast(target);
                }
                Core.DelayAction(() =>
                {
                    // Use W Before Q
                    if (UseWCombo && W.IsReady() && UseWMisc == 0 && (Q.IsReady() || Player.Instance.IsInAutoAttackRange(target))) W.Cast();
                    if (UseQCombo && Q.IsReady()) Q.Cast(target);
                    if (UseECombo && E.IsReady()) E.Cast(target);
                }, 100);
            }
        }

        private void Killsteal()
        {
            if (!KSQ && !KSE && !KSR && !KSIgnite) return;
            foreach (var enemy in EntityManager.Enemies.Where(ene => ene.IsInRange(Player.Instance, R.Range) && ene.Type == Player.Instance.Type && ene.IsVisible))
            {
                if (KSQ && Q.IsReady() && Q.IsInRange(enemy) && enemy.Health <= Q.GetSpellDamage(enemy)) Q.Cast(enemy);
                else if (KSE && E.IsReady() && E.IsInRange(enemy) && enemy.Health <= E.GetSpellDamage(enemy)) E.Cast(enemy);
                else if (KSR && R.IsReady() && R.IsInRange(enemy) && enemy.Health <= R.GetSpellDamage(enemy)) SmartRCast(enemy);
                else if (KSIgnite && Player.Instance.Spellbook.CanUseSpell(Ignite) == SpellState.Ready && enemy.IsValidTarget(600f, true) && IgniteDamage(enemy) >= enemy.Health) Player.Instance.Spellbook.CastSpell(Ignite, enemy);
            }
        }

        public static float IgniteDamage(Obj_AI_Base target)
        {
            if (Ignite == SpellSlot.Unknown || Player.Instance.Spellbook.CanUseSpell(Ignite) != SpellState.Ready) return 0f;
            return Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);
        }

        public static void SmartRCast(Obj_AI_Base target)
        {
            var prediction = R.GetPrediction(target);
            switch (UseRChanceCombo)
            {
                case 0:
                {
                    if(prediction.HitChance == HitChance.Low || prediction.HitChance == HitChance.Medium || prediction.HitChance == HitChance.High)
                    {
                        var castPosition = Player.Instance.ServerPosition.Extend(prediction.CastPosition, R.Range).To3D();
                        R.Cast(castPosition);
                    }
                }
                break;
                case 1:
                {
                    if (prediction.HitChance == HitChance.Medium || prediction.HitChance == HitChance.High)
                    {
                        var castPosition = Player.Instance.ServerPosition.Extend(prediction.CastPosition, R.Range).To3D();
                        R.Cast(castPosition);
                    }
                }
                break;
                case 2:
                {
                    if (prediction.HitChance == HitChance.High)
                    {
                        var castPosition = Player.Instance.ServerPosition.Extend(prediction.CastPosition, R.Range).To3D();
                        R.Cast(castPosition);
                    }
                }
                break;
            }
        }

        public static bool CanKillWithUltCombo(AIHeroClient target)
        {
            return target.Health <= (Player.Instance.GetSpellDamage(target, SpellSlot.Q) + Player.Instance.GetSpellDamage(target, SpellSlot.W) + Player.Instance.GetSpellDamage(target, SpellSlot.R));
        }
        #endregion
    }
}