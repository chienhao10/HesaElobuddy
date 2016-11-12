using Automation.Enums;
using EloBuddy.SDK;

namespace Automation.Controllers
{
    public static class ShopController
    {
        private static bool isShopping = false;
        public static void Tick()
        {
            if(Automation.CurrentState == States.Shopping)
            {
                if(!isShopping)
                {
                    isShopping = true;
                    Shop();
                }
            }
        }

        public static bool IsShopping()
        {
            return false;
        }

        public static void Shop()
        {
            
        }
        
    }
}