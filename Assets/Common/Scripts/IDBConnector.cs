public interface IDBConnector
{
    string timeFormat { get; }

    void CreateGamesTable();
    void InsertGame(Minigame game);
    void DeleteGame(int id);
    void DeleteGame(string name);
    void DeleteAllGames();
    void DropGamesTable();

    void CreateScoresTable();
    void InsertScore(Score score);
    void DeleteScore(int id);
    void DeleteScores(string from, string to);
    void DeleteScores(int playerId);
    void DeleteScores(int playerId, string from, string to);
    void DeleteScores(int playerId, int gameId);
    void DeleteScores(int playerId, int gameId, string from, string to);
    void DeleteAllScores();
    void DropScoresTable();

    Score GetScoreById(int id);
    Score GetScoreByTime(int playerId, string time);
    Score[] GetScores(int playerId, int gameId);
    Score[] GetScoresByRank(int playerId, int gameId, int rank);
    Score[] GetScoresByDay(int playerId, int gameId, string day);
    Score[] GetScoresByMonth(int playerId, int gameId, string month);
    Score[] GetScoresByYear(int playerId, int gameId, string year);
    Score[] GetScoresByTimePeriod(int playerId, int gameId, string from, string to);

    Score[] GetRecentScores(int playerId, int gameId);
    Score[] GetRecentScores(int playerId, int gameId, int limit, int offset);
    Score GetRecentScoreByRank(int playerId, int gameId, int rank);
    int GetScoreRank(int playerId, int gameId, int id);
}
