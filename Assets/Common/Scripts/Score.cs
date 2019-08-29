public struct Score
{
    public int id;
    public int playerId;
    public int gameId;
    public string time;
    public int score;
    public int rank;

    public Score(int playerId, int gameId, string time, int score)
    {
        id = -1;
        this.playerId = playerId;
        this.gameId = gameId;
        this.time = time;
        this.score = score;
        rank = 0;
    }

    public static bool operator ==(Score score, Score other)
    {
        if(score.id == other.id)
            return true;

        return score.playerId == other.playerId && score.time == other.time;
    }

    public static bool operator !=(Score score, Score other)
    {
        return !(score == other);
    }

    override public string ToString()
    {
        string s = "[" + id + "] " + "player{" + playerId + "}, minigame{" + gameId + "}, time{" + time + "}, score{" + score + "}";

        if(rank > 0)
            s += ", rank{" + rank + "}";

        return s;
    }
}
