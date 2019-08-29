using UnityEngine;

namespace Bloxyz
{
    public class MoveController : MonoBehaviour, IPlatformUser
    {
        public Platform platform { private get; set; }

        public Transform target;
        public float duration = 0.5f;
        public Vector3 translation = new Vector3(-0.5f, 1f, -0.5f);

        private int _groundMax = 0;

        public void Move(int direction)
        {
            int groundDiff = Mathf.Abs(platform.groundMax - _groundMax);
            if(groundDiff != 0)
            {
                if(duration == 0)
                    ObjectMover.MoveInstantly(target, translation * direction * groundDiff);
                else
                    ObjectMover.Move(target, translation * direction * groundDiff, duration);
                _groundMax = platform.groundMax;
            }
        }
    }
}

