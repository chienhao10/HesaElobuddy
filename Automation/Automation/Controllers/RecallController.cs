using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace Automation.Controllers
{
    public static class RecallController
    {
        private static Obj_SpawnPoint spawn;

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
            Core.DelayAction(() =>
            {
                if (IsRecalling || ObjectManager.Player.Distance(spawn) < 500) return;
                //lastRecallTime = Game.Time + 2f;
                //AutoWalker.SetMode(Orbwalker.ActiveModes.None);
                //if (!AutoWalker.Recalling())
                {
                    //if (AutoWalker.B.IsReady())
                    {
                    //    AutoWalker.B.Cast();
                    }
                }
            }, 300);
        }

        public static void Tick()
        {
            if(spawn == null)
            {
                foreach (Obj_SpawnPoint so in ObjectManager.Get<Obj_SpawnPoint>().Where(so => so.Team == ObjectManager.Player.Team))
                {
                    spawn = so;
                }
            }
        }
    }
}