using EloBuddy;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using AutoShop.Properties;
using EloBuddy.SDK;
using System;

namespace AutoShop.Controllers
{
    public static class ItemController
    {
        public static LoLItem[] ItemDatabase;
        public static LoLItem[] AvailableItemDatabase;
        static bool initialized;

        public static void Initialize()
        {
            if (initialized) return;
            initialized = true;
            List<LoLItem> all = new List<LoLItem>();
            List<LoLItem> av = new List<LoLItem>();
            foreach (LoLItem loLItem in ParseItems())
            {
                all.Add(loLItem);
                if (loLItem.purchasable && loLItem.maps.Contains((int)Game.MapId) &&
                    (loLItem.requiredChampion == string.Empty || loLItem.requiredChampion == ObjectManager.Player.ChampionName))
                    av.Add(loLItem);
            }
            ItemDatabase = all.ToArray();
            AvailableItemDatabase = av.ToArray();
        }

        public static List<LoLItem> ParseItems()
        {
            JObject data = JObject.Parse(Resources.item);
            List<LoLItem> loLItems = new List<LoLItem>();
            foreach (JToken token in data.GetValue("data"))
            {
                JToken t = token.First;

                List<int> maps = new List<int>();
                if ((bool)t["maps"]["1"]) maps.Add(1);
                if ((bool)t["maps"]["8"]) maps.Add(8);
                if ((bool)t["maps"]["10"]) maps.Add(10);
                if ((bool)t["maps"]["11"]) maps.Add(11);
                if ((bool)t["maps"]["12"]) maps.Add(12);
                if ((bool)t["maps"]["14"]) maps.Add(14);

                List<int> fromItems = new List<int>();
                if (t["from"] != null)
                    fromItems.AddRange(t["from"].Select(tok => (int)tok));

                List<int> toItems = new List<int>();
                if (t["to"] != null)
                    toItems.AddRange(t["to"].Select(tok => (int)tok));

                List<string> tags = new List<string>();
                if (t["tags"] != null)
                    tags.AddRange(t["tags"].Select(tok => tok.ToString()));

                var name = t["name"].ToString();
                var description = t["description"].ToString();
                var sanitizedDescription = string.Empty;//t["sanitizedDescription"].ToString();
                var plaintext = t["plaintext"] == null ? string.Empty : t["plaintext"].ToString();
                var id = Convert.ToInt32(t.Path.ToString().Replace("data.", ""));
                var baseGold = (int)t["gold"]["base"];
                var totalGold = (int)t["gold"]["total"];
                var sellGold = (int)t["gold"]["sell"];
                var purchasable = (bool)t["gold"]["purchasable"];
                var requiredChampion = t["requiredChampion"] == null ? string.Empty : t["requiredChampion"].ToString();
                var depth = t["depth"] == null ? -1 : (int)t["depth"];
                var cq = t["cq"] == null ? string.Empty : t["cq"].ToString();
                var group = t["group"] == null ? string.Empty : t["group"].ToString();

                loLItems.Add(new LoLItem(name, description,
                    sanitizedDescription,
                    plaintext,
                    id, baseGold, totalGold, sellGold,
                    purchasable,
                    requiredChampion, maps.ToArray(),
                    fromItems.ToArray(), toItems.ToArray(), depth, tags.ToArray(),
                    cq,
                    group));
            }
            return loLItems;
        }

        public static LoLItem FindBestItem(string name)
        {
            return AvailableItemDatabase.OrderByDescending(it => it.name.Match(name)).First();
        }

        public static LoLItem FindBestItemAll(string name)
        {
            return ItemDatabase.OrderByDescending(it => it.name.Match(name)).First();
        }

        public static LoLItem FindItemByID(this LoLItem[] items, int id)
        {
            return items.OrderByDescending(it => it.id == id).First();
        }

        public static List<LoLItem> MyItems()
        {
            List<LoLItem> l = ObjectManager.Player.InventoryItems.Select(s => ItemDatabase.FindItemByID((int)s.Id)).ToList();
            l.Remove(l.FirstOrDefault(le => le.id == 1411)); //TODO !! Remove this when eb or rito will fix it
            return l;
        }

        public static LoLItem GetItemByID(int id)
        {
            return ItemDatabase.First(it => it.id == id);
        }

        public static int GetItemSlot(int id)
        {
            for (int i = 0; i < ObjectManager.Player.InventoryItems.Length; i++)
            {
                if ((int)ObjectManager.Player.InventoryItems[i].Id == id)
                    return i;
            }
            return -1;
        }

        public static int GetHealtlyConsumableSlot()
        {
            for (int i = 0; i < ObjectManager.Player.InventoryItems.Length; i++)
            {
                if (ObjectManager.Player.InventoryItems[i].Id.IsHealthlyConsumable())
                    return i;
            }
            return -1;
        }

        public static int GetHPotionSlot()
        {
            for (int i = 0; i < ObjectManager.Player.InventoryItems.Length; i++)
            {
                if (ObjectManager.Player.InventoryItems[i].Id.IsHPotion())
                    return i;
            }
            return -1;
        }

        public static bool IsHealthlyConsumable(this ItemId i)
        {

            return (int)i == 2003 || (int)i == 2009 || (int)i == 2010;
        }

        public static bool IsHPotion(this ItemId i)
        {

            return (int)i == 2003 || (int)i == 2009 || (int)i == 2010 || (int)i == 2031;
        }

        public static int GetItemCost(int itemId)
        {
            return new Item(itemId, 0).GoldRequired();
        }

        public static int HPotionCount()
        {
            var slot = GetHealtlyConsumableSlot();
            if (slot == -1) return 0;
            
            var item = ObjectManager.Player.InventoryItems.FirstOrDefault(x => x.Id.IsHPotion());
            if (item == null) return 5;
            
            return item.Stacks;
        }

        public static void BuyHPotion()
        {
            Shop.BuyItem(ItemId.Health_Potion);
        }

        public static void SellHPotion()
        {
            int slot = GetHPotionSlot();
            if (slot != -1)
            {
                Shop.SellItem(slot);
            }
        }

        public static void SellItem(int slot)
        {
            if(slot != -1)
            {
                Shop.SellItem(slot);
            }
        }

        public static void BuyItemId(int id)
        {
            Shop.BuyItem(id);
        }

        public static void SellItemId(int id)
        {
            var slot = GetItemSlot(id);
            if(slot != -1)
            {
                SellItem(slot);
            }
        }
    }
}