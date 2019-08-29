using UnityEngine;
using UnityEngine.Assertions;

namespace Bloxyz
{
    public class CubeSlot : MonoBehaviour
    {
        public Cube cube { get; private set; }

        public int x => Mathf.RoundToInt(transform.position.x);
        public int y => Mathf.RoundToInt(transform.position.y);
        public int z => Mathf.RoundToInt(transform.position.z);
        //public int localX => Mathf.RoundToInt(transform.localPosition.x);
        public int localY => Mathf.RoundToInt(transform.localPosition.y);
        //public int localZ => Mathf.RoundToInt(transform.localPosition.z);

        public void Load(Cube cube)
        {
            Assert.IsNull(this.cube);
            Assert.IsNotNull(cube);

            cube.transform.parent = transform;
            cube.transform.localPosition = Vector3.zero;
            this.cube = cube;
        }

        public Cube Unload()
        {
            Assert.IsNotNull(cube);

            cube.transform.parent = null;
            Cube unsetCube = cube;
            cube = null;

            return unsetCube;
        }
    }
}

