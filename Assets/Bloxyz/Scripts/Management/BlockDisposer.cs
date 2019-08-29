using UnityEngine;

namespace Bloxyz
{
    public class BlockDisposer
    {
        public static void Dispose(Block block)
        {
            Object.Destroy(block.gameObject);
        }
}
}

