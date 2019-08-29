using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Bloxyz
{
    public class Supervisor : MonoBehaviour, IOnBlockSpawnedWatcher, IOnQuickLandStartedWatcher, IOnBlockNearGroundWatcher, IOnBlockGroundedWatcher, IOnRotateWatcher, IOnLevelClearedWatcher, IOnLevelsHandledWatcher
    {
        public BlockSpawner spawner;
        public FallController fallController;
        public PlatformRotater rotater;
        public BlockTracker tracker;
        public OverPointTracker overPointTracker;
        public LevelHandler levelHandler;
        public MoveController cameraController;
        public MoveController spawnerController;
        public Scorer scorer;

        private Timer _timer;
        private ITimerUser[] _timerUsers;
        private IOnStartedWatcher[] _onStartedWatchers;
        private IOnPausedWatcher[] _onPausedWatchers;
        private IOnResumedWatcher[] _onResumedWatchers;
        private IOnOverWatcher[] _onOverWatchers;

        public bool started { get; private set; }
        public bool paused { get; private set; }
        public bool over { get; private set; }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ExitGame()
        {
            // TODO: setting scene
            Application.Quit();
        }

        public void ShowLeaderboard()
        {
            // TODO: proper logic
            SceneManager.LoadScene(0);
        }

        public void StartGame()
        {
            if(started || over)
                return;

            Debug.Log("Game started");

            _StartCycle();
            
            started = true;
            over = false;
            _timer.Reset();
            _timer.Start();

            _EmitOnGameStarted();
        }

        public void PauseGame()
        {
            if(!started || over)
                return;

            Debug.Log("Game paused");

            _timer.Stop();
            _EnableComponents(false);
            paused = true;

            _EmitOnGamePaused();
        }

        public void ResumeGame()
        {
            if(!started || over)
                return;

            Debug.Log("Game resumed");

            _timer.Start();
            _EnableComponents(true);
            paused = false;

            _EmitOnGameResumed();
        }

        public void OnBlockSpawned(Block block)
        {
            fallController.StartFall(block);
            tracker.SetTracking(block);
        }

        public void OnQuickLandStarted()
        {
            rotater.QuicklyFinishCurrentRotation();
            rotater.blocked = true;
        }

        public void OnBlockNearGround(Block block)
        {
            rotater.blocked = true;
        }

        public void OnBlockGrounded(Block block, float speed)
        {
            tracker.Untrack();
            scorer.AddPoints(block);

            Cube[] cubes = block.UnloadCubes();

            rotater.blocked = false;

            if(levelHandler.IsLimitReached(cubes))
                OnGameOver();
            else
            {
                levelHandler.PutCubesInLevels(cubes);
                List<Level> filledLevels = levelHandler.GetFilledLevels();
                if(filledLevels.Count > 0)
                    levelHandler.ClearLevels();
                else
                {
                    cameraController.Move(1);
                    spawnerController.Move(1);
                    _StartCycle();
                }
            }

            BlockDisposer.Dispose(block);
        }

        public void OnGameOver()
        {
            Debug.Log("Game over");

            _EnableComponents(false);

            _timer.Stop();
            over = true;
            
            _EmitOnGameOver(scorer.points);
        }

        public void OnRotationStarted()
        {
            tracker.HideTracks();
        }

        public void OnRotationFinished()
        {
            tracker.UpdateTracks();
        }

        public void OnLevelCleared(Level level)
        {
            cameraController.Move(-1);
            spawnerController.Move(-1);
            scorer.AddPoints(level);
        }

        public void OnLevelsHandled()
        {
            _StartCycle();
        }

        private void _StartCycle()
        {
            overPointTracker.Track();
            spawner.Spawn();
        }

        private void _EnableComponents(bool enable)
        {
            spawner.enabled = enable;
            fallController.enabled = enable;
            rotater.enabled = enable;
            levelHandler.enabled = enable;
        }

        private void _EmitOnGameStarted()
        {
            foreach(var watcher in _onStartedWatchers)
                watcher.OnGameStarted();
        }

        private void _EmitOnGamePaused()
        {
            foreach(var watcher in _onPausedWatchers)
                watcher.OnGamePaused();
        }

        private void _EmitOnGameResumed()
        {
            foreach(var watcher in _onResumedWatchers)
                watcher.OnGameResumed();
        }

        private void _EmitOnGameOver(int score)
        {
            foreach(var watcher in _onOverWatchers)
                watcher.OnGameOver(score);
        }

        private void Awake()
        {
            started = false;
            paused = false;

            Assert.IsNotNull(spawner);
            Assert.IsNotNull(fallController);
            Assert.IsNotNull(rotater);
            Assert.IsNotNull(tracker);
            Assert.IsNotNull(levelHandler);
            Assert.IsNotNull(cameraController);
            Assert.IsNotNull(scorer);

            _onStartedWatchers = RootProvider.root.GetComponentsInChildren<IOnStartedWatcher>();
            _onPausedWatchers = RootProvider.root.GetComponentsInChildren<IOnPausedWatcher>();
            _onResumedWatchers = RootProvider.root.GetComponentsInChildren<IOnResumedWatcher>();
            _onOverWatchers = RootProvider.root.GetComponentsInChildren<IOnOverWatcher>();

            _InitTimer();
        }

        private void _InitTimer()
        {
            _timer = new Timer();
            _timerUsers = RootProvider.root.GetComponentsInChildren<ITimerUser>();
            foreach(var user in _timerUsers)
                user.timer = _timer;
        }
    }
}

