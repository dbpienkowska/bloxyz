using UnityEngine;
using System.Data;
using System.Collections.Generic;
using Mono.Data.Sqlite;

public class SqliteConnector : IDBConnector
{
    public string timeFormat => "yyyy-MM-dd HH:mm:ss.fff";

    private readonly string _dbPath;
    private SqliteConnection _connection;
    private SqliteCommand _command;
    private SqliteDataReader _reader;

    public SqliteConnector()
    {
        _dbPath = "URI=file:" + Application.persistentDataPath + "/Database.db";

        _connection = new SqliteConnection(_dbPath);
        _command = new SqliteCommand(_connection)
        {
            CommandType = CommandType.Text
        };
    }


    public void CreateGamesTable()
    {
        _command.Parameters.Clear();
        _command.CommandText =
            "CREATE TABLE IF NOT EXISTS Games(id INTEGER PRIMARY KEY, name TEXT NOT NULL);";

        _ExecuteCommand();
    }

    public void InsertGame(Minigame game)
    {
        _command.Parameters.Clear();
        _command.CommandText = "INSERT INTO Games(name) VALUES (@name);";
        _AddCommandParameter("name", game.name);

        _ExecuteCommand();
    }

    public void DeleteGame(int id)
    {
        _command.Parameters.Clear();
        _command.CommandText = "DELETE FROM Games WHERE id = @id;";
        _AddCommandParameter("id", id);

        _ExecuteCommand();
    }

    public void DeleteGame(string name)
    {
        _command.Parameters.Clear();
        _command.CommandText = "DELETE FROM Games WHERE name = @name;";
        _AddCommandParameter("name", name);

        _ExecuteCommand();
    }

    public void DeleteAllGames()
    {
        _command.Parameters.Clear();
        _command.CommandText = "DELETE FROM Games;";

        _ExecuteCommand();
    }

    public void DropGamesTable()
    {
        _command.Parameters.Clear();
        _command.CommandText = "DROP TABLE IF EXISTS Games;";

        _ExecuteCommand();
    }


    public void CreateScoresTable()
    {
        _command.Parameters.Clear();
        _command.CommandText = 
            "CREATE TABLE IF NOT EXISTS Scores("
            + "id INTEGER PRIMARY KEY, "
            + "playerId INTEGER NOT NULL, "
            + "gameId INTEGER NOT NULL, "
            + "time TEXT NOT NULL, "
            + "score INTEGER NOT NULL, "
            + "UNIQUE(playerId, time));";

        _ExecuteCommand();
    }

    public void InsertScore(Score score)
    {
        _command.Parameters.Clear();
        _command.CommandText = 
            "INSERT INTO Scores(playerId, gameId, time, score) " +
            "VALUES (@playerId, @gameId, @time, @score);";
        _AddCommandParameter("playerId", score.playerId);
        _AddCommandParameter("gameId", score.gameId);
        _AddCommandParameter("time", score.time);
        _AddCommandParameter("score", score.score);

        _ExecuteCommand();
    }

    public void DeleteScore(int id)
    {
        _command.Parameters.Clear();
        _command.CommandText = "DELETE FROM Scores WHERE id = @id";
        _AddCommandParameter("id", id);

        _ExecuteCommand();
    }

    public void DeleteScores(string from, string to)
    {
        _command.Parameters.Clear();
        _command.CommandText = "DELETE FROM Scores WHERE time >= @from AND time < @to";
        _AddCommandParameter("from", from);
        _AddCommandParameter("to", to);

        _ExecuteCommand();
    }

    public void DeleteScores(int playerId)
    {
        _command.Parameters.Clear();
        _command.CommandText = "DELETE FROM Scores WHERE playerId = @playerId;";
        _AddCommandParameter("playerId", playerId);

        _ExecuteCommand();
    }

    public void DeleteScores(int playerId, string from, string to)
    {
        _command.Parameters.Clear();
        _command.CommandText = 
            "DELETE FROM Scores WHERE playerId = @playerId " +
            "AND time >= @from AND time < @to";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("from", from);
        _AddCommandParameter("to", to);

        _ExecuteCommand();
    }

