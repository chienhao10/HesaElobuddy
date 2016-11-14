namespace NechritoRiven.Core
{
    #region

    using EloBuddy.SDK;

    #endregion

    internal class Usables : Core
    {
        #region Public Methods and Operators

        private static bool HasTitanAndIsReady() => (Item.HasItem(3748) && Item.CanUseItem(3748));
        private static bool HasRavenousHydraAndIsReady() => (Item.HasItem(3074) && Item.CanUseItem(3074));
        private static bool HasTiamatAndIsReady() => (Item.HasItem(3074) && Item.CanUseItem(3074));

        public static void CastHydra()
        {
            if (HasRavenousHydraAndIsReady())
            {
                Item.UseItem(3074);
            }else if (HasTitanAndIsReady())
            {
                Item.UseItem(3748);
            }else if (HasTiamatAndIsReady())
            {
                Item.UseItem(3074);
            }
        }

        public static void CastYoumoo()
        {
            if (Item.HasItem(3142) && Item.CanUseItem(3142))
                Item.UseItem(3142);
        }

        #endregion
    }
}