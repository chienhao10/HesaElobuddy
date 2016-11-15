namespace NechritoRiven
{
    #region

    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    #endregion

    internal class MenuConfig : Core.Core
    {
        #region Constants

        private const string MenuName = "Nechrito Riven ( Elobuddy )";

        #endregion

        #region Static Fields

        private static Menu config, orbwalker, animation, combo, lane, jngl, killsteal, misc, draw, flee, skin;

        #endregion

        #region Public Properties
        
        #region Animation
        public static bool CancelPing => animation["CancelPing"].Cast<CheckBox>().CurrentValue;
        public static string EmoteList
        {
            get
            {
                switch (animation["EmoteList"].Cast<ComboBox>().CurrentValue)
                {
                    case 0:
                    {
                        return "Laugh";
                    }
                    break;

                    case 1:
                    {
                        return "Taunt";
                    }
                    break;

                    case 2:
                    {
                        return "Joke";
                    }
                    break;

                    default:
                    case 3:
                    {
                        return "Dance";
                    }
                    break;

                    case 4:
                    {
                        return "None";
                    }
                    break;
                }
            }
        }
        public static bool AnimDance => EmoteList == "Dance";
        public static bool AnimLaugh => EmoteList == "Laugh";
        public static bool AnimTalk => EmoteList == "Joke";
        public static bool AnimTaunt => EmoteList == "Taunt";
        #endregion
        
        #region Combo
        public static bool Q3Wall => combo["Q3Wall"].Cast<CheckBox>().CurrentValue;
        public static bool Flash => combo["FlashOften"].Cast<CheckBox>().CurrentValue;
        public static bool OverKillCheck => combo["OverKillCheck"].Cast<CheckBox>().CurrentValue;
        public static bool Doublecast => combo["Doublecast"].Cast<CheckBox>().CurrentValue;
        public static bool AlwaysF => combo["AlwaysF"].Cast<KeyBind>().CurrentValue;
        public static bool AlwaysR => combo["AlwaysR"].Cast<KeyBind>().CurrentValue;
        public static bool BurstEnabled => combo["BurstEnabled"].Cast<KeyBind>().CurrentValue;
        public static bool FastHarass => combo["FastHarass"].Cast<KeyBind>().CurrentValue;
        #endregion

        #region Lane
        public static bool LaneEnemy => lane["LaneEnemy"].Cast<CheckBox>().CurrentValue;
        public static bool LaneQFast => lane["laneQFast"].Cast<CheckBox>().CurrentValue;
        public static bool LaneQ => lane["LaneQ"].Cast<CheckBox>().CurrentValue;
        public static bool LaneW => lane["LaneW"].Cast<CheckBox>().CurrentValue;
        public static bool LaneE => lane["LaneE"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region Jungle
        public static bool JnglQ => jngl["JungleQ"].Cast<CheckBox>().CurrentValue;
        public static bool JnglW => jngl["JungleW"].Cast<CheckBox>().CurrentValue;
        public static bool JnglE => jngl["JungleE"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region Killsteal
        public static bool Ignite => killsteal["ignite"].Cast<CheckBox>().CurrentValue;
        public static bool KsW => killsteal["ksW"].Cast<CheckBox>().CurrentValue;
        public static bool KsR2 => killsteal["ksR2"].Cast<CheckBox>().CurrentValue;
        public static bool KsQ => killsteal["ksQ"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region Misc
        public static bool GapcloserMenu => misc["GapcloserMenu"].Cast<CheckBox>().CurrentValue;
        public static bool InterruptMenu => misc["InterruptMenu"].Cast<CheckBox>().CurrentValue;
        public static bool KeepQ => misc["KeepQ"].Cast<CheckBox>().CurrentValue;
        public static bool QMove => misc["QMove"].Cast<KeyBind>().CurrentValue;
        #endregion

        #region Draw
        public static bool FleeSpot => draw["FleeSpot"].Cast<CheckBox>().CurrentValue;
        public static bool Dind => draw["Dind"].Cast<CheckBox>().CurrentValue;
        public static bool ForceFlash => draw["DrawForceFlash"].Cast<CheckBox>().CurrentValue;
        public static bool DrawAlwaysR => draw["DrawAlwaysR"].Cast<CheckBox>().CurrentValue;
        public static bool DrawBurst => draw["DrawBurst"].Cast<CheckBox>().CurrentValue;
        public static bool DrawFastHarassStatus => draw["DrawFastHarassStatus"].Cast<CheckBox>().CurrentValue;
        public static bool DrawCb => draw["DrawCB"].Cast<CheckBox>().CurrentValue;
        public static bool DrawBt => draw["DrawBT"].Cast<CheckBox>().CurrentValue;
        public static bool DrawFh => draw["DrawFH"].Cast<CheckBox>().CurrentValue;
        public static bool DrawHs => draw["DrawHS"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region Flee
        public static bool WallFlee => flee["WallFlee"].Cast<CheckBox>().CurrentValue;
        public static bool FleeYomuu => flee["FleeYoumuu"].Cast<CheckBox>().CurrentValue;
        #endregion

        #region SkinChanger
        public static bool UseSkin => skin["UseSkin"].Cast<CheckBox>().CurrentValue;
        public static string SkinList
        {
            get
            {
                switch (skin["SkinList"].Cast<ComboBox>().CurrentValue)
                {
                    default:
                    case 0:
                    {
                        return "Redeemed";
                    }
                    break;

                    case 1:
                    {
                        return "Crimson Elite";
                    }
                    break;

                    case 2:
                    {
                        return "Battle Bunny";
                    }
                    break;
                    case 3:
                    {
                        return "Championship";
                    }
                    break;
                    case 4:
                    {
                        return "Dragonblade";
                    }
                    break;
                    case 5:
                    {
                        return "Arcade";
                    }
                    break;
                    case 6:
                    {
                        return "Championship 2016";
                    }
                    break;
                    case 7:
                    {
                        return "Chroma 1";
                    }
                    break;
                    case 8:
                    {
                        return "Chroma 2";
                    }
                    break;
                    case 9:
                    {
                        return "Chroma 3";
                    }
                    break;
                    case 10:
                    {
                        return "Chroma 4";
                    }
                    break;
                    case 11:
                    {
                        return "Chroma 5";
                    }
                    break;
                    case 12:
                    {
                        return "Chroma 6";
                    }
                    break;
                    case 13:
                    {
                        return "Chroma 7";
                    }
                    break;
                    case 14:
                    {
                        return "Chroma 8";
                    }
                    break;
                }
            }
        }
        public static int SelectedSkinId => skin["SkinList"].Cast<ComboBox>().CurrentValue;
        #endregion
        
        #endregion

        #region Public Methods and Operators

        public static void LoadMenu()
        {
            config = MainMenu.AddMenu(MenuName, MenuName);

            animation = config.AddSubMenu("Animation", "riv_anim");
            animation.Add("CancelPing", new CheckBox("Include Ping", true));
            animation.Add("EmoteList", new ComboBox("Emotes", new[] { "Laugh", "Taunt", "Joke", "Dance", "None" }, 3));
            

            combo = config.AddSubMenu("Combo", "riv_combo");
            combo.Add("Q3Wall", new CheckBox("Q3 Over Wall", true));
            combo.Add("FlashOften", new CheckBox("Flash Burst Frequently", true));
            combo.Add("OverKillCheck", new CheckBox("R2 Max Damage", true));
            combo.Add("Doublecast", new CheckBox("Fast Combo, less dmg", true));
            combo.Add("AlwaysR", new KeyBind("Use R (Toggle)", true, KeyBind.BindTypes.PressToggle, 'G'));
            combo.Add("AlwaysF", new KeyBind("Use Flash (Toggle)", true, KeyBind.BindTypes.PressToggle, 'L'));
            combo.Add("BurstEnabled", new KeyBind("Enable Burst Combo (Toggle)", false, KeyBind.BindTypes.PressToggle, 'H'));
            combo.Add("FastHarass", new KeyBind("Fast Harass (Toggle)", false, KeyBind.BindTypes.PressToggle, 'J'));


            lane = config.AddSubMenu("Lane", "riv_lane");
            lane.Add("LaneEnemy", new CheckBox("Stop If Nearby Enemy", true));
            lane.Add("laneQFast", new CheckBox("Fast Clear", true));
            lane.Add("LaneQ", new CheckBox("Use Q", true));
            lane.Add("LaneW", new CheckBox("Use W", true));
            lane.Add("LaneE", new CheckBox("Use E", true));


            jngl = config.AddSubMenu("Jungle", "riv_jngl");
            jngl.Add("JungleQ", new CheckBox("Use Q", true));
            jngl.Add("JungleW", new CheckBox("Use W", true));
            jngl.Add("JungleE", new CheckBox("Use E", true));


            killsteal = config.AddSubMenu("Killsteal", "riv_ks");
            killsteal.Add("ignite", new CheckBox("Use Ignite", true));
            killsteal.Add("ksW", new CheckBox("Use W", true));
            killsteal.Add("ksR2", new CheckBox("Use R2", true));
            killsteal.Add("ksQ", new CheckBox("Use Q", true));


            misc = config.AddSubMenu("Misc", "riv_misc");
            misc.Add("GapcloserMenu", new CheckBox("Anti-Gapcloser", true));
            misc.Add("InterruptMenu", new CheckBox("Interrupter", true));
            misc.Add("KeepQ", new CheckBox("Keep Q Alive", true));
            misc.Add("QMove", new KeyBind("Q Move to mouse", false, KeyBind.BindTypes.HoldActive, 'K'));


            draw = config.AddSubMenu("Draw", "riv_draw");
            draw.Add("FleeSpot", new CheckBox("Draw Flee Spots", true));
            draw.Add("Dind", new CheckBox("Damage Indicator", true));
            draw.Add("DrawForceFlash", new CheckBox("Flash Status", true));
            draw.Add("DrawAlwaysR", new CheckBox("R Status", true));
            draw.Add("DrawBurst", new CheckBox("Burst Status", true));
            draw.Add("DrawFastHarassStatus", new CheckBox("Fast Harass Status", true));
            draw.Add("DrawCB", new CheckBox("Combo Engage", true));
            draw.Add("DrawBT", new CheckBox("BurstMode Engage", false));
            draw.Add("DrawFH", new CheckBox("FastHarassMode Engage", false));
            draw.Add("DrawHS", new CheckBox("Harass Engage", false));


            flee = config.AddSubMenu("Flee", "riv_flee");
            flee.Add("WallFlee", new CheckBox("WallJump in Flee", true));
            flee.Add("FleeYoumuu", new CheckBox("Use Youmuu's Ghostblade", true));


            skin = config.AddSubMenu("SkinChanger", "riv_skin");
            skin.Add("UseSkin", new CheckBox("Use SkinChanger", false));
            skin.Add("SkinList", new ComboBox("Skin", new[] { "Default",
                "Redeemed",
                "Crimson Elite",
                "Battle Bunny",
                "Championship",
                "Dragonblade",
                "Arcade",
                "Championship 2016",
                "Chroma 1",
                "Chroma 2",
                "Chroma 3",
                "Chroma 4",
                "Chroma 5",
                "Chroma 6",
                "Chroma 7",
                "Chroma 8"
            }, 0));

            var about = config.AddSubMenu("About", "riv_about");
            about.AddLabel("Version: 6.2.2");
            about.AddLabel("Paypal: nechrito@live.se");
        }
        #endregion
    }
}