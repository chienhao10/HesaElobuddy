using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace Hesa_Fizz
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if(Player.Instance.ChampionName.ToLower() == "fizz")
            {
                new HesaFizz();
            }
        }
    }
}