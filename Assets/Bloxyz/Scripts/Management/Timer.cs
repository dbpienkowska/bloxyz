using System.Diagnostics;

namespace Bloxyz
{
    public class Timer
    {
        public float time => (float)_stopwatch.Elapsed.TotalSeconds;
        Stopwatch _stopwatch;

        public Timer()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public void Reset()
        {
            _stopwatch.Reset();
        }
    }
}

