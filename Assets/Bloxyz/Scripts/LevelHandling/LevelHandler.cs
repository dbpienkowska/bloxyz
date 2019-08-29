using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using MEC;

namespace Bloxyz
{
    public class LevelHandler : MonoBehaviour, IPlatformUser
    {
        public Platform platform { private get; set; }
        public float shiftSpeed = 2f;

        private List<Level> _filledLevels = new List<Level>(Block.MAX_HEIGHT);

        private int _shiftedCount = 0;
        private int _shiftAmount = 0;
        private IOnLevelClearedWatcher[] _clearedWatchers;
        private IOnLevelsHandledWatcher[] _handledWatchers;

        public void PutCubesInLevels(Cube[] cubes)
        {
            Assert.IsNotNull(cubes);
            Assert.IsTrue(cubes.Length > 0);

            foreach(Cube cube in cubes)
            {
                platform.levels[cube.y].SetCube(cube);
                platform.UpdateGround(cube);
            }
        }

        public bool IsLimitReached(Cube[] cubes) // true if limit reached
        {
            foreach(Cube cube in cubes)
            {
                if(cube.y == platform.limit)
                    return true;
            }

            return false;
        }

        public List<Level> GetFilledLevels()
        {
            _filledLevels.Clear();

            foreach(Level level in platform.levels)
            {
                if(level.cubeCount == level.capacity)
                    _filledLevels.Add(level);
            }
            
            return _filledLevels;
        }

        public void ClearLevels()
        {
            Level level = _filledLevels[0];
            _Clear(level);
            _filledLevels.RemoveAt(0);
            //_Shift(level);
        }

        private void _Clear(Level level)
        {
            Cube[] cubes = level.PullCubes();

            foreach(Cube cube in cubes)
                CubeDisposer.Dispose(cube);

            //platform.levelClearParticles[level.localY].Play();
            Timing.RunCoroutine(_ClearCoroutine(level));
        }

        private void _Shift(Level clearedLevel)
        {
            _shiftedCount = 0;
            _shiftAmount = 0;

            for(int i = 1; i < platform.limit; i++)
            {
                Level level = platform.levels[i];
                if(level.localY > clearedLevel.localY)
                {
                    _shiftAmount++;
                    Timing.RunCoroutine(_ShiftCoroutine(level, shiftSpeed), "ShiftCoroutine");
                }
            }

            clearedLevel.transform.ChangeLocalPosition(y: platform.limit - 1);

            if(_shiftAmount == 0)
            {
                _FixPlatformAfterShift();
                _EmitOnLevelsHandled();
            }
        }

        private void _FixPlatformAfterShift()
        {
            System.Array.Sort(platform.levels);
            platform.LowerGrounds(1);
        }

        private IEnumerator<float> _ShiftCoroutine(Level level, float fallSpeed)
        {
            float targetY = level.localY - 1;

            while(level.transform.localPosition.y > targetY)
            {
                level.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
                yield return 0f;
            }

            level.RoundPositionY();

            _shiftedCount++;
            if(_shiftedCount == _shiftAmount)
            {
                _FixPlatformAfterShift();
                if(_filledLevels.Count > 0)
                    ClearLevels();
                else
                    _EmitOnLevelsHandled();
            }
        }

        private IEnumerator<float> _ClearCoroutine(Level level)
        {
            ParticleSystem particle = platform.levelClearParticles[level.localY];
            particle.Play();

            while(particle.time < particle.main.duration / 2)
                yield return 0f;

            platform.groundMax--;
            _EmitOnLevelCleared(level);
            _Shift(level);
        }

        private void _EmitOnLevelCleared(Level level)
        {
            foreach(var watcher in _clearedWatchers)
                watcher.OnLevelCleared(level);
        }

        private void _EmitOnLevelsHandled()
        {
            foreach(var watcher in _handledWatchers)
                watcher.OnLevelsHandled();
        }

        private void OnDisable()
        {
            Timing.PauseCoroutines("ShiftCoroutine");
        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines("ShiftCoroutine");
        }

        private void Awake()
        {
            _clearedWatchers = RootProvider.root.GetComponentsInChildren<IOnLevelClearedWatcher>();
            _handledWatchers = RootProvider.root.GetComponentsInChildren<IOnLevelsHandledWatcher>();
        }
    }
}

