using EloBuddy;

namespace AutoShop.Controllers
{
    public static class PotionController
    {
        public static void BuyOrSellPotions()
        {
            if (BuildController.CurrentBuild == null || !BuildController.CurrentBuild.UseHPotion) return;
            if (ObjectManager.Player.Level <= BuildController.CurrentBuild.MaxHPotionLevel)
            {
                //Buy Healing Potions
                if (ItemController.HPotionCount() >= BuildController.CurrentBuild.MaxHPotionCount) return;
                ItemController.BuyHPotion();
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