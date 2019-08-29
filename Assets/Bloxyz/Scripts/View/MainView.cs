using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Bloxyz
{
    public class MainView : MonoBehaviour, IOnStartedWatcher, IOnPausedWatcher, IOnResumedWatcher, IOnScoreSavedWatcher
    {
        public PanelView startPanel;
        public PanelView pausedPanel;
        public PanelView scoreSection;
        public GameOverPanel overPanel;
        public DataAgent dataAgent;

        public void OnGameStarted()
        {
            startPanel.Hide();
            scoreSection.Show();
        }

        public void OnGamePaused()
        {
            pausedPanel.Show();
            scoreSection.SetAlpha(0.5f);
        }

        public void OnGameResumed()
        {
            pausedPanel.Hide();
            scoreSection.SetAlpha(1f);
        }

        public void OnScoreSaved(Score score)
        {
            scoreSection.Hide();

            overPanel.cheerText.text = "Yay! Made it!";
            overPanel.scoreText.text = score.score.ToString();
            overPanel.rankText.text = "Rank: " + score.rank.ToString();

            Score[] topScores = dataAgent.GetTopScores(overPanel.scoreListLimit, 0);
            for(int i = 0; i < topScores.Length; i++)
            {
                if(topScores[i] == score)
                    overPanel.SetScoreListItem(topScores[i], true);
                else
                    overPanel.SetScoreListItem(topScores[i]);
            }

            overPanel.Show();
        }

        private void Awake()
        {
            _ActivateViews();
            scoreSection.Hide();
            pausedPanel.Hide();
            overPanel.Hide();
        }

        private void _ActivateViews()
        {
            startPanel.gameObject.SetActive(true);
            pausedPanel.gameObject.SetActive(true);
            scoreSection.gameObject.SetActive(true);
            overPanel.gameObject.SetActive(true);
        }

    }
}

