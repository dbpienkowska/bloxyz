using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Bloxyz
{
    public class PlatformRotater : MonoBehaviour, IPlatformUser
    {
        public Platform platform { private get; set; }
        public float duration = 0.5f;
        public bool blocked = false;
        
        private float _leftOver;
        private CoroutineHandle _currentCoroutine;
        private IOnRotateWatcher[] _rotateWatchers;

        public void Rotate(int angle)
        {
            if(!blocked)
            {
                Timing.KillCoroutines(_currentCoroutine);
                _currentCoroutine = Timing.RunCoroutine(_RotationCoroutine(angle, duration));
                _EmitOnRotationStarted();
            }
        }

        public void QuicklyFinishCurrentRotation()
        {
            Timing.KillCoroutines(_currentCoroutine);
            _currentCoroutine = Timing.RunCoroutine(_RotationCoroutine(0, duration / 4));
        }

        private void _EmitOnRotationStarted()
        {
            foreach(var watcher in _rotateWatchers)
                watcher.OnRotationStarted();
        }

        private void _EmitOnRotationFinished()
        {
            foreach(var watcher in _rotateWatchers)
                watcher.OnRotationFinished();
        }

        private IEnumerator<float> _RotationCoroutine(int angle, float duration)
        {
            float startRotation = platform.transform.eulerAngles.y;
            Vector3 rotation = platform.transform.eulerAngles;

            float actualDuration = duration;
            if(_leftOver * angle < 0)
                actualDuration -= duration * Mathf.Abs(_leftOver) / Mathf.Abs(angle);

            float compensatedAngle = angle + _leftOver;
            _leftOver = compensatedAngle;

            float time = 0f;

            while(time < actualDuration)
            {
                rotation.y = compensatedAngle * Time.deltaTime / actualDuration;
                platform.transform.Rotate(rotation);

                _leftOver -= rotation.y;
                time += Time.deltaTime;

                yield return 0f;
            }

            rotation.y = Mathf.RoundToInt(startRotation + compensatedAngle);
            platform.transform.eulerAngles = rotation;
            _leftOver = 0;

            _EmitOnRotationFinished();
        }

        private void OnDisable()
        {
            Timing.PauseCoroutines(_currentCoroutine);
        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(_currentCoroutine);
        }

        private void Awake()
        {
            _rotateWatchers = RootProvider.root.GetComponentsInChildren<IOnRotateWatcher>();
        }
    }
}

