namespace NechritoRiven.Event.Interrupters_Etc
{
    using Core;
    #region

    using EloBuddy;

    #endregion

    internal class ProcessSpell : Core
    {
        #region Public Methods and Operators

        public static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy)// || !sender.IsValid(1000))//TODO: Fix this...
            {
                return;
            }

            if (Spells.E.IsReady())
            {
                if (BackgroundData.AntigapclosingSpells.Contains(args.SData.Name) || (BackgroundData.TargetedSpells.Contains(args.SData.Name) && args.Target.IsMe))
                {
                    EloBuddy.SDK.Core.DelayAction(() => Spells.E.Cast(Game.CursorPos), 120);
                }
            }

            if (!BackgroundData.InterrupterSpell.Contains(args.SData.Name) || !Spells.W.IsReady() || !BackgroundData.InRange(sender))
            {
                return;
            }

            BackgroundData.CastW(sender);
        }

        #endregion
    }
}