using EloBuddy;
using EloBuddy.Sandbox;
using EloBuddy.SDK;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AutoShop.Controllers
{
    static class BuildController
    {
        public static Build CurrentBuild;
        public static string LoadedBuildName;
        private static string BuildPath = "";

        static BuildController()
        {
            var fileName = ObjectManager.Player.ChampionName + "_" + Game.MapId + ".txt";
            LoadBuild(fileName);
        }

        public static void LoadBuild(string buildName)
        {
            if(string.IsNullOrEmpty(buildName))
            {
                LoadGenericBuild();
                return;
            }else
            {
                var specialPath = SandboxConfig.DataDirectory + "AutoShop\\";
                var fullpath = Path.Combine(specialPath + buildName);
                if(!File.Exists(fullpath))
                {
                    LoadGenericBuild();
                    return;
                }
                else
                {
                    var build = GetBuildFromFile(fullpath);
                    if(build == null)
                    {
                        LoadGenericBuild();
                        return;
                    }
                    else
                    {
                        BuildPath = fullpath;
                        LoadedBuildName = buildName.Split('.')[0];
                        CurrentBuild = build;
                        Chat.Print("Loaded profile: " + LoadedBuildName);
                    }
                }
            }
        }

        public static void LoadGenericBuild()
        {
            var genericBuild = GetGenericBuildByChampion();
            var specialPath = SandboxConfig.DataDirectory + "AutoShop\\";
            var fullpath = Path.Combine(specialPath + genericBuild);

            if (!Directory.Exists(specialPath)) Directory.CreateDirectory(specialPath);

            if (!File.Exists(fullpath))
            {
                using (StreamWriter writer = new StreamWriter(fullpath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString(genericBuild.Split('.')[0]));
                }
            }
            LoadBuild(genericBuild);
        }

        public static void ReloadBuild()
        {
            if (string.IsNullOrEmpty(BuildPath) || !File.Exists(BuildPath)) return;

            var build = GetBuildFromFile(BuildPath);
            if(build != null)
            {
                LoadedBuildName = BuildPath.Split('.')[0];
                CurrentBuild = build;
            }
        }

        public static string GetGenericBuildByChampion()
        {
            var defaultBuild = "GenericADC.txt";

            var isJungler = ObjectManager.Player.GetSpellSlotFromName("summonersmite") != SpellSlot.Unknown;
            var isSupport = ObjectManager.Player.GetSpellSlotFromName("summonerexhaust") != SpellSlot.Unknown;

            if (isJungler)
            {
                switch(ObjectManager.Player.ChampionName.ToLower())
                {
                    //AP
                    case "diana":
                    case "ekko":
                    case "elise":
                    case "evelynn":
                    case "fiddlesticks":
                    case "fizz":
                    case "ivern":
                    case "kayle":
                    case "nidalee":
                    case "shaco":
                        return "GenericJungleAP.txt";
                    break;
                    //AD
                    case "aatrox":
                    case "graves":
                    case "irelia":
                    case "jax":
                    case "kindred":
                    case "rengar":
                    case "nocturne":
                        return "GenericJungleAD.txt";
                    break;
                    //AD Tank
                    case "drmundo":
                    case "leesin":
                    case "masteryi":
                    case "jarvaniv":
                    case "olaf":
                    case "vi":
                    case "volibear":
                    case "warwick":
                    case "xin zhao":
                    case "wukong":
                    case "udyr":
                    case "tryndamere":
                    case "trundle":
                    case "skarner":
                    case "tahm kench":
                    case "shyvana":
                    case "reksai":
                    case "rammus":
                    case "nautilus":
                    case "yorick":
                        return "GenericJungleADTank.txt";
                    break;
                    //AP Tank
                    case "amumu":
                    case "chogath":
                    case "gragas":
                    case "malphite":
                    case "maokai":
                    case "sejuani":
                    case "zac":
                        return "GenericJungleAPTank.txt";
                    break;
                }
            }else if(isSupport)
            {
                switch (ObjectManager.Player.ChampionName.ToLower())
                {
                    //AD Tank
                    case "alistar":
                    case "braum":
                    case "leona":
                    case "nautilus":
                    case "tahmkench":
                        return "GenericSupportADTank.txt";
                    break;
                    //AP Tank
                    case "bard":
                    case "blitzcrank":
                        return "GenericSupportAPTank.txt";
                    break;
                    //Ap Healer/Shielder
                    case "janna":
                    case "lulu":
                    case "nami":
                    case "sona":
                    case "soraka":
                        return "GenericSupportAPHeal.txt";
                    break;
                    //AP Burst
                    case "anivia":
                    case "annie":
                    case "brand":
                    case "ivern":
                    case "karma":
                    case "lux":
                    case "morgana":
                    case "zilean":
                    case "zyra":
                        return "GenericSupportAPBurst.txt";
                    break;
                }
            }else{
                switch (ObjectManager.Player.ChampionName.ToLower())
                {
                    //ADC
                    case "ashe":
                    case "caitlyn":
                    case "corki":
                    case "draven":
                    case "ezreal":
                    case "graves":
                    case "jhin":
                    case "jinx":
                    case "kalista":
                    case "kindred":
                    case "kogmaw":
                    case "lucian":
                    case "missfortune":
                    case "sivir":
                    case "tristana":
                    case "twitch":
                    case "urgot":
                    case "varus":
                    case "vayne":
                        return "GenericADC.txt";
                    break;
                    //Top AD
                    case "aatrox":
                    case "fiora":
                    case "gangplank":
                    case "jax":
                    case "jayce":
                    case "kled":
                    case "quinn":
                    case "riven":
                    case "khazix":
                    case "nocturne":
                    case "rengar":
                    case "shaco":
                    case "yasuo":
                        return "GenericTopAD.txt";
                    break;
                    //Top AD Tank
                    case "darius":
                    case "drmundo":
                    case "garen":
                    case "gnar":
                    case "illaoi":
                    case "irelia":
                    case "poppy":
                    case "rammus":
                    case "renekton":
                    case "shen":
                    case "sion":
                    case "trundle":
                    case "tryndamere":
                    case "jarvan iv":
                    case "hecarim":
                    case "leesin":
                    case "nautilus":
                    case "olaf":
                    case "pantheon":
                    case "shyvana":
                    case "reksai":
                    case "tahmkench":
                    case "vi":
                    case "volibear":
                    case "warwick":
                    case "wukong":
                    case "xinzhao":
                    case "yorick":
                        return "GenericTopADTank.txt";
                    break;
                    //Top AP
                    case "ryze":
                    case "teemo":
                    case "rumble":
                    case "kennen":
                        return "GenericTopAP.txt";
                    break;
                    //Top AP Tank
                    case "chogath":
                    case "malphite":
                    case "maokai":
                    case "mordekaiser":
                    case "nasus":
                    case "singed":
                    case "sejuani":
                    case "vladimir":
                    case "amumu":
                        return "GenericTopAPTank.txt";
                    break;
                    //Mid AD
                    case "talon":
                    case "zed":
                    return "GenericMidAD.txt";
                    break;
                    //Mid AP
                    case "ahri":
                    case "akali":
                    case "anivia":
                    case "annie":
                    case "aurelionsol":
                    case "azir":
                    case "bard":
                    case "blitzcrank":
                    case "brand":
                    case "cassiopeia":
                    case "diana":
                    case "ekko":
                    case "elise":
                    case "evelynn":
                    case "fiddlesticks":
                    case "fizz":
                    case "galio":
                    case "heimerdinger":
                    case "ivern":
                    case "janna":
                    case "karma":
                    case "karthus":
                    case "kassadin":
                    case "katarina":
                    case "kayle":
                    case "leblanc":
                    case "lissandra":
                    case "lulu":
                    case "lux":
                    case "malzahar":
                    case "morgana":
                    case "nami":
                    case "nidalee":
                    case "nunu":
                    case "orianna":
                    case "sona":
                    case "soraka":
                    case "swain":
                    case "syndra":
                    case "taliyah":
                    case "veigar":
                    case "velkoz":
                    case "viktor":
                    case "xerath":
                    case "ziggs":
                    case "zilean":
                    case "zyra":
                        return "GenericMidAP.txt";
                    break;
                }
            }
            return defaultBuild;
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

        public static void SaveBuildToFile(string filePath)
        {
            if (CurrentBuild == null) return;
            string output = JsonConvert.SerializeObject(CurrentBuild);
            if (string.IsNullOrEmpty(output)) return;

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(output);
                sw.Close();
            }
        }
        static int count = 50;
        static int randomNumber = new Random().Next(25, 50);
        public static void BuyOrSellItems()
        {
            if (CurrentBuild == null) return;
            count++;
            if (count < randomNumber) return;
            count = 0;
            randomNumber = new Random().Next(25, 50);
            //
            if (CurrentBuild.Items.Count != 0)
            {
                var buildItem = CurrentBuild.Items[0];
                if (buildItem != null)
                {
                    if (buildItem.Sell)
                    {
                        ItemController.SellItemId(buildItem.Id);
                        Chat.Print("Sold item: " + buildItem.Name);
                        CurrentBuild.Items.Remove(buildItem);
                    }
                    else
                    {
                        if (Item.HasItem(buildItem.Id, ObjectManager.Player))
                        {
                            lock (CurrentBuild.Items)
                            {
                                Chat.Print("Already had item: " + buildItem.Name);
                                Chat.Print("Removing item:" + CurrentBuild.Items[0].Name);
                                CurrentBuild.Items.RemoveAt(0);
                                Chat.Print("Items to buy count: " + CurrentBuild.Items.Count);
                            }
                        }
                        else
                        {
                            if (hasEnoughGold(buildItem))
                            {
                                ItemController.BuyItemId(buildItem.Id);
                                Chat.Print("Bought item: " + buildItem.Name);
                                lock (CurrentBuild.Items)
                                {
                                    Chat.Print("Removing item:" + CurrentBuild.Items[0].Name);
                                    CurrentBuild.Items.RemoveAt(0);
                                    Chat.Print("Items to buy count: " + CurrentBuild.Items.Count);
                                }
                            }
                            else
                            {
                                if (buildItem.BuildFrom.Count != 0)
                                {
                                    var itemToBuy = buildItem.BuildFrom[0];
                                    if (itemToBuy.BuildFrom.Count != 0)
                                    {
                                        var tmp = itemToBuy.BuildFrom[0];
                                        if (hasEnoughGold(tmp))
                                        {
                                            lock (itemToBuy.BuildFrom)
                                            {
                                                Chat.Print("Removed sub sub item:: " + tmp.Name);
                                                itemToBuy.BuildFrom.RemoveAt(0);
                                            }
                                            //Buy tmp
                                            ItemController.BuyItemId(tmp.Id);
                                            Chat.Print("Bought sub sub item: " + tmp.Name);
                                        }
                                    }
                                    else
                                    {
                                        if (hasEnoughGold(itemToBuy))
                                        {
                                            lock (itemToBuy.BuildFrom)
                                            {
                                                Chat.Print("Removed sub item: " + itemToBuy.Name);
                                                buildItem.BuildFrom.RemoveAt(0);

                                                ItemController.BuyItemId(itemToBuy.Id);
                                                Chat.Print("Bought sub item: " + itemToBuy.Name);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            PotionController.BuyOrSellPotions();
        }

        static bool hasEnoughGold(BuildItem item)
        {
            return ObjectManager.Player.Gold >= ItemController.GetItemCost(item.Id);
        }
        
    }
}