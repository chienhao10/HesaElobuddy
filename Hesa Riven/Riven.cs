using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Linq;

namespace Hesa_Riven
{
    public class Riven
    {
        public static Menu Menu, ComboMenu, LaneMenu, MiscMenu, DrawMenu;
        private const string FirstR = "RivenFengShuiEngine";
        private const string SecondR = "RivenIzunaBlade";

        public static Spell.Active Q = new Spell.Active(SpellSlot.Q, 300);//150

        public static Spell.Active W
        {
            get { return new Spell.Active(SpellSlot.W, (uint)(70 + Player.Instance.BoundingRadius + (Player.Instance.HasBuff("RivenFengShuiEngine") ? 195 : 120))); }//200
        }

        public static Spell.Active E = new Spell.Active(SpellSlot.E, 300);//325
        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 45, DamageType.Physical)
        {
            AllowedCollisionCount = int.MaxValue
        };

        private static readonly SpellSlot Flash = Player.Instance.GetSpellSlotFromName("summonerFlash");
        public static Spell.Targeted F;

        #region Variables
        public static Text Timer, Timer2;
        private static int QStack = 1;
        private static bool forceQ;
        private static bool forceW;
        private static bool forceR;
        private static bool forceR2;
        private static bool forceItem;
        private static float LastQ;
        private static float LastR;
        private static AttackableUnit QTarget;
        #endregion

        #region Menu Values
        private static bool AlwaysR => ComboMenu["AlwaysR"].Cast<KeyBind>().CurrentValue;
        private static bool RKillable => ComboMenu["RKillable"].Cast<CheckBox>().CurrentValue;
        private static bool ComboW => ComboMenu["ComboW"].Cast<CheckBox>().CurrentValue;
        private static bool UseHesa => ComboMenu["UseHesa"].Cast<KeyBind>().CurrentValue;
        private static bool FlashW => ComboMenu["FlashW"].Cast<KeyBind>().CurrentValue;
        private static bool BurstEnabled => ComboMenu["DoBurst"].Cast<KeyBind>().CurrentValue;
        private static bool DrawAlwaysR => DrawMenu["DrawAlwaysR"].Cast<CheckBox>().CurrentValue;
        private static bool DrawTimer1 => DrawMenu["DrawTimer1"].Cast<CheckBox>().CurrentValue;
        private static bool DrawTimer2 => DrawMenu["DrawTimer2"].Cast<CheckBox>().CurrentValue;
        private static bool Dind => DrawMenu["Dind"].Cast<CheckBox>().CurrentValue;
        private static bool DrawCB => DrawMenu["DrawCB"].Cast<CheckBox>().CurrentValue;
        private static bool DrawHS => DrawMenu["DrawHS"].Cast<CheckBox>().CurrentValue;
        private static bool DrawBT => DrawMenu["DrawBT"].Cast<CheckBox>().CurrentValue;
        private static bool KillstealW => MiscMenu["KillstealW"].Cast<CheckBox>().CurrentValue;
        private static bool KillstealR => MiscMenu["KillstealR"].Cast<CheckBox>().CurrentValue;
        private static bool AutoShield => MiscMenu["AutoShield"].Cast<CheckBox>().CurrentValue;
        private static bool Shield => MiscMenu["Shield"].Cast<CheckBox>().CurrentValue;
        private static bool KeepQ => MiscMenu["KeepQ"].Cast<CheckBox>().CurrentValue;
        private static int QD => MiscMenu["QD"].Cast<Slider>().CurrentValue;
        private static int QLD => MiscMenu["QLD"].Cast<Slider>().CurrentValue;
        private static int AutoW => MiscMenu["AutoW"].Cast<Slider>().CurrentValue;
        private static bool RMaxDam => MiscMenu["RMaxDam"].Cast<CheckBox>().CurrentValue;
        private static int LaneW => MiscMenu["LaneW"].Cast<Slider>().CurrentValue;
        private static bool LaneE => MiscMenu["LaneE"].Cast<CheckBox>().CurrentValue;
        private static bool WInterrupt => MiscMenu["WInterrupt"].Cast<CheckBox>().CurrentValue;
        private static bool Qstrange => MiscMenu["Qstrange"].Cast<CheckBox>().CurrentValue;
        private static bool FirstHydra => MiscMenu["FirstHydra"].Cast<CheckBox>().CurrentValue;
        private static bool LaneQ => MiscMenu["LaneQ"].Cast<CheckBox>().CurrentValue;
        private static bool Youmu => MiscMenu["youmu"].Cast<CheckBox>().CurrentValue;
        #endregion

        public Riven()
        {
            Chat.Print("Hesa Riven v" + Environment.Version + " has loaded.");

            InitializeMenu();

            var loadingTime = new Random().Next(5, 10);
            Chat.Print(string.Format("Hesa Riven will start in {0} seconds.", loadingTime));
            Core.DelayAction(Start, loadingTime * 1000);
        }

