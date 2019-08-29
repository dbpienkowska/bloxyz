namespace Bloxyz
{
    public class StartGameCommand : ICommand
    {
        private Supervisor _supervisor;

        public StartGameCommand(Supervisor supervisor)
        {
            _supervisor = supervisor;
        }

        public void Execute()
        {
            _supervisor.StartGame();
        }
    }
}

