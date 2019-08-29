namespace Bloxyz
{
    public class QuickLandCommand : ICommand
    {
        private FallController _fallController;

        public QuickLandCommand(FallController fallController)
        {
            _fallController = fallController;
        }

        public void Execute()
        {
            if(_fallController.enabled)
                _fallController.QuickLand();
        }
    }
}

