using System.Linq;
using AutoBuddy.MainLogics;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace AutoBuddy.MyChampLogic
{
    internal class Vayne : IChampLogic
    {
        public float MaxDistanceForAA { get { return int.MaxValue; } }
        public float OptimalMaxComboDistance { get { return AutoWalker.myHero.AttackRange; } }
        public float HarassDistance { get { return AutoWalker.myHero.AttackRange; } }


        public Spell.Active Q;
        public Spell.Skillshot W, E, R;

        public Vayne()
        {
            skillSequence = new[] { 1, 3, 2, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
            
            Game.OnTick += Game_OnTick;
        }

        public int[] skillSequence { get; private set; }
        public LogicSelector Logic { get; set; }
        
        public void Harass(AIHeroClient target)
        {
        }

        public void Survi()
        {

        }

        public void Combo(AIHeroClient target)
        {

        }

        private void Game_OnTick(System.EventArgs args)
        {

        }
    }
}