using UnityEngine;
using UnityEngine.Assertions;

namespace Bloxyz
{
    public class Initializer : MonoBehaviour
    {
        public Transform root;

        public Platform platform;
        public int platformSize = 2;
        public int levelLimit = 8;
        public Transform cameraTransform;
        public Transform floorTransform;
        public Transform wallTransform;

        public Supervisor supervisor;
        public IInputReader inputReader;
        public ColorPalette colorPalette;

        private void Awake()
        {
            Assert.IsNotNull(platform);
            Assert.IsNotNull(cameraTransform);
            Assert.IsNotNull(root);
            Assert.IsNotNull(supervisor);
            Assert.IsNotNull(colorPalette);

            _InitRootProvider();
            _InitPlatform();
            _InitCamera();
            _InitInputReader();
            _InitColors();
            _InitWallAndFloor();

            Mapper.platformSize = platformSize;
        }

        private void _InitPlatform()
        {
            platform.Init(platformSize, levelLimit);
            PlatformProvider.Provide(platform, root);
        }

        private void _InitInputReader()
        {
            inputReader = root.GetComponentInChildren<IInputReader>();
            inputReader.startGameCommand = new StartGameCommand(supervisor);
            inputReader.pauseResumeCommand = new PauseResumeCommand(supervisor);
            inputReader.rotateLeftCommand = new RotateCommand(supervisor.rotater, -90);
            inputReader.rotateRightCommand = new RotateCommand(supervisor.rotater, 90);
            inputReader.quickLandCommand = new QuickLandCommand(supervisor.fallController);
        }

        private void _InitRootProvider()
        {
            RootProvider.root = root;
        }

        private void _InitColors()
        {
            Assert.IsNotNull(supervisor.spawner);
            Assert.IsNotNull(supervisor.spawner.painter);

            supervisor.spawner.painter.palette = colorPalette;
        }

        private void _InitCamera()
        {
            Vector3 position = cameraTransform.localPosition;

            float factor = (platformSize - 2) / 2;
            position.x -= factor;
            position.z -= factor;
            position.y += factor;

            cameraTransform.localPosition = position;
        }

        private void _InitWallAndFloor()
        {
            float factor = (platformSize - 2);

            Vector3 position = wallTransform.position;
            position.x += factor;
            position.z += factor;
            wallTransform.position = position;

            position = floorTransform.position;
            position.y = platform.baseTransform.localPosition.y - platform.baseTransform.localScale.y / 2;
            floorTransform.position = position;
        }
    }
}

