using table_tennis_backend.Const;

namespace table_tennis_backend.Dtos.DoublePlayer;
public class GetAllResDto
{
    public int Id { get; set; }
    public string TeamName { get; set; } = "";
    public int Player_Id_1 { get; set; }
    public int Player_Id_2 { get; set; }
    public string Player_Name_1 { get; set; } = "";
    public string Player_Name_2 { get; set; } = "";
    public int Score { get; set; }
    public int? Rank { get; set; }
    public int ResultCount { get; set; }
    public int ResultCountByYearly { get; set; }
    public int WinningCount { get; set; }
    public DateTimeOffset? UpdateDateTime { get; set; }
    public DateTimeOffset? LatestResultDateTime { get; set; }
    public DoublePlayerStatusEnum Status { get; set; }
}
