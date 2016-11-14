namespace NechritoRiven.Event.Interrupters_Etc
{
    using Core;
    #region

    using EloBuddy;
    using EloBuddy.SDK;
    using System;

    #endregion

    internal class ProcessSpell : NechritoRiven.Core.Core
    {
        #region Public Methods and Operators

        public static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy)// || !sender.IsValid(1000))//TODO: Fix this...
            {
                return;
            }

            if (Spells.E.IsReady())
            {
                if (BackgroundData.AntigapclosingSpells.Contains(args.SData.Name) || (BackgroundData.TargetedSpells.Contains(args.SData.Name) && args.Target.IsMe))
                {
                    EloBuddy.SDK.Core.DelayAction(() => Spells.E.Cast(Game.CursorPos), 120);
                }
            }

            if (!BackgroundData.InterrupterSpell.Contains(args.SData.Name) || !Spells.W.IsReady() || !BackgroundData.InRange(sender))
            {
                return;
            }

            BackgroundData.CastW(sender);
        }

        public static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Spells.Q.IsReady())
                {
                    if (Qstack == 0 || !Orbwalker.IsAutoAttacking)
                    {

                        Spells.Q.Cast(target.Position);

                    }

                    if (Qstack == 1 || !Orbwalker.IsAutoAttacking)
                    {

                        Spells.Q.Cast(target.Position);

                    }

                    if (Qstack == 2 || !Orbwalker.IsAutoAttacking)
                    {

                        Spells.Q.Cast(target.Position);
                        EloBuddy.SDK.Core.DelayAction(Animation.Animation.Reset, Animation.Animation.ResetDelay(MenuConfig.Qld));
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && target.Type == GameObjectType.AIHeroClient)
            {
                if (Spells.Q.IsReady())
                {
                    if (Qstack == 0 || !Orbwalker.IsAutoAttacking)
                    {

                        Spells.Q.Cast(target.Position);

                    }

                    if (Qstack == 1 || !Orbwalker.IsAutoAttacking)
                    {

                        Spells.Q.Cast(target.Position);

                    }

                    if (Qstack == 2 || !Orbwalker.IsAutoAttacking)
                    {

                        Spells.Q.Cast(target.Position);

                    }
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Spells.Q.IsReady())
                {
                    if (Qstack == 0 || !Orbwalker.IsAutoAttacking)
                    {
                        Spells.Q.Cast(target.Position);
                    }

                    if (Qstack == 1 || !Orbwalker.IsAutoAttacking)
                    {
                        Spells.Q.Cast(target.Position);
                    }

                    // if (CountQ == 2 || !Orbwalker.IsAutoAttacking)
                    //{
                    //   Q.Cast(target.Position);
                    //}
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (MenuConfig.JnglQ && Spells.Q.IsReady())
                {
                    if (Qstack == 0 || !Orbwalker.IsAutoAttacking)
                    {
                        Spells.Q.Cast(target.Position);
                    }

                    if (Qstack == 1 || !Orbwalker.IsAutoAttacking)
                    {
                        Spells.Q.Cast(target.Position);
                    }

                    if (Qstack == 2 || !Orbwalker.IsAutoAttacking)
                    {
                        Spells.Q.Cast(target.Position);
                    }
                }
            }
        }

        #endregion
    }
}