        void InitializeMenu()
        {
            Menu = MainMenu.AddMenu("Hesa Riven", "hesa_riven", "Hesa Riven");

            ComboMenu = Menu.AddSubMenu("Combo", "hesa_riven_combo", "Combo");
            ComboMenu.AddLabel("Combo Settings");
            ComboMenu.Add("UseHesa", new KeyBind("Use Hesa Combo Logic (Toggle)", true, KeyBind.BindTypes.PressToggle, 'L'));
            ComboMenu.Add("DoBurst", new KeyBind("Enter Burst Mode", false, KeyBind.BindTypes.PressToggle, 'T'));
            ComboMenu.Add("ComboW", new CheckBox("Use W", true));
            ComboMenu.Add("RKillable", new CheckBox("Use R When Target Is Killable", true));
            ComboMenu.Add("FlashW", new CheckBox("Flash W When Target Is Killable And R In Cooldown", true));

            LaneMenu = Menu.AddSubMenu("Lane", "hesa_riven_lane", "Lane Settings");
            LaneMenu.Add("LaneQ", new CheckBox("Use Q While Laneclear", true));
            LaneMenu.Add("LaneW", new Slider("Use W X Minion (0 = Don't)", 5, 0, 5));
            LaneMenu.Add("LaneE", new CheckBox("Use E While Laneclear", true));

            MiscMenu = Menu.AddSubMenu("Misc", "hesa_riven_misc", "Misc Settings");
            MiscMenu.Add("youmu", new CheckBox("Use Youmus When E", false));
            MiscMenu.Add("FirstHydra", new CheckBox("Flash Burst Hydra Cast before W", false));
            MiscMenu.Add("Qstrange", new CheckBox("Strange Q For Speed", false));
            MiscMenu.Add("Winterrupt", new CheckBox("W interrupt", true));
            MiscMenu.Add("AutoW", new Slider("Auto W When x Enemy", 5, 0, 5));
            MiscMenu.Add("RMaxDam", new CheckBox("Use Second R Max Damage", true));
            MiscMenu.Add("killstealw", new CheckBox("Killsteal W", true));
            MiscMenu.Add("killstealr", new CheckBox("Killsteal Second R", true));
            MiscMenu.Add("AutoShield", new CheckBox("Auto Cast E", true));
            MiscMenu.Add("Shield", new CheckBox("Auto Cast E While LastHit", true));
            MiscMenu.Add("KeepQ", new CheckBox("Keep Q Alive", true));
            MiscMenu.Add("QD", new Slider("First,Second Q Delay", 29, 23, 43));
            MiscMenu.Add("QLD", new Slider("Third Q Delay", 39, 36, 53));

            DrawMenu = Menu.AddSubMenu("Draw", "hesa_riven_draw", "Draw Settings");
            DrawMenu.Add("DrawDamage", new CheckBox("Draw Damage Healthbar", true));
            DrawMenu.Add("DrawAlwaysR", new CheckBox("Draw Always R Status", true));
            DrawMenu.Add("DrawTimer1", new CheckBox("Draw Q Expiry Time", true));
            DrawMenu.Add("DrawTimer2", new CheckBox("Draw R Expiry Time", true));
            DrawMenu.Add("Dind", new CheckBox("Draw Damage Indicator", true));
            DrawMenu.Add("DrawCB", new CheckBox("Draw Combo Engage Range", false));
            DrawMenu.Add("DrawBT", new CheckBox("Draw Burst Engage Range", false));
            DrawMenu.Add("DrawHS", new CheckBox("Draw Harass Engage Range", false));

            Menu CreditMenu = Menu.AddSubMenu("Credit", "hesa_riven_credit", "Credit");
            CreditMenu.AddLabel("Made by Hesa!");
            CreditMenu.AddLabel("This addon is currently in beta.");
            CreditMenu.AddLabel("If you find any issue please report.");
            CreditMenu.AddLabel("You will find me on the Elobuddy forum.");
        }

