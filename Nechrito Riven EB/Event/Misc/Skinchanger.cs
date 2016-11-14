namespace NechritoRiven.Event.Misc
{
    #region

    using System;

    using Core;

    #endregion

    internal class Skinchanger : Core
    {
        #region Public Methods and Operators

        private static int lastSkinId = -1;

        public static void Update(EventArgs args)
        {
            var skinId = MenuConfig.UseSkin ? MenuConfig.SelectedSkinId : Player.SkinId;
            if(lastSkinId != skinId)
            {
                lastSkinId = skinId;
                Player.SetSkin(Player.CharData.BaseSkinName, skinId);
            }
        }

        #endregion
    }
}