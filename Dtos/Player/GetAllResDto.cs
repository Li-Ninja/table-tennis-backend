namespace table_tennis_backend.Dtos.Player;
public class GetAllResDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Score { get; set; }
    public int Rank { get; set; }
    public int ResultCount { get; set; }
    public int WinningCount { get; set; }
    public DateTimeOffset? UpdateDateTime { get; set; }
    public DateTimeOffset? LatestResultDateTime { get; set; }
}
