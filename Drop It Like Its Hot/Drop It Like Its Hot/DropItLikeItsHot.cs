using Drop_It_Like_Its_Hot.Properties;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Media;

namespace Drop_It_Like_Its_Hot
{
    public class DropItLikeItsHot
    {
        public static Menu Menu;

        public static bool Enabled => Menu["Enabled"].Cast<CheckBox>().CurrentValue;
        //public static bool MyWard => Menu["MyWard"].Cast<CheckBox>().CurrentValue;
        public static bool AllyWard => Menu["AllyWard"].Cast<CheckBox>().CurrentValue;
        public static bool EnemyWard => Menu["EnemyWard"].Cast<CheckBox>().CurrentValue;

        public DropItLikeItsHot()
        {
            InitializeMenu();
            GameObject.OnCreate += GameObject_OnCreate;
        }

        public void InitializeMenu()
        {
            Menu = MainMenu.AddMenu("Drop It Like Its Hot", "dropItLikeItsHot");
            Menu.Add("Enabled", new CheckBox("Enabled", true));
            //Menu.Add("MyWard", new CheckBox("My Wards", true));
            Menu.Add("AllyWard", new CheckBox("Ally Wards", true));
            Menu.Add("EnemyWard", new CheckBox("Enemy Wards", true));
        }

        public void PlayHotSound()
        {
            SoundPlayer audio = new SoundPlayer(Resources.DropItLikeItsHot);
            audio.Play();
        }

        public void PlayColdSound()
        {
            SoundPlayer audio = new SoundPlayer(Resources.ItsCold);
            audio.Play();
        }

        private void GameObject_OnCreate(GameObject sender, System.EventArgs args)
        {
            if (!Enabled) return;
            var ward = sender as Obj_AI_Minion;
            if (ward == null || !sender.Name.ToLower().Contains("ward"))
            {
                return;
            }
            if (ward.IsAlly)
            {
                PlayHotSound();
            }else if(EnemyWard)
            {
                PlayColdSound();
            }
        }

    }
}