namespace NechritoRiven
{
    #region

    using System;

    using Core;

    using Draw;

    using Event.OrbwalkingModes;

    using NechritoRiven.Event.Animation;
    using NechritoRiven.Event.Interrupters_Etc;
    using NechritoRiven.Event.Misc;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    #endregion

    internal class Load
    {
        #region Public Methods and Operators

        public static void LoadAssembly()
        {
            MenuConfig.LoadMenu();
            Spells.Load();

            Obj_AI_Base.OnSpellCast += AfterAuto.OnDoCast;
            Obj_AI_Base.OnProcessSpellCast += ProcessSpell.OnProcessSpell;
            Obj_AI_Base.OnProcessSpellCast += BackgroundData.OnCast;
            Obj_AI_Base.OnPlayAnimation += Animation.OnPlay;

            Drawing.OnEndScene += DrawDmg.DmgDraw;
            Drawing.OnDraw += DrawMisc.RangeDraw;
            Drawing.OnDraw += DrawWallSpot.WallDraw;

            Game.OnUpdate += KillSteal.Update;
            Game.OnUpdate += PermaActive.Update;
            Game.OnUpdate += Skinchanger.Update;
            Orbwalker.OnPostAttack += ProcessSpell.Orbwalker_OnPostAttack;

            Interrupter.OnInterruptableSpell += Interrupt2.OnInterruptableTarget;
            Gapcloser.OnGapcloser += Gapclose.Gapcloser;

            Chat.Print("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Riven ( Elobuddy )</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Loaded!</font></b>");
            Console.WriteLine("Nechrito Riven ( Elobuddy ): Loaded");
        }

        #endregion
    }
}