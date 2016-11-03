using EloBuddy;

namespace AutoShop.Controllers
{
    public static class PotionController
    {
        static int count = 0;
        public static void BuyOrSellPotions()
        {
            if (BuildController.CurrentBuild == null || !BuildController.CurrentBuild.UseHPotion) return;
            if(ObjectManager.Player.Level <= BuildController.CurrentBuild.MaxHPotionLevel)
            {
                //Buy Healing Potions
                if (ItemController.HPotionCount() >= BuildController.CurrentBuild.MaxHPotionCount) return;
                if(count == 0)
                    ItemController.BuyHPotion();
                count++;
                if (count >= 100) count = 0;
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