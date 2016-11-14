namespace NechritoRiven.Event.Misc
{
    #region

    using System;

    using Core;

    #endregion

    internal class Skinchanger : Core
    {
        #region Public Methods and Operators

        public static void Update(EventArgs args)
        {
            //Player.SetSkin(Player.CharData.BaseSkinName, MenuConfig.UseSkin  ? MenuConfig.SelectedSkinId : Player.SkinId);
        }

        #endregion
    }
}