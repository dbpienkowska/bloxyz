using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Bloxyz
{
    public class BlockTracker : MonoBehaviour, IPlatformUser
    {
        public Platform platform { private get; set; }
        public GameObject trackPrefab;

        private Track[] _tracks;
        private int _trackNumber;
        private List<Track> _activeTracks;

        private void _InitTracks()
        {
            _trackNumber = platform.size * platform.size;
            _tracks = new Track[_trackNumber];

            for(int i = 0; i < _trackNumber; i++)
            {
                Track track = Instantiate(trackPrefab, transform).GetComponent<Track>();
                track.transform.ChangeLocalPosition(x: i % platform.size, z: i / platform.size);
                track.Hide();
                _tracks[i] = track;
            }

            _activeTracks = new List<Track>(_trackNumber - 1);
        }

        public void SetTracking(Block block)
        {
            foreach(CubeSlot slot in block.bottomSlots)
            {
                Track track = _tracks[_Index(slot.x, slot.z)];
                track.SetY(platform.GetGround(track.x, track.z));
                track.Show();
                _activeTracks.Add(track);
            }
        }

        public void UpdateTracks()
        {
            foreach(Track track in _activeTracks)
            {
                track.SetY(platform.GetGround(track.x, track.z));
                track.Show();
            }
        }

        public void HideTracks()
        {
            foreach(Track track in _activeTracks)
                track.Hide();
        }

        public void Untrack()
        {
            HideTracks();
            _activeTracks.Clear();
        }

        private int _Index(int x, int z)
        {
            Assert.IsFalse(x < 0);
            Assert.IsFalse(z < 0);

            int i = platform.size * z + x;

            Assert.IsTrue(i < _trackNumber);

            return i;
        }

        private void Start()
        {
            _InitTracks();
        }
    }
}

