namespace Bloxyz
{
    public class PauseResumeCommand : ICommand
    {
        private Supervisor _supervisor;

        public PauseResumeCommand(Supervisor supervisor)
        {
            _supervisor = supervisor;
        }

        public void Execute()
        {
            if(_supervisor.started && !_supervisor.over)
            {
                if(_supervisor.paused)
                    _supervisor.ResumeGame();
                else
                    _supervisor.PauseGame();
            }
        }
    }
}

