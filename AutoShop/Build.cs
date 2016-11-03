using System.Collections.Generic;

namespace AutoShop
{
    public class BuildItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool BuyNowIfWeCan { get; set; }

        public int TotalCost { get; set; }

        public int UpgradeCost { get; set; }

        public List<BuildItem> BuildFrom { get; set; }

        public bool Sell { get; set; }
    }

    public class Build
    {
        public bool UseHPotion { get; set; }
        public int MaxHPotionCount { get; set; }
        public int MaxHPotionLevel { get; set; }
        public List<BuildItem> Items { get; set; }
    }
}