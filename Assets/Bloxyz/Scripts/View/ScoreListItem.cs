using TMPro;
using UnityEngine;

namespace Bloxyz
{
    public class ScoreListItem : MonoBehaviour
    {
        public TextMeshProUGUI rankText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI timeText;

        private RectTransform _rectTransform;

        public void SetRank(int rank)
        {
            rankText.text = rank.ToString() + ". ";
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void SetTime(string time)
        {
            timeText.text = time;
        }

        public void Emphasize()
        {
            rankText.fontStyle = FontStyles.Bold;
            scoreText.fontStyle = FontStyles.Bold;
            timeText.fontStyle = FontStyles.Bold;

            rankText.fontSize += 4;
            scoreText.fontSize += 4;
            timeText.fontSize += 4;

            Vector2 size = _rectTransform.sizeDelta;
            size.y += 24;
            _rectTransform.sizeDelta = size;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}

