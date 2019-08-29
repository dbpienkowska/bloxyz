using UnityEngine;

namespace Bloxyz
{
    public class Scorer : MonoBehaviour, ITimerUser
    {
        public Timer timer { private get; set; }

        public int points { get; private set; }
        public float maxMultiplier = 4f;
        public AnimationCurve curve;
        public int maxScoreMultiplierSeconds = 90;

        private IOnScoreUpdatedWatcher[] _updatedWatchers;

        public void AddPoints(Block block)
        {
            points += block.slots.Length;
            _EmitOnScoreUpdated(points);
        }

        public void AddPoints(Level level)
        {
            float multiplier = 1 + (curve.Evaluate(timer.time / maxScoreMultiplierSeconds) * (maxMultiplier - 1));
            points += Mathf.RoundToInt(level.capacity * multiplier);
            _EmitOnScoreUpdated(points);
        }

        public void ResetScore()
        {
            points = 0;
            _EmitOnScoreUpdated(points);
        }

        private void _EmitOnScoreUpdated(int score)
        {
            foreach(var watcher in _updatedWatchers)
                watcher.OnScoreUpdated(score);
        }

        private void Awake()
        {
            points = 0;
            
            if(curve == null)
                curve = AnimationCurve.Constant(0, 1, 1);

            _updatedWatchers = RootProvider.root.GetComponentsInChildren<IOnScoreUpdatedWatcher>(true);
        }
    }
}

