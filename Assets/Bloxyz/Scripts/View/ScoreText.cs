using UnityEngine;
using TMPro;

namespace Bloxyz
{
    public class ScoreText : MonoBehaviour, IOnScoreUpdatedWatcher
    {
        public TextMeshProUGUI _text;

        public void OnScoreUpdated(int score)
        {
            _text.text = score.ToString();
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = "0";
        }
    }
}

