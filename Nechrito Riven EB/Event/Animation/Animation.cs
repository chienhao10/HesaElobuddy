namespace NechritoRiven.Event.Animation
{
    #region

    using System;
    using System.Linq;
    //using Orbwalking = Orbwalking;
    using EloBuddy;
    using EloBuddy.SDK;

    #endregion

    internal class Animation : NechritoRiven.Core.Core
    {
        #region Public Methods and Operators

        private static AIHeroClient Target => TargetSelector.GetTarget(ObjectManager.Player.AttackRange + 50, DamageType.Physical);
        private static Obj_AI_Minion Mob => EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => Player.IsInRange(x, Player.AttackRange + 50)).FirstOrDefault();
        static string lastAnimation = "";
        public static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (ObjectManager.Player.ChampionName == "Riven")
            {
                if (lastAnimation != args.Animation)
                {
                    lastAnimation = args.Animation;
                    Chat.Print("Current animation = " + args.Animation);
                }
                switch (args.Animation)
                {
                    case "Spell1a":
                    {
                        LastQ = Environment.TickCount;
                        Qstack = 2;

                        //if (SafeReset())
                        {
                            Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));

                            Console.WriteLine("Q1 Delay: " + ResetDelay(MenuConfig.Qd));
                        }
                    }
                    break;
                    case "Spell1b":
                    {
                        LastQ = Environment.TickCount;
                        Qstack = 3;

                        //if (SafeReset())
                        {
                            Core.DelayAction(Reset, ResetDelay(MenuConfig.Q2D));

                            Console.WriteLine("Q2 Delay: " + ResetDelay(MenuConfig.Q2D));
                        }
                    }
                    break;
                    case "Spell1c":
                    {
                        LastQ = Environment.TickCount;
                        Qstack = 1;

                        //if (SafeReset())
                        {
                            Core.DelayAction(Reset, ResetDelay(MenuConfig.Qld));

                            Console.WriteLine("Q3 Delay: " + ResetDelay(MenuConfig.Qld) + Environment.NewLine + ">----END----<");
                        }
                    }
                    break;
                    case "Spell2":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                    case "Spell3":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                    case "Spell4a":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                    case "Spell4b":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                }
            }
            else
            {
                switch (args.Animation)
                {
                    case "Spell1":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                    case "Spell2":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                    case "Spell3":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                    case "Spell4":
                    {
                        Core.DelayAction(Reset, ResetDelay(MenuConfig.Qd));
                    }
                    break;
                }
            }
        }

        #endregion

        #region Methods

        private static void Emotes()
        {
            switch (MenuConfig.EmoteList)
            {
                case "Laugh":
                    EloBuddy.Player.DoEmote(Emote.Laugh);
                    //Chat.Print("/l");
                    //Game.Say("/l");
                    break;
                case "Taunt":
                    EloBuddy.Player.DoEmote(Emote.Taunt);
                    //Chat.Print("/t");
                    //Game.Say("/t");
                    break;
                case "Joke":
                    EloBuddy.Player.DoEmote(Emote.Joke);
                    //Chat.Print("/j");
                    //Game.Say("/j");
                    break;
                case "Dance":
                    EloBuddy.Player.DoEmote(Emote.Dance);
                    //Chat.Print("/d");
                    //Game.Say("/d");
                    break;
            }
        }

        private static int ResetDelay(int qDelay)
        {
            if (MenuConfig.CancelPing)
            {
                return qDelay + Game.Ping / 2;
            }

            if ((Target != null && Target.IsMoving) || (Mob != null && Mob.IsMoving) || IsGameObject)
            {
                return (int)(qDelay * 1.15);
            }

            return qDelay;
        }
        
        private static void Reset()
        {
            Emotes();
            //Orbwalker.ResetAutoAttack();
            //Orbwalking.ResetAutoAttackTimer();
            Orbwalker.MoveTo(Game.CursorPos);
        }

        private static bool SafeReset()
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None))
            //if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Flee || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
            {
                return false;
            }

            return !ObjectManager.Player.HasBuffOfType(BuffType.Stun)
                && !ObjectManager.Player.HasBuffOfType(BuffType.Snare) 
                && !ObjectManager.Player.HasBuffOfType(BuffType.Knockback)
                && !ObjectManager.Player.HasBuffOfType(BuffType.Knockup);
        }

        #endregion
    }
}