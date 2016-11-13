using AutoShopGUI.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoShopGUI.Controllers
{
    public static class ItemController
    {
        public static LoLItem[] ItemDatabase;
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
            }
            ItemDatabase = all.ToArray();
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
                var baseGold = (int) t["gold"]["base"];
                var totalGold = (int)t["gold"]["total"];
                var sellGold = (int)t["gold"]["sell"];
                var purchasable = (bool) t["gold"]["purchasable"];
                var requiredChampion = t["requiredChampion"] == null ? string.Empty : t["requiredChampion"].ToString();
                var depth = t["depth"] == null ? -1 : (int) t["depth"];
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
        
        public static LoLItem FindBestItemAll(string name)
        {
            return ItemDatabase.OrderByDescending(it => it.name.Match(name)).First();
        }

        public static LoLItem FindItemByID(this LoLItem[] items, int id)
        {
            return items.OrderByDescending(it => it.id == id).First();
        }
        
        public static LoLItem GetItemByID(int id)
        {
            return ItemDatabase.First(it => it.id == id);
        }
    }
}
