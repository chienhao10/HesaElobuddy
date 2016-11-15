namespace NechritoRiven.Event.Misc
{
    #region

    using System;

    using NechritoRiven.Core;
    using NechritoRiven.Event.OrbwalkingModes;

    //using Orbwalking = Orbwalking;
    using EloBuddy;

    #endregion

    internal class PermaActive : Core
    {
        #region Public Methods and Operators

        private static void QMove()
        {
            if (!MenuConfig.QMove || !Spells.Q.IsReady())
            {
                return;
            }
            //EloBuddy.SDK.Core.DelayAction(() => Spells.Q.Cast(Player.Position - 15), Game.Ping + 2);
            EloBuddy.SDK.Core.DelayAction(() => Player.Spellbook.CastSpell(SpellSlot.Q, Game.CursorPos), Game.Ping + 2);
        }

        public static void Update(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (Environment.TickCount - LastQ >= 3650 - Game.Ping && MenuConfig.KeepQ
                
                //&& !Player.InFountain()//TODO: Figure if this exist in Elobuddy
                && !Player.HasBuff("Recall")
                && Player.HasBuff("RivenTriCleave"))
            {
                Player.Spellbook.CastSpell(SpellSlot.Q, Game.CursorPos);
            }

            QMove();

            BackgroundData.ForceSkill();
            /*
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    ComboMode.Combo();
                    break;
                case Orbwalking.OrbwalkingMode.Burst:
                    BurstMode.Burst();
                    break;
                case Orbwalking.OrbwalkingMode.Flee:
                    FleeMode.Flee();
                    break;
                case Orbwalking.OrbwalkingMode.FastHarass:
                    FastHarassMode.FastHarass();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Mixed.Harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    JungleClearMode.Jungleclear();
                    LaneclearMode.Laneclear();
                    break;
            }*/
            switch (EloBuddy.SDK.Orbwalker.ActiveModesFlags)
            {
                case EloBuddy.SDK.Orbwalker.ActiveModes.Combo:
                    if (MenuConfig.BurstEnabled) BurstMode.Burst(); else ComboMode.Combo();
                break;
                case EloBuddy.SDK.Orbwalker.ActiveModes.Flee:
                    FleeMode.Flee();
                break;
                case EloBuddy.SDK.Orbwalker.ActiveModes.Harass:
                    if (MenuConfig.FastHarass) FastHarassMode.FastHarass(); else Mixed.Harass();
                break;
                case EloBuddy.SDK.Orbwalker.ActiveModes.JungleClear:
                case EloBuddy.SDK.Orbwalker.ActiveModes.LaneClear:
                    JungleClearMode.Jungleclear();
                    LaneclearMode.Laneclear();
                break;
            }
        }
        #endregion
    }
}