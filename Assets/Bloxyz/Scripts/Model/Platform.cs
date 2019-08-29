using UnityEngine;

namespace Bloxyz
{
    public class Platform : MonoBehaviour
    {
        public Transform baseTransform;
        public Transform overPointTransform;
        public GameObject levelPrefab;
        public GameObject levelClearParticlesPrefab;
        public int size { get; private set; }
        public int limit { get; private set; }
        public Level[] levels { get; private set; }
        public ParticleSystem[] levelClearParticles { get; private set; }
        public int groundMax;
        public int rotation => Mathf.RoundToInt(transform.eulerAngles.y);

        private Material _material;
        private int[][] _grounds;

        public void Init(int size, int limit)
        {
            _SetSize(size);
            _SetLimit(limit);
            _InitLevels(levelPrefab);
            _InitParticles(levelClearParticlesPrefab);
            _InitGrounds();
        }

        public int GetGround(int globalX, int globalZ)
        {
            int x = Mapper.MapX(globalX, globalZ, rotation);
            int z = Mapper.MapZ(globalX, globalZ, rotation);

            return _grounds[x][z];
        }

        public void UpdateGround(Cube cube)
        {
            int x = Mapper.MapX(cube.x, cube.z, rotation);
            int z = Mapper.MapZ(cube.x, cube.z, rotation);

            int cubeTopY = cube.y + 1;

            if(cubeTopY > _grounds[x][z])
            {
                _grounds[x][z] = cubeTopY;

                if(cubeTopY > groundMax)
                    groundMax = cubeTopY;
            }
        }
        
        public void LowerGrounds(int clearedAmount)
        {
            int temp = groundMax + 1;
            //groundMax = 0;
            for(int x = 0; x < size; x++)
            {
                for(int z = 0; z < size; z++)
                {
                    for(int y = temp; y > 0; y--)
                    {
                        if(levels[y - 1].cubes[size * z + x])
                        {
                            _grounds[x][z] = y;

                            //if(y > groundMax)
                            //    groundMax = y;

                            break;
                        }

                        _grounds[x][z] = 0;
                    }
                }
            }
        }

        //public void SetGround(int globalX, int globalZ, int value)
        //{
        //    int x = _MapX(globalX, globalZ);
        //    int z = _MapZ(globalX, globalZ);

        //    _grounds[x][z] = value;
        //}

        private void _SetSize(int size)
        {
            this.size = size;

            float baseScaleY = (float)size / 10;
            Vector3 scale = new Vector3(size, baseScaleY, size);
            baseTransform.localScale = scale;

            float baseY = -0.5f - baseScaleY / 2;
            baseTransform.ChangeLocalPosition(y: baseY);

            float platformXZ = 0.5f * (size - 1);
            transform.ChangeLocalPosition(x: platformXZ, z: platformXZ);
            
            _material = GetComponentInChildren<MeshRenderer>().material;
            _material.mainTextureScale = new Vector2(size, size);
        }

        private void _SetLimit(int limit)
        {
            this.limit = limit;
            overPointTransform.ChangeLocalPosition(y: limit);
        }

        private void _InitLevels(GameObject levelPrefab)
        {
            levels = new Level[limit];
            int levelCapacity = size * size;

            for(int i = 0; i < limit; i++)
            {
                Level level = Instantiate(levelPrefab, transform).GetComponent<Level>();
                level.transform.localPosition = -transform.position;
                level.Init(levelCapacity, i);
                levels[i] = level;
            }
        }

        private void _InitParticles(GameObject particlePrefab)
        {
            levelClearParticles = new ParticleSystem[limit];

            for(int i = 0; i < limit; i++)
            {
                ParticleSystem particle = Instantiate(particlePrefab, transform).GetComponent<ParticleSystem>();
                particle.transform.ChangeLocalPosition(y: i);
                levelClearParticles[i] = particle;
            }
        }

        private void _InitGrounds()
        {
            _grounds = new int[size][];

            for(int i = 0; i < size; i++)
                _grounds[i] = new int[size];

            groundMax = 0;
        }
    }
}

