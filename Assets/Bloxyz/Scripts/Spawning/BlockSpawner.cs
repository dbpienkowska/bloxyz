using UnityEngine;

namespace Bloxyz
{
    public class BlockSpawner : MonoBehaviour, IPlatformUser
    {
        public Platform platform { private get; set; }
        public BlockSelector selector;
        public BlockTransformer transformer;
        public BlockPainter painter;
        public GameObject cubePrefab;

        private IOnBlockSpawnedWatcher[] _spawnedWatchers;

        public Block Spawn()
        {
            Block block = Instantiate(selector.Select().gameObject).GetComponent<Block>();

            _PrepareSpawnedBlock(block);
            _EmitOnBlockSpawned(block);

            return block;
        }

        public Block Spawn(int index)
        {
            Block block = Instantiate(selector.blockPrefabs[index].gameObject).GetComponent<Block>();

            _PrepareSpawnedBlock(block);
            _EmitOnBlockSpawned(block);

            return block;
        }

        private void _PrepareSpawnedBlock(Block block)
        {
            block.LoadCubes(_CreateCubes(block.slots.Length));

            painter.Paint(block);
            transformer.Transform(block);

            block.transform.ChangeLocalPosition(y: transform.localPosition.y);
        }

        private Cube[] _CreateCubes(int amount)
        {
            Cube[] cubes = new Cube[amount];

            for(int i = 0; i < amount; i++)
                cubes[i] = Instantiate(cubePrefab).GetComponent<Cube>();

            return cubes;
        }

        private void _EmitOnBlockSpawned(Block block)
        {
            foreach(var watcher in _spawnedWatchers)
                watcher.OnBlockSpawned(block);
        }

        private void Awake()
        {
            _spawnedWatchers = RootProvider.root.GetComponentsInChildren<IOnBlockSpawnedWatcher>();
        }
    }
}

