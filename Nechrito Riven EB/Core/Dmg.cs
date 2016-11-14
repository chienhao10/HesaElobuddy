namespace NechritoRiven.Core
{
    #region

    using EloBuddy;
    using EloBuddy.SDK;

    #endregion

    internal class Dmg : Core
    {
        #region Public Methods and Operators

        public static float GetComboDamage(Obj_AI_Base enemy)
        {
            if (enemy == null) return 0;

            float damage = 0;

            var attackDmg = (float)ObjectManager.Player.GetAutoAttackDamage(enemy);

            if (Spells.E.IsReady())
            {
                damage += attackDmg;
            }

            if (Spells.W.IsReady())
            {
                damage += Spells.W.GetSpellDamage(enemy) + attackDmg;
            }

            if (Spells.Q.IsReady())
            {
                var qcount = 4 - Qstack;
                damage += Spells.Q.GetSpellDamage(enemy) * qcount + attackDmg * qcount;
            }

            if (Spells.R.IsReady())
            {
                damage += Spells.R.GetSpellDamage(enemy) + attackDmg;
            }

            /*if (!Player.IsWindingUp)
            {
                damage += attackDmg;
            }*/

            return damage;
        }

        public static float IgniteDamage(AIHeroClient target)
        {
            if (Spells.Ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(Spells.Ignite) != SpellState.Ready)
            {
                return 0f;
            }
            
            return (float)Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);
        }

        public static float RDmg(AIHeroClient target)
        {
            if (target == null || !Spells.R.IsReady())
            {
                return 0;
            }
            return Spells.R.GetSpellDamage(target);
        }

        #endregion
    }
}