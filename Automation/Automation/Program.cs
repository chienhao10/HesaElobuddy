using EloBuddy.SDK.Events;
namespace Automation
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoaded;
        }

        private static void OnLoaded(System.EventArgs args)
        {
            new Automation();
        }
    }
}