namespace Bloxyz
{
    public interface IInputReader
    {
        //ICommand spawnCommand { get; set; }
        ICommand startGameCommand { get; set; }
        ICommand pauseResumeCommand { get; set; }
        ICommand rotateLeftCommand { get; set; }
        ICommand rotateRightCommand { get; set; }
        ICommand quickLandCommand { get; set; }

        //ICommand spawnFirstCommand { get; set; }
        //ICommand spawnSecondCommand { get; set; }
        //ICommand spawnThirdCommand { get; set; }


    }
}

