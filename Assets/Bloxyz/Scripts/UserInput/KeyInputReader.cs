using UnityEngine;
using UnityEngine.Assertions;

namespace Bloxyz
{
    public class KeyInputReader : MonoBehaviour, IInputReader
    {
        public KeyCode startGameKey = KeyCode.Return;
        public KeyCode pauseResumeKey = KeyCode.Space;
        public KeyCode rotateLeftKey = KeyCode.LeftArrow;
        public KeyCode rotateRightKey = KeyCode.RightArrow;
        public KeyCode quickLandKey = KeyCode.DownArrow;

        public ICommand startGameCommand { get; set; }
        public ICommand pauseResumeCommand { get; set; }
        public ICommand rotateLeftCommand { get; set; }
        public ICommand rotateRightCommand { get; set; }
        public ICommand quickLandCommand { get; set; }

        private KeyCode[] _keys;
        private ICommand[] _commands;

        private void Start()
        {
            Assert.IsNotNull(startGameCommand);
            Assert.IsNotNull(pauseResumeCommand);
            Assert.IsNotNull(rotateLeftCommand);
            Assert.IsNotNull(rotateRightCommand);
            Assert.IsNotNull(quickLandCommand);

            _keys = new KeyCode[] { startGameKey, pauseResumeKey, rotateLeftKey, rotateRightKey, quickLandKey };
            _commands = new ICommand[] { startGameCommand, pauseResumeCommand, rotateLeftCommand, rotateRightCommand, quickLandCommand };
        }

        private void Update()
        {
            for(int i = 0; i < _keys.Length; i++)
                if(Input.GetKeyDown(_keys[i]) && _commands[i] != null)
                    _commands[i].Execute();
        }
    }
}

