namespace Bloxyz
{
    public class RotateCommand : ICommand
    {
        private PlatformRotater _rotater;
        private int _angle;

        public RotateCommand(PlatformRotater rotater, int angle)
        {
            _rotater = rotater;
            _angle = angle;
        }

        public void Execute()
        {
            if(_rotater.enabled)
                _rotater.Rotate(_angle);
        }
    }
}

