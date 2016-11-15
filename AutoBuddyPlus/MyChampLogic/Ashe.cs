using System.Linq;
using AutoBuddy.MainLogics;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace AutoBuddy.MyChampLogic
{
    internal class Ashe : IChampLogic
    {
        public float MaxDistanceForAA { get { return int.MaxValue; } }
        public float OptimalMaxComboDistance { get { return AutoWalker.myHero.AttackRange; } }
        public float HarassDistance { get { return AutoWalker.myHero.AttackRange; } }

        public Spell.Active Q;
        public Spell.Skillshot W, E, R;

        public Ashe()
        {
            skillSequence = new[] {2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3};
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Cone);
            E = new Spell.Skillshot(SpellSlot.E, 2500, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 250, 1600, 130)
            {
                MinimumHitChance = HitChance.Medium,
                AllowedCollisionCount = 99
            };
            Game.OnTick += Game_OnTick;
        }

        public int[] skillSequence { get; private set; }
        public LogicSelector Logic { get; set; }
        
        public void Harass(AIHeroClient target)
        {
        }

        public void Survi()
        {
            if (R.IsReady() || W.IsReady())
            {
                AIHeroClient chaser =  EntityManager.Heroes.Enemies.FirstOrDefault(chase => chase.Distance(AutoWalker.myHero) < 600 && chase.IsVisible());
                if (chaser != null)
                {
                    if (R.IsReady() && AutoWalker.myHero.HealthPercent() > 18)
                        R.Cast(chaser);
                    if (W.IsReady())
                        W.Cast(chaser);
                }
            }
        }

        public void Combo(AIHeroClient target)
        {
            if (R.IsReady() && AutoWalker.myHero.Distance(target) > 600 &&
                AutoWalker.myHero.Distance(target) < 1600 && target.IsVisible())
                R.Cast(target);

            if (W.IsReady())
                W.Cast(target);

            if (Q.IsReady())
                Q.Cast();
        }

        private void Game_OnTick(System.EventArgs args)
        {
            if (!R.IsReady()) return;
            AIHeroClient vic = EntityManager.Heroes.Enemies.FirstOrDefault(v => v.IsVisible() && v.Health < AutoWalker.myHero.GetSpellDamage(v, SpellSlot.R) && v.Distance(AutoWalker.myHero) > 700 && AutoWalker.myHero.Distance(v) < 2500);
            if (vic == null) return;
            R.Cast(vic);
        }
    }
}