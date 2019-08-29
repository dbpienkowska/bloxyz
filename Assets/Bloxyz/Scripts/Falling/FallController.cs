using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using MEC;

namespace Bloxyz
{
    public class FallController : MonoBehaviour, IPlatformUser, ITimerUser
    {
        public Platform platform { private get; set; }
        public Timer timer { private get; set; }

        public float startSpeed = 1f;
        public float maxSpeed = 4f;
        public float quickLandSpeed = 8f;
        public float nearGroundThreshold = 0.5f;
        public AnimationCurve speedCurve;
        public int maxSpeedReachSeconds = 60;

        private float _speed;
        private CoroutineHandle _currentCoroutine;
        private Block _block;
        private IOnBlockGroundedWatcher[] _groundedWatchers;
        private IOnBlockNearGroundWatcher[] _nearGroundWatchers;
        private bool _nearGround;
        private IOnQuickLandStartedWatcher[] _quickLandWatchers;

        public void StartFall(Block block)
        {
            _currentCoroutine = Timing.RunCoroutine(_FallCoroutine(block, _speed));
        }

        public void QuickLand()
        {
            if(!_block)
                return;

            _EmitOnQuickLandStarted();
            Timing.KillCoroutines(_currentCoroutine);
            _currentCoroutine = Timing.RunCoroutine(_FallCoroutine(_block, quickLandSpeed));
        }

        private void _IncreseSpeed()
        {
            float eval = speedCurve.Evaluate(timer.time / maxSpeedReachSeconds);
            _speed = startSpeed + eval * (maxSpeed - startSpeed);
        }

        private IEnumerator<float> _FallCoroutine(Block block, float speed)
        {
            Assert.IsNotNull(block);
            Assert.IsTrue(speed > 0f);

            _block = block;
            _nearGround = false;
            
            while(!_CheckGround(block, speed))
            {
                if(!_nearGround && block.transform.position.y < platform.groundMax + nearGroundThreshold)
                {
                    _EmitOnBlockNearGround(block);
                    _nearGround = true;
                }

                block.transform.Translate(Vector3.down * speed * Time.deltaTime);
                yield return 0f;
            }
            
            block.RoundPosition();
            _block = null;

            _IncreseSpeed();

            _EmitOnBlockGrounded(block, speed);
        }

        private void _EmitOnBlockGrounded(Block block, float speed)
        {
            foreach(var watcher in _groundedWatchers)
                watcher.OnBlockGrounded(block, speed);
        }

        private void _EmitOnBlockNearGround(Block block)
        {
            foreach(var watcher in _nearGroundWatchers)
                watcher.OnBlockNearGround(block);
        }

        private void _EmitOnQuickLandStarted()
        {
            foreach(var watcher in _quickLandWatchers)
                watcher.OnQuickLandStarted();
        }

        private bool _CheckGround(Block block, float speed)
        {
            float speedCompensation = 0.02f * speed;

            foreach(CubeSlot slot in block.bottomSlots)
            {
                int ground = platform.GetGround(slot.x, slot.z);
                if(slot.transform.position.y - speedCompensation <= platform.GetGround(slot.x, slot.z))
                    return true;
            }

            return false;
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
            _speed = startSpeed;

            _groundedWatchers = RootProvider.root.GetComponentsInChildren<IOnBlockGroundedWatcher>();
            _nearGroundWatchers = RootProvider.root.GetComponentsInChildren<IOnBlockNearGroundWatcher>();
            _quickLandWatchers = RootProvider.root.GetComponentsInChildren<IOnQuickLandStartedWatcher>();

            if(speedCurve == null)
                speedCurve = AnimationCurve.Constant(0, 1, 1);
        }

        private void Start()
        {
            Assert.IsNotNull(_groundedWatchers);
            Assert.IsNotNull(_nearGroundWatchers);
        }
    }
}

