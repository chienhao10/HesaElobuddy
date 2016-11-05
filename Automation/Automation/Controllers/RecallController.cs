using EloBuddy;
using EloBuddy.SDK;

namespace Automation.Controllers
{
    public static class RecallController
    {
        public static bool IsRecalling { get { return ObjectManager.Player.IsRecalling(); } }
        public static bool ShouldRecall
        {
            get
            {
                bool returnValue = false;

                return returnValue;
            }
        }

        private static void CastRecall()
        {

        }
    }
}