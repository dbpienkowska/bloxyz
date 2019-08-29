using UnityEngine;
using UnityEngine.Assertions;
using System;
using TMPro;

namespace Bloxyz
{
    public class GameOverPanel : PanelView
    {
        public TextMeshProUGUI cheerText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI rankText;

        public RectTransform scoreListParent;
        public GameObject scoreItemPrefab;
        public int scoreListLimit = 5;

        private ScoreListItem[] _scoreList;

        private void Start()
        {
            Assert.IsNotNull(cheerText);
            Assert.IsNotNull(scoreText);
            Assert.IsNotNull(rankText);
            Assert.IsNotNull(scoreListParent);

            _scoreList = new ScoreListItem[scoreListLimit];
            for(int i = 0; i < scoreListLimit; i++)
            {
                _scoreList[i] = Instantiate(scoreItemPrefab, scoreListParent).GetComponent<ScoreListItem>();
                _scoreList[i].SetRank(i + 1);
                _scoreList[i].gameObject.SetActive(false);
            }
        }

        public void SetScoreListItem(Score score, bool emphasize = false)
        {
            ScoreListItem item = _scoreList[score.rank - 1];
            item.SetScore(score.score);

            DateTime scoreTime = DateTime.Parse(score.time, null, System.Globalization.DateTimeStyles.RoundtripKind);
            item.SetTime(TimeSpanTranslator.Translate(scoreTime, DateTime.Now));

            if(emphasize)
                item.Emphasize();

            _scoreList[score.rank - 1].gameObject.SetActive(true);
        }
    }
}
