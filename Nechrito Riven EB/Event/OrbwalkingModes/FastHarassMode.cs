namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;
    using EloBuddy;
    using EloBuddy.SDK;
    //using Orbwalking = NechritoRiven.Orbwalking;

    #endregion

    internal class FastHarassMode : NechritoRiven.Core.Core
    {
        #region Public Methods and Operators

        public static void FastHarass()
        {
            var target = TargetSelector.GetTarget(400, DamageType.Physical);

            if (target == null || !Spells.Q.IsReady() || !Spells.E.IsReady())
            {
                return;
            }

            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady() && Qstack == 1)
            {
                BackgroundData.CastQ(target);
            }

            if (Qstack == 3 && !Orbwalker.CanAutoAttack && Orbwalker.CanMove)//!Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                Spells.E.Cast(Game.CursorPos);

                EloBuddy.SDK.Core.DelayAction(() => Spells.W.Cast(), 170);
                EloBuddy.SDK.Core.DelayAction(() => Spells.Q.Cast(Game.CursorPos), 190);
            }
        }

        #endregion
    }
}
