namespace NechritoRiven
{
    #region

    using System;
    using EloBuddy;
    using EloBuddy.SDK.Events;

    #endregion

    public class Program
    {
        #region Methods

        private static void Main()
        {
            Loading.OnLoadingComplete += OnLoad; 
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Riven")
            {
                return;
            }
            Load.LoadAssembly();
        }

        #endregion
    }
}