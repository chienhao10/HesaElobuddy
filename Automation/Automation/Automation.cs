using EloBuddy;
using EloBuddy.SDK.Menu;
using System;

namespace Automation
{
    internal class Automation
    {
        public static Menu MyMenu;
        public Automation()
        {
            InitializeMenu();
            InitializeEvents();
        }

        private static void InitializeMenu()
        {
            MyMenu = MainMenu.AddMenu("Automation", "hesa_automation", "Automation");
            MyMenu.AddGroupLabel("Automation is a full-afk bot, it will play for you like a decent human player!");
            MyMenu.AddLabel("Any and all suggestions are welcome.");
            MyMenu.AddSeparator(400);
        }

        private static void InitializeEvents()
        {
            Game.OnTick += OnTick;
            Drawing.OnEndScene += Drawings;
        }

        private static void OnTick(EventArgs args)
        {

        }

        private static void Drawings(EventArgs args)
        {

        }
    }
}