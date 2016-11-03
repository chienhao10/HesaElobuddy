using EloBuddy;
using EloBuddy.SDK.Events;

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
            new AutoShop();
        }
    }
}