    public void DeleteScores(int playerId, int gameId)
    {
        _command.Parameters.Clear();
        _command.CommandText = 
            "DELETE FROM Scores WHERE playerId = @playerId AND gameId = @gameId;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);

        _ExecuteCommand();
    }

    public void DeleteScores(int playerId, int gameId, string from, string to)
    {
        _command.Parameters.Clear();
        _command.CommandText = 
            "DELETE FROM Scores WHERE playerId = @playerId AND gameId = @gameId " +
            "AND time >= @from AND time < @to";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);
        _AddCommandParameter("from", from);
        _AddCommandParameter("to", to);

        _ExecuteCommand();
    }

    public void DeleteAllScores()
    {
        _command.Parameters.Clear();
        _command.CommandText = "DELETE FROM Scores;";

        _ExecuteCommand();
    }

    public void DropScoresTable()
    {
        _command.Parameters.Clear();
        _command.CommandText = "DROP TABLE IF EXISTS Scores;";
        
        _ExecuteCommand();
    }


    public Score GetScoreById(int id)
    {
        Score score = new Score();

        _command.Parameters.Clear();
        _command.CommandText = 
            "SELECT id, playerId, gameId, time, score FROM Scores WHERE id = @id;";
        _AddCommandParameter("id", id);

        _ExecuteReaderCommand();

        if(_reader.Read())
            score = _ReadScore();

        _CloseReader();

        return score;
    }

    public Score GetScoreByTime(int playerId, string time)
    {
        Score score = new Score();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, "
              + "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank " +
              "FROM Scores WHERE playerId = @playerId) " +
            "WHERE time = @time;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("time", time);

        _ExecuteReaderCommand();

        if(_reader.Read())
            score = _ReadScore();

        _CloseReader();

        return score;
    }

    public Score[] GetScores(int playerId, int gameId)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText = 
            "SELECT id, playerId, gameId, time, score, "
             + "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank " +
            "FROM Scores WHERE playerId = @playerId AND gameId = @gameId " +
            "ORDER BY time DESC;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());

        _CloseReader();

        return scoreList.ToArray();
    }

    public Score[] GetScoresByRank(int playerId, int gameId, int rank)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText = 
            "SELECT id, playerId, gameId, time, score, scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, " +
              "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank "
            + "FROM Scores WHERE playerId = @playerId AND gameId = @gameId) " +
            "WHERE scoreRank = @rank " +
            "ORDER BY time DESC;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);
        _AddCommandParameter("rank", rank);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());
        
        _CloseReader();

        return scoreList.ToArray();
    }

    public Score[] GetScoresByDay(int playerId, int gameId, string day)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, " +
              "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank "
            + "FROM Scores WHERE playerId = @playerId AND gameId = @gameId) " +
            "WHERE DATE(time) = @day " +
            "ORDER BY time DESC;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);
        _AddCommandParameter("day", day);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());

        _CloseReader();

        return scoreList.ToArray();
    }

    public Score[] GetScoresByMonth(int playerId, int gameId, string month)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, " +
              "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank "
            + "FROM Scores WHERE playerId = @playerId AND gameId = @gameId) " +
            "WHERE STRFTIME('%Y-%m', time) = @month " +
            "ORDER BY time DESC;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);
        _AddCommandParameter("month", month);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());

        _CloseReader();

        return scoreList.ToArray();
    }

    public Score[] GetScoresByYear(int playerId, int gameId, string year)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, " +
              "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank "
            + "FROM Scores WHERE playerId = @playerId AND gameId = @gameId) " +
            "WHERE STRFTIME('%Y', time) = @year " +
            "ORDER BY time DESC;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);
        _AddCommandParameter("year", year);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());

        _CloseReader();

        return scoreList.ToArray();
    }

    public Score[] GetScoresByTimePeriod(int playerId, int gameId, string from, string to)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, " +
              "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank "
            + "FROM Scores WHERE playerId = @playerId AND gameId = @gameId) " +
            "WHERE time >= @from AND time < @to " +
            "ORDER BY time DESC;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);
        _AddCommandParameter("from", from);
        _AddCommandParameter("to", to);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());

        _CloseReader();

        return scoreList.ToArray();
    }

    public Score[] GetRecentScores(int playerId, int gameId)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText = 
            "SELECT id, playerId, gameId, time, score, "
            + "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank " +
            "FROM (SELECT * FROM Scores WHERE playerId = @playerId AND gameId = @gameId "
              + "AND time IN (SELECT MAX(time) FROM Scores GROUP BY score));";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", playerId);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());

        _CloseReader();

        return scoreList.ToArray();
    }

    public Score[] GetRecentScores(int playerId, int gameId, int limit, int offset)
    {
        List<Score> scoreList = new List<Score>();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, "
            + "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank " +
            "FROM (SELECT * FROM Scores WHERE playerId = @playerId AND gameId = @gameId "
              + "AND time IN (SELECT MAX(time) FROM Scores GROUP BY score)) " +
            "LIMIT @limit OFFSET @offset;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", playerId);
        _AddCommandParameter("limit", limit);
        _AddCommandParameter("offset", offset);

        _ExecuteReaderCommand();

        while(_reader.Read())
            scoreList.Add(_ReadScore());

        _CloseReader();

        return scoreList.ToArray();
    }

    public Score GetRecentScoreByRank(int playerId, int gameId, int rank)
    {
        Score score = new Score();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, " +
            "scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, "
              + "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank"
            + " FROM Scores WHERE playerId = @playerId AND gameId = @gameId "
              + "AND time IN (SELECT MAX(time) FROM Scores GROUP BY score))" +
            "WHERE scoreRank = @rank;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", playerId);
        _AddCommandParameter("rank", rank);

        _ExecuteReaderCommand();

        if(_reader.Read())
            score = _ReadScore();

        _CloseReader();

        return score;
    }

    public int GetScoreRank(int playerId, int gameId, int id)
    {
        Score score = new Score();

        _command.Parameters.Clear();
        _command.CommandText =
            "SELECT id, playerId, gameId, time, score, " +
            "scoreRank FROM "
            + "(SELECT id, playerId, gameId, time, score, "
              + "DENSE_RANK() OVER (ORDER BY score DESC) scoreRank"
            + " FROM Scores WHERE playerId = @playerId AND gameId = @gameId)" +
            "WHERE id = @id;";
        _AddCommandParameter("playerId", playerId);
        _AddCommandParameter("gameId", gameId);
        _AddCommandParameter("id", id);

        _ExecuteReaderCommand();

        if(_reader.Read())
            score = _ReadScore();

        _CloseReader();

        return score.rank;
    }

    private void _ExecuteCommand()
    {
        _connection.Open();
        _command.ExecuteNonQuery();
        _connection.Close();
    }

    private void _ExecuteReaderCommand()
    {
        _connection.Open();
        _reader = _command.ExecuteReader();
    }

    private void _CloseReader()
    {
        _reader.Close();
        _connection.Close();
    }

    private void _AddCommandParameter(string name, object value)
    {
        _command.Parameters.Add(new SqliteParameter
        {
            ParameterName = name,
            Value = value
        });
    }

    private Score _ReadScore()
    {
        Score score = new Score();
        int fieldCount = 0;

        score.id = _reader.GetInt32(fieldCount++);
        score.playerId = _reader.GetInt32(fieldCount++);
        score.gameId = _reader.GetInt32(fieldCount++);
        score.time = _reader.GetString(fieldCount++);
        score.score = _reader.GetInt32(fieldCount++);

        if(fieldCount < _reader.FieldCount)
            score.rank = _reader.GetInt32(fieldCount);

        return score;
    }
}
