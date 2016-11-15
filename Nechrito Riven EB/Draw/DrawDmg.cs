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

        private static Vector2 BarOffset = new Vector2(0, 15);
        private const int BarWidth = 104;
        private const int LineThickness = 10;

        public static void DmgDraw(EventArgs args)
        {
            foreach (var enemy in EntityManager.Enemies.Where(ene => ene.IsInRange(Player.Instance, 1750) && ene.Type == Player.Instance.Type && ene.IsVisible))
            {
                if (!MenuConfig.Dind || ObjectManager.Player.IsDead)
                {
                    return;
                }

                var damage = Dmg.GetComboDamage(enemy) * .85;
                //Chat.Print(enemy.BaseSkinName);
                var tempOffset = 0;
                if (enemy.BaseSkinName == "Annie") tempOffset -= 12;
                if (enemy.BaseSkinName == "Jhin") tempOffset -= 14;

                var damagePercentage = ((enemy.TotalShieldHealth() - 0.9 * damage) > 0 ? (enemy.TotalShieldHealth() - damage) : 0) / (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);
                var currentHealthPercentage = enemy.TotalShieldHealth() / (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);

                var startPoint = new Vector2((int)(enemy.HPBarPosition.X + BarOffset.X + damagePercentage * BarWidth), (int)(enemy.HPBarPosition.Y + BarOffset.Y + tempOffset) - 5);
                var endPoint = new Vector2((int)(enemy.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 1, (int)(enemy.HPBarPosition.Y + BarOffset.Y + tempOffset) - 5);

                Drawing.DrawLine(startPoint, endPoint, LineThickness, System.Drawing.Color.Chartreuse);

                //Indicator.Unit = enemy;
                //Indicator.DrawDmg(Dmg.GetComboDamage(enemy),  enemy.Health <= Dmg.GetComboDamage(enemy) * .85 ? Color.LawnGreen : Color.Yellow);
            }
        }
    }
}