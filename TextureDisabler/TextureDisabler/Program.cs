using EloBuddy;

namespace TextureDisabler
{
    class Program
    {
        static void Main(string[] args)
        {
            Hacks.DisableTextures = true;
            ManagedTexture.OnLoad += delegate (OnLoadTextureEventArgs texture) { texture.Process = false; };
        }
    }
}