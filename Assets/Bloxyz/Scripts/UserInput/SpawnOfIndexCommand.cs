using UnityEngine.Assertions;

namespace Bloxyz
{
    public class SpawnOfIndexCommand : ICommand
    {
        private BlockSpawner _spawner;
        private int _index;

        public SpawnOfIndexCommand(BlockSpawner spawner, int blockIndex)
        {
            _spawner = spawner;
            _index = blockIndex;
        }

        public void Execute()
        {
            Assert.IsTrue(_index < _spawner.selector.blockPrefabs.Length);

            if(_spawner.enabled)
                _spawner.Spawn(_index);
        }
    }
}

