using UnityEngine;
using UnityEngine.Assertions;

namespace Bloxyz
{
    public class Block : MonoBehaviour
    {
        public const int MAX_HEIGHT = 3;

        public int platformCompatibility;
        public CubeSlot[] slots;
        public CubeSlot[] topSlots;
        public CubeSlot[] bottomSlots;

        public void LoadCubes(Cube[] cubes)
        {
            Assert.IsNotNull(cubes);
            Assert.IsTrue(cubes.Length == slots.Length);

            for(int i = 0; i < slots.Length; i++)
                slots[i].Load(cubes[i]);
        }

        public Cube[] UnloadCubes()
        {
            Cube[] cubes = new Cube[slots.Length];

            for(int i = 0; i < cubes.Length; i++)
                cubes[i] = slots[i].Unload();

            return cubes;
        }
    }
}

