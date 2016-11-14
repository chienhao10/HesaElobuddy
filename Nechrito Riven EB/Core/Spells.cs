namespace NechritoRiven.Core
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    #region

    #endregion

    internal class Spells : Core
    {
        #region Static Fields

        public static SpellSlot Flash;

        public static SpellSlot Ignite;

        #endregion

        #region Public Properties
        
        public static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 150, SkillShotType.Circular, 250);
        public static Spell.Active W
        {
            get { return new Spell.Active(SpellSlot.W, (uint)((Player.HasBuff("RivenFengShuiEngine") ? 200 : 120))); }
        }
        public static Spell.Active E = new Spell.Active(SpellSlot.E, 325);


        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 45, DamageType.Physical)
        {
            AllowedCollisionCount = int.MaxValue
        };


        #endregion

        #region Public Methods and Operators

        public static void Load()
        {
            //Q.SetSkillshot(0.25f, 100f, 2200f, false, SkillshotType.SkillshotCircle);
            //R.SetSkillshot(0.25f, (float)(45 * 0.5), 1600, false, SkillshotType.SkillshotCone);

            Ignite = Player.GetSpellSlotFromName("SummonerDot");
            Flash = Player.GetSpellSlotFromName("SummonerFlash");
        }

        #endregion
    }
}