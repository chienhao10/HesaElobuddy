using AutoBuddy.MainLogics;
using EloBuddy;
using EloBuddy.SDK;

namespace AutoBuddy.Champions
{
    internal class Annie : IChampLogic
    {

        public float MaxDistanceForAA { get { return int.MaxValue; } }
        public float OptimalMaxComboDistance { get { return AutoWalker.myHero.AttackRange; } }
        public float HarassDistance { get { return AutoWalker.myHero.AttackRange; } }

        public Spell.Active Q;
        public Spell.Skillshot W, E, R;

        public Annie()
        {
            skillSequence = new[] {
                1,//level 1
                2,//level 2
                1,//level 3
                3,//level 4
                2,//level 5
                4,//level 6
                1,//level 7
                2,//level 8
                1,//level 9
                2,//level 10
                4,//level 11
                1,//level 12
                2,//level 13
                3,//level 14
                3,//level 15
                4,//level 16
                3,//level 17
                3//level 18
            };
            ShopSequence = "2003:StartHpPot,1056:Buy,3010:Buy,1026:Buy,3027:Buy,1058:Buy,3020:Buy,3191:Buy,3157:Buy,1058:Buy,2003:StopHpPot,1011:Buy,3116:Buy,1056:Sell,1058:Buy,1026:Buy,3089:Buy,3136:Buy,3151:Buy";
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