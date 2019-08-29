using UnityEngine;

namespace Bloxyz
{
    public class OverPointTracker : MonoBehaviour, IPlatformUser
    {
        public Platform platform { private get; set; }
        public GameObject trackPrefab;
        public Track track { get; private set; }
        public int trackDistance = 1;

        public void Track()
        {
            if(platform.groundMax >= platform.limit - trackDistance)
                track.Show();
            else
                track.Hide();
        }

        private void _InitTrack()
        {
            track = Instantiate(trackPrefab, platform.overPointTransform).GetComponent<Track>();

            Vector3 scale = track.transform.localScale;
            scale.x = platform.size;
            scale.z = platform.size;
            track.transform.localScale = scale;

            track.MultiplyColor(Color.red);
            //track.transform.ChangeLocalPosition(x: i % platform.size, z: i / platform.size);
            track.Hide();
        }

        private void Awake()
        {
            _InitTrack();
        }
    }
}

