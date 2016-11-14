namespace NechritoRiven.Draw
{
    #region

    using System;
    using System.Drawing;

    using Core;
    using EloBuddy.SDK.Rendering;
    using EloBuddy;

    #endregion

    internal class DrawMisc : Core
    {
        #region Public Methods and Operators

        public static void RangeDraw(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (MenuConfig.DrawCb)
            {
                if (Spells.E.IsReady())
                {
                    Circle.Draw(Spells.Q.IsReady() ? SharpDX.Color.DodgerBlue : SharpDX.Color.DarkSlateGray, 370 + Player.AttackRange, Player);
                }
                else
                {
                    Circle.Draw(Spells.Q.IsReady() ? SharpDX.Color.LightBlue : SharpDX.Color.DarkSlateGray, Player.AttackRange, Player);
                }
            }
            
            if (MenuConfig.DrawBt && Spells.Flash != SpellSlot.Unknown && Player.Spellbook.CanUseSpell(Spells.Flash) == SpellState.Ready)
            {
                Circle.Draw(SharpDX.Color.Orange, Player.AttackRange + 625, Player);
            }

            if (MenuConfig.DrawFh)
            {
                Circle.Draw(Spells.Q.IsReady() && Spells.E.IsReady() ? SharpDX.Color.LightBlue : SharpDX.Color.DarkSlateGray, Player.AttackRange + 450 + 70, Player);
            }

            if (MenuConfig.DrawHs)
            {
                Circle.Draw(Spells.Q.IsReady() && Spells.W.IsReady() ? SharpDX.Color.LightBlue : SharpDX.Color.DarkSlateGray, Player.AttackRange + 400, Player);
            }

            var pos = Drawing.WorldToScreen(Player.Position);

            if (MenuConfig.DrawAlwaysR)
            {
                Drawing.DrawText(pos.X - 20, pos.Y + 20, Color.DodgerBlue, "Use R1  (     )");
                Drawing.DrawText(pos.X + 43, pos.Y + 20, MenuConfig.AlwaysR  ? Color.Yellow : Color.Red, MenuConfig.AlwaysR ? "On" : "Off");
            }

            if (!MenuConfig.ForceFlash)
            {
                return;
            }

            Drawing.DrawText(pos.X - 20, pos.Y + 40, Color.DodgerBlue, "Use Flash  (     )");
            Drawing.DrawText(pos.X + 64, pos.Y + 40, MenuConfig.AlwaysF ? Color.Yellow : Color.Red, MenuConfig.AlwaysF ? "On" : "Off");
        }

        #endregion
    }
}