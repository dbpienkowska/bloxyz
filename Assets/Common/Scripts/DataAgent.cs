using System;
using UnityEngine;

public class DataAgent : MonoBehaviour
{
    public string timeFormat => _connector.timeFormat;

    public int playerId = 0;
    public int gameId = 0;

    private IDBConnector _connector;

    public void CreateTables()
    {
        _connector.CreateGamesTable();
        _connector.CreateScoresTable();
    }

    public void AddScore(Score score)
    {
        _connector.InsertScore(score);
    }

    public void ClearScores()
    {
        _connector.DeleteScores(playerId, gameId);
    }

    public int GetScoreRank(int id)
    {
        return _connector.GetScoreRank(playerId, gameId, id);
    }

    public Score GetScoreById(int id)
    {
        return _connector.GetScoreById(id);
    }

    public Score GetScoreByTime(string time)
    {
        return _connector.GetScoreByTime(playerId, time);
    }

    public Score[] GetScoresByRank(int rank)
    {
        return _connector.GetScoresByRank(playerId, gameId, rank);
    }

    public Score[] GetAllScores()
    {
        return _connector.GetScores(playerId, gameId);
    }

    public Score[] GetScoresByDay(DateTime day)
    {
        string dayString = day.ToString(_connector.timeFormat);
        return _connector.GetScoresByDay(playerId, gameId, dayString);
    }

    public Score[] GetScoresByMonth(DateTime month)
    {
        string monthString = month.ToString(_connector.timeFormat);
        return _connector.GetScoresByMonth(playerId, gameId, monthString);
    }

    public Score[] GetScoresByYear(DateTime year)
    {
        string yearString = year.ToString(_connector.timeFormat);
        return _connector.GetScoresByYear(playerId, gameId, yearString);
    }

    public Score[] GetScoresByYear(int year)
    {
        string yearString = year.ToString();
        return _connector.GetScoresByYear(playerId, gameId, yearString);
    }

    public Score[] GetScoresByTimePeriod(DateTime from, DateTime to)
    {
        string fromString = from.ToString(_connector.timeFormat);
        string toString = to.ToString(_connector.timeFormat);
        return _connector.GetScoresByTimePeriod(playerId, gameId, fromString, toString);
    }

    public Score[] GetTopScores(int limit, int offset)
    {
        return _connector.GetRecentScores(playerId, gameId, limit, offset);
    }

    private void Awake()
    {
        _connector = new SqliteConnector();
    }

    private void Start()
    {
        CreateTables();
    }
}
