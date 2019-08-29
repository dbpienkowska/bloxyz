using UnityEngine;

namespace Bloxyz
{
    public class PlatformProvider
    {
        public static void Provide(Platform platform, Transform root)
        {
            IPlatformUser[] users = root.GetComponentsInChildren<IPlatformUser>();

            foreach(var user in users)
                user.platform = platform;
        }
    }
}

