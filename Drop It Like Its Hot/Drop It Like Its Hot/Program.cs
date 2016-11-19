using EloBuddy.SDK.Events;
using System;

namespace Drop_It_Like_Its_Hot
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            new DropItLikeItsHot();
        }
    }
}