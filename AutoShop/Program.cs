using EloBuddy.SDK.Events;
using System;

namespace AutoShop
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoaded;
        }

        private static void OnLoaded(System.EventArgs args)
        {
            try
            {
                new AutoShop();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
