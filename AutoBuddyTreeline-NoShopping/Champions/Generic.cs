using AutoBuddy.MainLogics;
using EloBuddy;
using EloBuddy.SDK;

namespace AutoBuddy.Champions
{
    internal class Generic : IChampLogic
    {

        public float MaxDistanceForAA { get { return int.MaxValue; } }
        public float OptimalMaxComboDistance { get { return AutoWalker.myHero.AttackRange; } }
        public float HarassDistance { get { return AutoWalker.myHero.AttackRange; } }

        public Spell.Skillshot Q;
        public Spell.Skillshot W, E, R;

        public Generic()
        {
            skillSequence = new[] {2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3};
            ShopSequence =
                "2003:StartHpPot,1055:Buy,3086:Buy,2015:Buy,3087:Buy,1001:Buy,3122:Buy,1037:Buy,3104:Buy,3006:Buy,1043:Buy,2003:StopHpPot,3144:Buy,3153:Buy,1037:Buy,1055:Sell,1053:Buy,3181:Buy,3035:Buy,3033:Buy";
        }

        public int[] skillSequence { get; private set; }
        public LogicSelector Logic { get; set; }
        public string ShopSequence { get; private set; }


        public void Harass(AIHeroClient target)
        {
        }

        public void Survi()
        {
        }

        public void Combo(AIHeroClient target)
        {
        }
    }
}