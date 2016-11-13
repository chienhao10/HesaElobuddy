using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace Hesa_Riven
{
    class Program
    {
        private static void Main() => Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            switch(Player.Instance.ChampionName)
            {
                case "Riven":
                {
                    new Riven();
                }
                break;
            }
        }
    }
}