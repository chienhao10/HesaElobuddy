namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using Core;
    using EloBuddy;
    using EloBuddy.SDK;

    #endregion

    internal class BurstMode : NechritoRiven.Core.Core
    {
        #region Public Methods and Operators

        public static void Burst()
        {
            if (Player.Spellbook.CanUseSpell(Spells.Flash) == SpellState.Ready && MenuConfig.AlwaysF)
            {
                var selectedTarget = TargetSelector.SelectedTarget;

                if (selectedTarget == null 
                    || !selectedTarget.IsValidTarget(Player.AttackRange + 625)
                    || Player.Distance(selectedTarget.Position) < Player.AttackRange
                    || (MenuConfig.Flash && selectedTarget.Health > Dmg.GetComboDamage(selectedTarget) && !Spells.R.IsReady())
                    || (!MenuConfig.Flash && (!Spells.R.IsReady() || !Spells.W.IsReady())))
                {
                    return;
                }

                Usables.CastYoumoo();
                Spells.E.Cast(selectedTarget.Position);
                Spells.R.Cast();
                EloBuddy.SDK.Core.DelayAction(BackgroundData.FlashW, 170);
            }
            else
            {
                var target = TargetSelector.GetTarget(Player.AttackRange + 360, DamageType.Physical);

                if (target == null) return;

                if (Spells.R.IsReady() && Spells.R.Name == IsSecondR && Qstack > 1)
                {
                    var pred = Spells.R.GetPrediction(
                        target);/*,
                        true,
                        collisionable: new[] { CollisionableObjects.YasuoWall });*/

                    if (pred.HitChance != EloBuddy.SDK.Enumerations.HitChance.High)
                    {
                        return;
                    }
                    Spells.R.Cast(pred.CastPosition);
                }

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(target.Position);
                }

                if (Spells.R.IsReady() && Spells.R.Name == IsFirstR)
                {
                    Spells.R.Cast();
                }

                if (!Spells.W.IsReady() || !BackgroundData.InRange(target))
                {
                    return;
                }

                BackgroundData.CastW(target);
                BackgroundData.DoubleCastQ(target);
            }
        }

        #endregion
    }
}
