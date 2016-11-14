namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;
    using EloBuddy;
    using EloBuddy.SDK;
    //using Orbwalking = Orbwalking;

    #endregion

    internal class Mixed : NechritoRiven.Core.Core
    {
        #region Public Methods and Operators

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(360, DamageType.Physical);

            if (target == null)
            {
                return;
            }

            if (Spells.Q.IsReady() && Spells.W.IsReady() && Qstack == 1)
            {
                BackgroundData.CastQ(target);
            }

            if (Spells.W.IsReady() && BackgroundData.InRange(target))
            {
                BackgroundData.CastW(target);
            }

            if (!Spells.Q.IsReady()
                || !Spells.E.IsReady()
                || Qstack != 3
                || Orbwalker.CanAutoAttack//Orbwalking.CanAttack()
                || !Orbwalker.CanMove)//!Orbwalking.CanMove(5))
            {
                return;
            }

            Spells.E.Cast(Game.CursorPos);
            EloBuddy.SDK.Core.DelayAction(() => Spells.Q.Cast(target.Position), 190);
        }

        #endregion
    }
}
