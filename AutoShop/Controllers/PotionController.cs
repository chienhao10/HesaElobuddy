using EloBuddy;
using System;

namespace AutoShop.Controllers
{
    public static class PotionController
    {
        static int count = 0;
        static int randomNumber = new Random().Next(75, 125);
        public static void BuyOrSellPotions()
        {
            if (BuildController.CurrentBuild == null || !BuildController.CurrentBuild.UseHPotion) return;
            if(ObjectManager.Player.Level <= BuildController.CurrentBuild.MaxHPotionLevel)
            {
                //Buy Healing Potions
                if (ItemController.HPotionCount() >= BuildController.CurrentBuild.MaxHPotionCount) return;
                if(count == 0)
                {
                    ItemController.BuyHPotion();
                    randomNumber = new Random().Next(75, 125);
                }
                count++;
                if (count >= randomNumber) count = 0;
            }
            else
            {
                //Sell Healing Potions if any...
                if (ItemController.HPotionCount() > 0)
                {
                    ItemController.SellHPotion();
                }
            }
        }
    }
}