namespace NechritoRiven.Draw
{
    #region

    using System;
    using System.Linq;

    using Core;

    using SharpDX;
    using EloBuddy.SDK;
    using EloBuddy;

    #endregion

    internal class DrawDmg
    {
        private static readonly HpBarIndicator Indicator = new HpBarIndicator();

        public static void DmgDraw(EventArgs args)
        {
            foreach (var enemy in EntityManager.Enemies.Where(ene => ene.IsInRange(Player.Instance, 1750)))
            {
                if (!MenuConfig.Dind || ObjectManager.Player.IsDead)
                {
                    return;
                }
                Indicator.Unit = enemy;
                Indicator.DrawDmg(Dmg.GetComboDamage(enemy),  enemy.Health <= Dmg.GetComboDamage(enemy) * .85 ? Color.LawnGreen : Color.Yellow);
            }
        }
    }
}