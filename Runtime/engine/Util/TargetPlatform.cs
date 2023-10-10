using UnityEngine;

namespace CofyEngine.Util
{
    public static class TargetPlatform
    {
        public static bool InEditor(this RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                    return true;
            }

            return false;
        }
    }
}