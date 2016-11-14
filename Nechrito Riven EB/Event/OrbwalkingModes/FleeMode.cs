namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using System.Linq;

    using Core;
    using EloBuddy.SDK;
    using EloBuddy;

    #endregion

    internal class FleeMode : NechritoRiven.Core.Core
    {
        #region Public Methods and Operators

        public static void Flee()
        {

            if (MenuConfig.WallFlee && Player.CountEnemiesInRange(1200) == 0)
            {
                var end = Player.ServerPosition.Extend(Game.CursorPos, 350).To3D();
                var isWallDash = FleeLogic.IsWallDash(end, 350);

                var eend = Player.ServerPosition.Extend(Game.CursorPos, 350).To3D();
                var wallE = FleeLogic.GetFirstWallPoint(Player.ServerPosition, eend);
                var wallPoint = FleeLogic.GetFirstWallPoint(Player.ServerPosition, end);

                Player.GetPath(wallPoint);

                if (Spells.Q.IsReady() && Qstack < 3)
                {
                    Spells.Q.Cast(Game.CursorPos);
                }

                if (Qstack != 3 || !isWallDash) return;

                if (Spells.E.IsReady() && wallPoint.Distance(Player.ServerPosition) <= Spells.E.Range)
                {
                    Spells.E.Cast(wallE);
                    EloBuddy.SDK.Core.DelayAction(() => Spells.Q.Cast(wallPoint), 190);
                }
               else if (wallPoint.Distance(Player.ServerPosition) <= 65)
                {
                    Spells.Q.Cast(wallPoint);
                }
                else
                {
                    EloBuddy.SDK.Orbwalker.MoveTo(wallPoint);
                }
            }
            else
            {
                var enemy = EntityManager.Enemies.Where(target => BackgroundData.InRange(target) && Spells.W.IsReady());

                var x = Player.Position.Extend(Game.CursorPos, 300).To3D();

                var targets = enemy as AIHeroClient[] ?? enemy.ToArray();

                if (Spells.W.IsReady() && targets.Any())
                {
                    foreach (var target in targets)
                    {
                        if (BackgroundData.InRange(target))
                        {
                            Spells.W.Cast();
                        }
                    }
                }

                if (Spells.Q.IsReady())// && !Player.IsDashing())
                {
                    Spells.Q.Cast(Game.CursorPos);
                }

                if (MenuConfig.FleeYomuu)
                {
                    Usables.CastYoumoo();
                }

                if (Spells.E.IsReady())// && !Player.IsDashing())
                {
                    Spells.E.Cast(x);
                }
            }
        }

        #endregion
    }
}
