using UnityEngine.Assertions;

namespace Bloxyz
{
    public class SpawnCommand : ICommand
    {
        private BlockSpawner _spawner;

        public SpawnCommand(BlockSpawner spawner)
        {
            Assert.IsNotNull(spawner);

            _spawner = spawner;
        }

        public void Execute()
        {
            if(_spawner.enabled)
                _spawner.Spawn();
        }
    }
}

