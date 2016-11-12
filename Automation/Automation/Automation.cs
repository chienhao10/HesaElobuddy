using Automation.Controllers;
using Automation.Enums;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using System;
using System.Threading.Tasks;

namespace Automation
{
    internal class Automation
    {
        public static States CurrentState = States.Shopping;
        public static Menu MyMenu, DrawingMenu, RecallMenu;

        public Automation()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private void Loading_OnLoadingComplete(EventArgs args)
        {
            Task.Factory.StartNew(async () => {
                await Task.Delay(new Random().Next(5, 10) * 1000);
                Initialize();
            });
        }

        private void Initialize()
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

            DrawingMenu = MyMenu.AddSubMenu("Drawing Settings", "hesa_automation_drawing", "Automation Drawing Settings");



            RecallMenu = MyMenu.AddSubMenu("Recall Settings", "hesa_automation_recall", "Automation Recall Settings");

        }

        private static void InitializeEvents()
        {
            Game.OnTick += OnTick;
            Drawing.OnEndScene += Drawings;
        }

        private static void OnTick(EventArgs args)
        {
            ShopController.Tick();
            PhaseController.Tick();
            LaneController.Tick();
            GankController.Tick();
            TeleportController.Tick();
            MovementController.Tick();
            WardController.Tick();
            RecallController.Tick();
        }

        private static void Drawings(EventArgs args)
        {
            DrawingController.Draw();
        }
    }
}