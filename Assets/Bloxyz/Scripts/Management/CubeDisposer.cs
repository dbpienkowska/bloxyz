using UnityEngine;

namespace Bloxyz
{
    public class CubeDisposer
    {
        public static void Dispose(Cube cube)
        {
            Object.Destroy(cube.gameObject);
        }
    }
}

