using table_tennis_backend.Const;

namespace table_tennis_backend.Dtos.Player;
public class GetComparisonResDto
{
    public PlayerComparison? PlayerA { get; set; }
    public PlayerComparison? PlayerB { get; set; }

}

public class PlayerComparison
{
    public PlayerStats? All { get; set; }
    public PlayerStats? Recent { get; set; }
}

public class PlayerStats
{
    public double WinRate { get; set; }
    public double GameWinRate { get; set; }
    public double AverageScore { get; set; }
    public double CriticalGameWinRate { get; set; }
    public double LongestWinStreak { get; set; }
    public double CriticalPointRate { get; set; }
}