        void Start()
        {
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name == "SummonerFlash")
            {
                F = new Spell.Targeted(SpellSlot.Summoner1, 425);
            }
            else if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name == "SummonerFlash")
            {
                F = new Spell.Targeted(SpellSlot.Summoner2, 425);
            }
            InitializeEvents();
            
        }

        void InitializeEvents()
        {
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Obj_AI_Base.OnProcessSpellCast += OnCast;
            Obj_AI_Base.OnSpellCast += OnDoCast;
            Obj_AI_Base.OnSpellCast += OnDoCastLC;
            Obj_AI_Base.OnPlayAnimation += OnPlay;
            Obj_AI_Base.OnProcessSpellCast += OnEnemyCasting;
        }

        private void OnEnemyCasting(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && sender.Type == Player.Instance.Type && (AutoShield || (Shield && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))))
            {
                var epos = Player.Instance.ServerPosition + (Player.Instance.ServerPosition - sender.ServerPosition).Normalized() * 300;

                if (Player.Instance.Distance(sender.ServerPosition) <= args.SData.CastRange)
                {
                    switch (args.SData.TargettingType)
                    {
                        case SpellDataTargetType.Unit:

                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && !args.SData.Name.Contains("NasusW"))
                            {
                                if (E.IsReady()) E.Cast(epos);
                            }
                        }
                        break;
                        case SpellDataTargetType.SelfAoe:

                        if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                        {
                            if (E.IsReady()) E.Cast(epos);
                        }
                        break;
                    }
                    if (args.SData.Name.Contains("IreliaEquilibriumStrike"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (W.IsReady() && InWRange(sender)) W.Cast();
                            else if (E.IsReady()) E.Cast(epos);
                        }
                    }
                    if (args.SData.Name.Contains("TalonCutthroat"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (W.IsReady()) W.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RenektonPreExecute"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (W.IsReady()) W.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("GarenRPreCast"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast(epos);
                        }
                    }
                    if (args.SData.Name.Contains("GarenQAttack"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("XenZhaoThrust3"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (W.IsReady()) W.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarQ"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDash"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("RengarPassiveBuffDashAADummy"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("TwitchEParticle"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("FizzPiercingStrike"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("HungeringStrike"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("YasuoDash"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaRTrigger"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (W.IsReady() && InWRange(sender)) W.Cast();
                            else if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("YasuoDash"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("KatarinaE"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (W.IsReady()) W.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingSpinToWin"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                            else if (W.IsReady()) W.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                    if (args.SData.Name.Contains("MonkeyKingQAttack"))
                    {
                        if (args.Target.NetworkId == Player.Instance.NetworkId)
                        {
                            if (E.IsReady()) E.Cast();
                        }
                    }
                }
            }
        }

        private void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;
            
            switch (args.Animation)
            {
                case "Spell1a":
                {
                    LastQ = Environment.TickCount;
                    if (Qstrange && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None)) Player.DoEmote(Emote.Dance);
                    QStack = 2;
                    if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None) && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Core.DelayAction(() => Reset(), QD * 10);
                }
                break;
                case "Spell1b":
                {
                    LastQ = Environment.TickCount;
                    if (Qstrange && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None)) Player.DoEmote(Emote.Dance);
                    QStack = 3;
                    if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None) && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Core.DelayAction(() => Reset(), (QD * 10) + 1);
                }
                break;
                case "Spell1c":
                {
                    LastQ = Environment.TickCount;
                    if (Qstrange && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None)) Player.DoEmote(Emote.Dance);
                    QStack = 1;
                    if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None) && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Core.DelayAction(() => Reset(), (QLD * 10) + 3);
                }
                break;
                case "Spell3":
                {
                    if ((//Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Burst ||
                        Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                        //Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                        Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) && Youmu) CastYoumoo();
                }
                break;
                case "Spell4a":
                {
                    LastR = Environment.TickCount;
                }
                break;
                case "Spell4b":
                {
                    var target = TargetSelector.SelectedTarget;
                    if (Q.IsReady() && target.IsValidTarget()) ForceCastQ(target);
                }
                break;
            }
        }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            //if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;
            if (!sender.IsMe || !Orbwalker.IsAutoAttacking) return;
            QTarget = (Obj_AI_Base)args.Target;

            if (args.Target is Obj_AI_Minion)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    var Mobs = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, 120 + 70 + Player.Instance.BoundingRadius).ToList();
                    //var Mobs = MinionManager.GetMinions(120 + 70 + Player.BoundingRadius, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    if (Mobs.Count != 0)
                    {
                        if (HasTitan())
                        {
                            CastTitan();
                            return;
                        }
                        if (Q.IsReady())
                        {
                            ForceItem();
                            Core.DelayAction(() => ForceCastQ(Mobs[0]), 1);
                        }
                        else if (W.IsReady())
                        {
                            ForceItem();
                            Core.DelayAction(() => ForceW(), 1);
                        }
                        else if (E.IsReady())
                        {
                            E.Cast(Mobs[0].Position);
                        }
                    }
                }
            }
            if (args.Target is Obj_AI_Turret || args.Target is Obj_Barracks || args.Target is Obj_BarracksDampener || args.Target is Obj_Building) if (args.Target.IsValid && args.Target != null && Q.IsReady() && LaneQ && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) ForceCastQ((Obj_AI_Base)args.Target);
            if (args.Target is AIHeroClient && args.Target.Type == Player.Instance.Type)
            {
                var target = (AIHeroClient)args.Target;
                if (KillstealR && R.IsReady() && R.Name == SecondR) if (target.Health < (Rdame(target, target.Health) + Player.Instance.GetAutoAttackDamage(target)) && target.Health > Player.Instance.GetAutoAttackDamage(target)) R.Cast(target.Position);
                
                if (KillstealW && W.IsReady()) if (target.Health < (Player.Instance.GetSpellDamage(target, SpellSlot.W) + Player.Instance.GetAutoAttackDamage(target)) && target.Health > Player.Instance.GetAutoAttackDamage(target)) W.Cast();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (Q.IsReady())
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceCastQ(target), 1);
                    }
                    else if (W.IsReady() && InWRange(target))
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceW(), 1);
                    }
                    else if (E.IsReady() && !Player.Instance.IsInAutoAttackRange(target)) E.Cast(target.Position);
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (W.IsReady() && InWRange(target))
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceW(), 1);
                        Core.DelayAction(() => ForceCastQ(target), 2);
                    }
                    else if (Q.IsReady())
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceCastQ(target), 1);
                    }
                    else if (E.IsReady() && !Player.Instance.IsInAutoAttackRange(target) && !InWRange(target))
                    {
                        E.Cast(target.Position);
                    }
                }
                /*
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (QStack == 2 && Q.IsReady())
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceCastQ(target), 1);
                    }
                }*/
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (R.IsReady() && R.Name == SecondR)
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceR2(), 1);
                    }
                    else if (Q.IsReady())
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceCastQ(target), 1);
                    }
                }
            }
        }
        private void OnDoCastLC(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            //if (!sender.IsMe || !Orbwalking.IsAutoAttack((args.SData.Name))) return;
            if (!sender.IsMe || !Orbwalker.IsAutoAttacking) return;
            QTarget = (Obj_AI_Base)args.Target;
            if (args.Target is Obj_AI_Minion)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    var Minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, 70 + 120).ToList();
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (Q.IsReady() && LaneQ)
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceCastQ(Minions[0]), 1);
                    }
                    if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) && W.IsReady() && LaneW != 0 &&
                        Minions.Count >= LaneW)
                    {
                        ForceItem();
                        Core.DelayAction(() => ForceW(), 1);
                    }
                    if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) && (!W.IsReady() || (W.IsReady() && LaneW == 0) || Minions.Count < LaneW) &&
                        E.IsReady() && LaneE)
                    {
                        E.Cast(Minions[0].Position);
                        Core.DelayAction(() => ForceItem(), 1);
                    }
                }
            }
        }

        private void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.Name.Contains("ItemTiamatCleave")) forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) forceW = false;
            if (args.SData.Name == FirstR) forceR = false;
            if (args.SData.Name == SecondR) forceR2 = false;
        }


        const float _xOffset = 2;
        const float _yOffset = 9;
        const float _barLength = 104;

        private void Drawing_OnEndScene(EventArgs args)
        {
            if (Player.Instance.IsDead)
                return;

            if (!DrawMenu["DrawDamage"].Cast<CheckBox>().CurrentValue) return;
            foreach (var aiHeroClient in EntityManager.Heroes.Enemies)
            {
                if (aiHeroClient.Distance(Player.Instance) < 1000)
                {
                    var pos = new Vector2(aiHeroClient.HPBarPosition.X + _xOffset, aiHeroClient.HPBarPosition.Y + _yOffset);
                    var fullbar = (_barLength) * (aiHeroClient.HealthPercent / 100);
                    var damage = (_barLength) * ((getComboDamage(aiHeroClient) / aiHeroClient.MaxHealth) > 1 ? 1 : (getComboDamage(aiHeroClient) / aiHeroClient.MaxHealth));
                    EloBuddy.SDK.Rendering.Line.DrawLine(System.Drawing.Color.Gray, new Vector2(pos.X, pos.Y), new Vector2(pos.X + (damage > fullbar ? fullbar : damage), pos.Y));
                    EloBuddy.SDK.Rendering.Line.DrawLine(System.Drawing.Color.Black, new Vector2(pos.X + (damage > fullbar ? fullbar : damage) - 2, pos.Y), new Vector2(pos.X + (damage > fullbar ? fullbar : damage) + 2, pos.Y));
                }
                else
                {
                    return;
                }
            }
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead)
                return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);
            if (QStack != 1 && DrawTimer1)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y - 200, System.Drawing.Color.White, "Q Expiry =>  " + ((double)(LastQ - Environment.TickCount + 3800) / 1000).ToString("0.0") + "S");
            }
            if (Player.Instance.HasBuff("RivenFengShuiEngine") && DrawTimer2)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y - 180, System.Drawing.Color.White, "R Expiry =>  " + ((double)(LastR - Environment.TickCount + 15000) / 1000).ToString("0.0") + "S");
            }
            if (DrawCB) Circle.Draw(E.IsReady() ? Color.DodgerBlue : Color.IndianRed, E.Range + Player.Instance.AttackRange + 70, Player.Instance);
            if (DrawBT && Flash != SpellSlot.Unknown) Circle.Draw(R.IsReady() && Player.Instance.Spellbook.CanUseSpell(Flash) == SpellState.Ready ? Color.YellowGreen : Color.IndianRed, 800, Player.Instance);
            if (DrawHS) Circle.Draw(Q.IsReady() && W.IsReady() ? Color.PeachPuff : Color.IndianRed, 400, Player.Instance);
            if (DrawAlwaysR)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y + 20, System.Drawing.Color.White, "Always R  (     )");
                Drawing.DrawText(heropos.X + 34, heropos.Y + 20, AlwaysR ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red, AlwaysR ? "On" : "Off");
            }
            if(BurstEnabled)
            {
                Drawing.DrawText(heropos.X - 45, heropos.Y + 40, System.Drawing.Color.Red, "BURST MODE!!!");
            }
        }

        private void Game_OnTick(EventArgs args)
        {
            ForceSkill();
            UseRMaxDam();
            AutoUseW();
            Killsteal();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (BurstEnabled) Burst(); else Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Jungleclear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Flee();
            if (Environment.TickCount - LastQ >= 3650 && QStack != 1 && !Player.Instance.IsRecalling() && KeepQ && Q.IsReady()) Q.Cast(Game.CursorPos);
        }

        private void Combo()
        {
            var targetR = TargetSelector.GetTarget(250 + Player.Instance.AttackRange + 70, DamageType.Physical);
            if (targetR != null && targetR.IsValidTarget() && targetR.IsEnemy && !targetR.IsZombie && !targetR.IsMinion && !targetR.IsMonster && !targetR.IsMe && targetR.PlayerControlled && targetR.Type == Player.Instance.Type)
            {
                if (R.IsReady() && R.Name == FirstR && Player.Instance.IsInAutoAttackRange(targetR) && AlwaysR) ForceR();
                if (R.IsReady() && R.Name == FirstR && W.IsReady() && InWRange(targetR) && ComboW && AlwaysR)
                {
                    ForceR();
                    Core.DelayAction(ForceW, 1);
                }
                if (W.IsReady() && InWRange(targetR) && ComboW) W.Cast();
                if (UseHesa && R.IsReady() && R.Name == FirstR && W.IsReady() && E.IsReady() && targetR.IsValidTarget() && !targetR.IsZombie && (IsKillableR(targetR) || AlwaysR))
                {
                    if (!InWRange(targetR))
                    {
                        E.Cast(targetR.Position);
                        ForceR();
                        Core.DelayAction(ForceW, 200);
                        Core.DelayAction(() => ForceCastQ(targetR), 305);
                    }
                }
                else if (!UseHesa && R.IsReady() && R.Name == FirstR && W.IsReady() && targetR.IsEnemy && E.IsReady() && targetR.IsValidTarget() && !targetR.IsZombie && (IsKillableR(targetR) || AlwaysR))
                {
                    if (!InWRange(targetR))
                    {
                        E.Cast(targetR.Position);
                        ForceR();
                        Core.DelayAction(ForceW, 200);
                    }
                }
                else if (UseHesa && W.IsReady() && E.IsReady())
                {
                    if (!InWRange(targetR))
                    {
                        E.Cast(targetR.Position);
                        Core.DelayAction(ForceItem, 10);
                        Core.DelayAction(ForceW, 200);
                        Core.DelayAction(() => ForceCastQ(targetR), 305);
                    }
                }
                else if (!UseHesa && W.IsReady() && E.IsReady())
                {
                    if (targetR.IsValidTarget() && !InWRange(targetR))
                    {
                        E.Cast(targetR.Position);
                        Core.DelayAction(ForceItem, 10);
                        Core.DelayAction(ForceW, 240);
                    }
                }
                else if (E.IsReady())
                {
                    if (!InWRange(targetR))
                    {
                        E.Cast(targetR.Position);
                    }
                }
            }
        }

        private void Burst()
        {
            var target = TargetSelector.SelectedTarget;
            if (target != null && target.IsValidTarget() && target.IsEnemy && !target.IsZombie && !target.IsMinion && !target.IsMonster && !target.IsMe && target.PlayerControlled)
            {
                if (R.IsReady() && R.Name == FirstR && W.IsReady() && E.IsReady() && Player.Instance.Distance(target.Position) <= 250 + 70 + Player.Instance.AttackRange)
                {
                    E.Cast(target.Position);
                    CastYoumoo();
                    ForceR();
                    Core.DelayAction(ForceW, 100);
                }
                else if (R.IsReady() && R.Name == FirstR && E.IsReady() && W.IsReady() && Q.IsReady() && Player.Instance.Distance(target.Position) <= 400 + 70 + Player.Instance.AttackRange)
                {
                    E.Cast(target.Position);
                    CastYoumoo();
                    ForceR();
                    Core.DelayAction(() => ForceCastQ(target), 150);
                    Core.DelayAction(ForceW, 160);
                }
                else if (IsFlashReady && R.IsReady() && R.Name == FirstR && (Player.Instance.Distance(target.Position) <= 800) && (!FirstHydra || (FirstHydra && !HasItem())))
                {
                    E.Cast(target.Position);
                    CastYoumoo();
                    ForceR();
                    Core.DelayAction(DoFlashW, 180);
                }
                else if (IsFlashReady && R.IsReady() && E.IsReady() && W.IsReady() && R.Name == FirstR && (Player.Instance.Distance(target.Position) <= 800) && FirstHydra && HasItem())
                {
                    E.Cast(target.Position);
                    ForceR();
                    Core.DelayAction(ForceItem, 100);
                    Core.DelayAction(DoFlashW, 210);
                }
            }
        }

        private bool IsFlashReady {  get { return Player.Instance.Spellbook.CanUseSpell(Flash) == SpellState.Ready; } }

        private void Harass()
        {
            var target = TargetSelector.GetTarget(400, DamageType.Physical);
            if (Q.IsReady() && W.IsReady() && E.IsReady() && QStack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Core.DelayAction(ForceW, 1);
                }
            }
            if (Q.IsReady() && E.IsReady() && QStack == 3 && !Orbwalker.CanAutoAttack && Orbwalker.CanMove)
            {
                var epos = Player.Instance.ServerPosition + (Player.Instance.ServerPosition - target.ServerPosition).Normalized() * 300;
                E.Cast(epos);
                Core.DelayAction(() => Q.Cast(epos), 190);
            }
        }

        private void Flee()
        {
            var enemy = EntityManager.Enemies.Where(hero => hero.IsValidTarget(Player.Instance.HasBuff("RivenFengShuiEngine") ? 70 + 195 + Player.Instance.BoundingRadius : 70 + 120 + Player.Instance.BoundingRadius) && W.IsReady());
            var x = Player.Instance.Position.Extend(Game.CursorPos, 300);
            if (W.IsReady() && enemy.Any()) foreach (var target in enemy) if (InWRange(target) && target.Type == Player.Instance.Type) W.Cast();
        }

        private void Jungleclear()
        {
            var Mobs = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, 250 + Player.Instance.AttackRange + 70).ToList();
            
            if (Mobs.Count <= 0)
                return;

            if (W.IsReady() && E.IsReady() && !Player.Instance.IsInAutoAttackRange(Mobs[0]))
            {
                E.Cast(Mobs[0].Position);
                Core.DelayAction(ForceItem, 1);
                Core.DelayAction(ForceW, 200);
            }
        }

        public static void LaneClear()
        {
            try
            {
                Orbwalker.ForcedTarget = null;
                {
                    if (Orbwalker.IsAutoAttacking || LastQ + 260 > Environment.TickCount) return;
                    var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValidTarget(W.Range));
                    foreach ( var minion in minions)
                    {
                        if (LaneMenu["LaneQ"].Cast<CheckBox>().CurrentValue && Q.IsReady() &&
                            minion.Health <=
                            Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, Q.GetSpellDamage(minion)))
                        {
                            Player.CastSpell(SpellSlot.Q, minion.Position);
                            return;
                        }
                        if (LaneMenu["LaneW"].Cast<Slider>().CurrentValue >= minions.Count() && W.IsReady() &&
                            minion.Health <=
                            Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical, W.GetSpellDamage(minion)))
                        {
                            Player.CastSpell(SpellSlot.W);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

        }

        public bool IsKillableR(AIHeroClient target)
        {
            if (RKillable && target.IsValidTarget() && (totaldame(target) >= target.Health
                 && basicdmg(target) <= target.Health) || Player.Instance.CountEnemiesInRange(900) >= 2 && (!target.HasBuff("kindrednodeathbuff") && !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention")))
            {
                return true;
            }
            return false;
        }

        private bool HasItem() => (Item.HasItem(3074) && Item.CanUseItem(3074)) || (Item.HasItem(3748) && Item.CanUseItem(3748)) || (Item.HasItem(3077) && Item.CanUseItem(3077));

        private double totaldame(Obj_AI_Base target)
        {
            if (target != null)
            {
                double dmg = 0;
                double passivenhan = 0;
                if (Player.Instance.Level >= 18) { passivenhan = 0.5; }
                else if (Player.Instance.Level >= 15) { passivenhan = 0.45; }
                else if (Player.Instance.Level >= 12) { passivenhan = 0.4; }
                else if (Player.Instance.Level >= 9) { passivenhan = 0.35; }
                else if (Player.Instance.Level >= 6) { passivenhan = 0.3; }
                else if (Player.Instance.Level >= 3) { passivenhan = 0.25; }
                else { passivenhan = 0.2; }
                if (HasItem()) dmg = dmg + Player.Instance.GetAutoAttackDamage(target) * 0.7;
                if (W.IsReady()) dmg = dmg + Player.Instance.GetSpellDamage(target, SpellSlot.W);
                if (Q.IsReady())
                {
                    var qnhan = 4 - QStack;
                    dmg = dmg + Player.Instance.GetSpellDamage(target, SpellSlot.Q) * qnhan + Player.Instance.GetAutoAttackDamage(target) * qnhan * (1 + passivenhan);
                }
                dmg = dmg + Player.Instance.GetAutoAttackDamage(target) * (1 + passivenhan);
                if (R.IsReady())
                {
                    var rdmg = Rdame(target, target.Health - dmg * 1.2);
                    return dmg * 1.2 + rdmg;
                }
                return dmg;
            }
            return 0;
        }

        private double basicdmg(Obj_AI_Base target)
        {
            if (target != null)
            {
                double dmg = 0;
                double passivenhan = 0;
                if (Player.Instance.Level >= 18) { passivenhan = 0.5; }
                else if (Player.Instance.Level >= 15) { passivenhan = 0.45; }
                else if (Player.Instance.Level >= 12) { passivenhan = 0.4; }
                else if (Player.Instance.Level >= 9) { passivenhan = 0.35; }
                else if (Player.Instance.Level >= 6) { passivenhan = 0.3; }
                else if (Player.Instance.Level >= 3) { passivenhan = 0.25; }
                else { passivenhan = 0.2; }
                if (HasItem()) dmg = dmg + Player.Instance.GetAutoAttackDamage(target) * 0.7;
                if (W.IsReady()) dmg = dmg + Player.Instance.GetSpellDamage(target, SpellSlot.W);
                if (Q.IsReady())
                {
                    var qnhan = 4 - QStack;
                    dmg = dmg + Player.Instance.GetSpellDamage(target, SpellSlot.Q) * qnhan + Player.Instance.GetAutoAttackDamage(target) * qnhan * (1 + passivenhan);
                }
                dmg = dmg + Player.Instance.GetAutoAttackDamage(target) * (1 + passivenhan);
                return dmg;
            }
            return 0;
        }

        private float getComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                float passivenhan = 0;
                if (Player.Instance.Level >= 18) { passivenhan = 0.5f; }
                else if (Player.Instance.Level >= 15) { passivenhan = 0.45f; }
                else if (Player.Instance.Level >= 12) { passivenhan = 0.4f; }
                else if (Player.Instance.Level >= 9) { passivenhan = 0.35f; }
                else if (Player.Instance.Level >= 6) { passivenhan = 0.3f; }
                else if (Player.Instance.Level >= 3) { passivenhan = 0.25f; }
                else { passivenhan = 0.2f; }
                if (HasItem()) damage = damage + (float)Player.Instance.GetAutoAttackDamage(enemy) * 0.7f;
                if (W.IsReady()) damage = damage + Player.Instance.GetSpellDamage(enemy, SpellSlot.W);
                if (Q.IsReady())
                {
                    var qnhan = 4 - QStack;
                    damage = damage + Player.Instance.GetSpellDamage(enemy, SpellSlot.Q) * qnhan + (float)Player.Instance.GetAutoAttackDamage(enemy) * qnhan * (1 + passivenhan);
                }
                damage = damage + (float)Player.Instance.GetAutoAttackDamage(enemy) * (1 + passivenhan);
                if (R.IsReady())
                {
                    return damage * 1.2f + Player.Instance.GetSpellDamage(enemy, SpellSlot.R);
                }
                return damage;
            }
            return 0;
        }

        private void Reset()
        {
            Player.DoEmote(Emote.Dance);
            Orbwalker.MoveTo(Player.Instance.Position.Extend(Game.CursorPos, Player.Instance.Distance(Game.CursorPos) + 10).To3D());
        }

        private int GetWRange => Player.Instance.HasBuff("RivenFengShuiEngine") ? 330 : 265;

        private void AutoUseW()
        {
            if (AutoW > 0)
            {
                if (Player.Instance.CountEnemiesInRange(GetWRange) >= AutoW)
                {
                    ForceW();
                }
            }
        }

        private void UseRMaxDam()
        {
            if (RMaxDam && R.IsReady() && R.Name == SecondR)
            {
                var targets = EntityManager.Enemies.Where(x => x.IsValidTarget(R.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health / target.MaxHealth <= 0.25 && (!target.HasBuff("kindrednodeathbuff") || !target.HasBuff("Undying Rage") || !target.HasBuff("JudicatorIntervention")))
                        R.Cast(target.Position);
                }
            }
        }

        private void ForceR()
        {
            forceR = (R.IsReady() && R.Name == FirstR);
            Core.DelayAction(() =>
            {
                forceR = false;
            }, 500);
        }

        private void ForceR2()
        {
            forceR2 = R.IsReady() && R.Name == SecondR;
            Core.DelayAction(() =>
            {
                forceR2 = false;
            }, 500);
        }

        private void ForceW()
        {
            forceW = W.IsReady();
            Core.DelayAction(() =>
            {
                forceW = false;
            }, 500);
        }

        private void ForceCastQ(AttackableUnit target)
        {
            forceQ = true;
            QTarget = target;
        }

        private int AnyItem => Item.CanUseItem(3077) && Item.HasItem(3077) ? 3077 : Item.CanUseItem(3074) && Item.HasItem(3074) ? 3074 : 0;

        private void ForceSkill()
        {
            if (forceQ && QTarget != null && QTarget.IsValidTarget(E.Range + Player.Instance.BoundingRadius + 70) && Q.IsReady()) Q.Cast(QTarget.Position);
            if (forceW) W.Cast();
            if (forceR && R.Name == FirstR) R.Cast();
            if (forceItem && Item.CanUseItem(AnyItem) && Item.HasItem(AnyItem) && AnyItem != 0) Item.UseItem(AnyItem);
            if (forceR2 && R.Name == SecondR)
            {
                var target = TargetSelector.SelectedTarget;
                if (target != null) R.Cast(target.Position);
            }
            if (F == null) return;
            //Flash R
            //Chat.Print(Player.Instance.GetSpellDamage(Player.Instance, SpellSlot.R, DamageLibrary.SpellStages.SecondCast) + "");

            if (Player.Instance.Level < 6 || !R.IsReady() || R.Name != SecondR || !F.IsReady() || !E.IsReady()) return;

            foreach(var enemy in EntityManager.Enemies.Where(x => !x.IsMe && !x.IsMinion && !x.IsMonster))// && x.PlayerControlled))
            {
                var damage = Player.Instance.GetSpellDamage(enemy, SpellSlot.R);
                Chat.Print("Enemy: " + enemy.Name + " Health: " + enemy.Health + "DMG: " + damage);
                if((enemy.Health + enemy.AttackShield) < damage)
                {
                    var distance = Player.Instance.Distance(enemy);
                    Chat.Print(enemy.Name + " " + distance);
                    if (distance < (R.Range + E.Range + F.Range) && distance > (R.Range + E.Range))
                    {
                        Chat.Print("We can kill: " + enemy.Name);
                        E.Cast(enemy.Position);
                        Core.DelayAction(() => Player.Instance.Spellbook.CastSpell(Flash, enemy.Position), 1);
                        
                        Core.DelayAction(() => R.Cast(enemy.Position), 2);
                    }
                }
            }
        }

        private void ForceItem()
        {
            if (Item.CanUseItem(AnyItem) && Item.HasItem(AnyItem) && AnyItem != 0) forceItem = true;
            Core.DelayAction(() =>
            {
                forceItem = false;
            }, 500);
        }

        private bool HasTitan() => (Item.HasItem(3748) && Item.CanUseItem(3748));

        private void CastTitan()
        {
            if (HasTitan())
            {
                Item.UseItem(3748);
            }
        }

        private void CastYoumoo() { if (Item.HasItem(3142) && Item.CanUseItem(3142)) Item.UseItem(3142); }
        
        private void DoFlashW()
        {
            var target = TargetSelector.SelectedTarget;
            if (target != null && target.IsValidTarget() && target.IsEnemy && !target.IsZombie)
            {
                W.Cast(target);
                Core.DelayAction(() =>
                {
                    Player.Instance.Spellbook.CastSpell(Flash, target.Position);
                }, 10);
            }
        }

        private static void Killsteal()
        {
            if (KillstealW && W.IsReady())
            {
                var targets = EntityManager.Enemies.Where(x => x.IsValidTarget(R.Range) && !x.IsZombie && x.Type == Player.Instance.Type);
                foreach (var target in targets)
                {
                    if (target.Health < W.GetSpellDamage(target) && InWRange(target))
                    {
                        Chat.Print("KS with W!");
                        W.Cast();
                    }
                }
            }
            if (KillstealR && R.IsReady() && R.Name == SecondR)
            {
                var targets = EntityManager.Enemies.Where(x => x.IsValidTarget(R.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Rdame(target, target.Health) && (!target.HasBuff("kindrednodeathbuff") && !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention")))
                        R.Cast(target.Position);
                }
            }
        }

        public static void OnInterruptableTarget(AIHeroClient sender)//, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!sender.IsEnemy || !sender.IsValidTarget(W.Range) || sender.HasBuff("FioraW"))
            {
                return;
            }
            if (W.IsReady())
            {
                W.Cast(sender);
            }
            if (QStack != 3)
            {
                return;
            }
            Q.Cast(sender);
        }

        private static bool InWRange(GameObject target) => (Player.HasBuff("RivenFengShuiEngine") && target != null) ? 330 >= Player.Instance.Distance(target.Position) : 265 >= Player.Instance.Distance(target.Position);

        private static double Rdame(Obj_AI_Base target, double health)
        {
            if (target != null)
            {
                var missinghealth = (target.MaxHealth - health) / target.MaxHealth > 0.75 ? 0.75 : (target.MaxHealth - health) / target.MaxHealth;
                var pluspercent = missinghealth * (8 / 3);
                var rawdmg = new double[] { 80, 120, 160 }[R.Level - 1] + 0.6 * Player.Instance.FlatPhysicalDamageMod;
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float) rawdmg * (float)(1 + pluspercent));
            }
            return 0;
        }
        
    }
}