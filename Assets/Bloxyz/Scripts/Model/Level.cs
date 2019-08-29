using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Bloxyz
{
    public class Level : MonoBehaviour, IComparable<Level>
    {
        public int size { get; private set; } // sqrt(capacity)
        public int capacity { get; private set; } // size^2
        public Cube[] cubes { get; private set; }
        public int cubeCount { get; private set; }

        public int localY => Mathf.RoundToInt(transform.localPosition.y);

        private int _rotation => Mathf.RoundToInt(transform.eulerAngles.y);

        public void Init(int capacity, int y)
        {
            this.capacity = capacity;
            cubes = new Cube[capacity];
            cubeCount = 0;

            size = (int)Mathf.Sqrt(capacity);

            transform.ChangeLocalPosition(y: y);
        }
        
        public Cube GetCube(int x, int z) => cubes[_Index(x, z)];

        public void SetCube(Cube cube)
        {
            int i = _Index(cube.x, cube.z);

            cube.transform.parent = transform;
            cube.transform.localEulerAngles = Vector3.zero;
            cubes[i] = cube;
            cubeCount++;
        }

        public Cube[] PullCubes()
        {
            Cube[] pulledCubes = new Cube[capacity];
            Array.Copy(cubes, pulledCubes, capacity);

            Array.Clear(cubes, 0, capacity);
            cubeCount = 0;

            return pulledCubes;
        }

        private int _Index(int x, int z) // zamiast za każdym razem obliczać wartość indeksu, można by było zapisać na starcie mapę i tylko ją odczytywać
        {
            Assert.IsFalse(x < 0);
            Assert.IsFalse(z < 0);

            int mappedX = Mapper.MapX(x, z, _rotation);
            int mappedZ = Mapper.MapZ(x, z, _rotation);

            int i = size * mappedZ + mappedX;

            Assert.IsTrue(i < capacity);

            return i;
        }

        public int CompareTo(Level other)
        {
            return localY.CompareTo(other.localY);
        }
    }
}

