using AutoShopGUI;
using AutoShopGUI.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AutoShop.Controllers
{
    static class BuildController
    {
        public static Build CurrentBuild;
        
        public static void NewBuild()
        {
            CurrentBuild = new Build()
            {
                UseHPotion = false,
                MaxHPotionCount = 3,
                MaxHPotionLevel = 11,
                Items = new List<BuildItem>()
            };
        }

        public static Build GetBuildFromFile(string filePath)
        {
            Build build = null;
            if (!File.Exists(filePath)) return build;
            string input = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                input = reader.ReadToEnd();
                reader.Close();
            }
            if(!string.IsNullOrEmpty(input)) build = JsonConvert.DeserializeObject<Build>(input);

            return build;
        }

        public static bool SaveBuildToFile(string filePath)
        {
            if (CurrentBuild == null) return false;
            string output = JsonConvert.SerializeObject(CurrentBuild);
            if (string.IsNullOrEmpty(output)) return false;

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(output);
                sw.Close();
            }
            return true;
        }

        public static BuildItem AddItem(int itemId, bool sell, bool buyNowIfWeCan = false)
        {
            var item = ItemController.GetItemByID(itemId);
            if (item == null) return null;
            var buildItem = new BuildItem()
            {
                Id = item.id,
                Name = item.name,
                BuyNowIfWeCan = buyNowIfWeCan,
                BuildFrom = GetSubItems(item),
                TotalCost = item.totalGold,
                UpgradeCost = item.baseGold,
                Sell = sell
            };
            CurrentBuild.Items.Add(buildItem);
            return buildItem;
        }

        public static void RemoveItem(int index)
        {
            if(index > -1 && index <= (CurrentBuild.Items.Count -1))
            {
                CurrentBuild.Items.RemoveAt(index);
            }
        }

        public static List<BuildItem> GetSubItems(LoLItem item)
        {
            var returnList = new List<BuildItem>();

            foreach(var subItemId in item.fromItems)
            {
                var lolItem = ItemController.GetItemByID(subItemId);
                var buildItem = new BuildItem()
                {
                    Id = lolItem.id,
                    Name = lolItem.name,
                    BuyNowIfWeCan = false,
                    BuildFrom = GetSubItems(lolItem),
                    TotalCost = lolItem.totalGold,
                    UpgradeCost = lolItem.baseGold
                };
                returnList.Add(buildItem);
            }

            return returnList;
        }

        public static void MoveUp(List<int> indexes)
        {
            var parent = CurrentBuild.Items;
            var item = CurrentBuild.Items[indexes[0]];
            indexes.RemoveAt(0);

            foreach(var i in indexes)
            {
                parent = item.BuildFrom;
                item = item.BuildFrom[i];
            }

            var itemIndex = parent.IndexOf(item);
            parent.Move(itemIndex, MoveDirection.Up);
        }

        public static void MoveDown(List<int> indexes)
        {
            var parent = CurrentBuild.Items;
            var item = CurrentBuild.Items[indexes[0]];
            indexes.RemoveAt(0);

            foreach (var i in indexes)
            {
                parent = item.BuildFrom;
                item = item.BuildFrom[i];
            }

            var itemIndex = parent.IndexOf(item);
            parent.Move(itemIndex, MoveDirection.Down);
        }

    }
}