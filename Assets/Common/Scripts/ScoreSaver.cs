using UnityEngine;

public class ScoreSaver : MonoBehaviour, IOnOverWatcher
{
    public DataAgent dataAgent;

    private Score _score;
    private IOnScoreSavedWatcher[] _onSavedWatchers;

    public void OnGameOver(int score)
    {
        Save(score);
    }

    public void Save(int score)
    {
        string timeString = System.DateTime.Now.ToString(dataAgent.timeFormat);
        _score.time = timeString;
        _score.score = score;
        dataAgent.AddScore(_score);
        _score = dataAgent.GetScoreByTime(timeString);

        _EmitOnScoreSaved(_score);
    }

    private void _EmitOnScoreSaved(Score score)
    {
        foreach(var watcher in _onSavedWatchers)
            watcher.OnScoreSaved(score);
    }

    private void Awake()
    {
        _score = new Score
        {
            playerId = dataAgent.playerId,
            gameId = dataAgent.gameId,
            time = "",
            score = -1
        };

        _onSavedWatchers = RootProvider.root.GetComponentsInChildren<IOnScoreSavedWatcher>();
    }
}