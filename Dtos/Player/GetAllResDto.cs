using table_tennis_backend.Const;

namespace table_tennis_backend.Dtos.Player;
public class GetAllResDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Score { get; set; }
    public Boolean IsMan { get; set; }
    public Boolean IsRightHand { get; set; }
    public RacketTypeEnum RacketType { get; set; }
    public RubberTypeEnum ForehandRubberType { get; set; }
    public RubberTypeEnum BackhandRubberType { get; set; }
    public int Rank { get; set; }
    public int ResultCount { get; set; }
    public int WinningCount { get; set; }
    public DateTimeOffset? UpdateDateTime { get; set; }
    public DateTimeOffset? LatestResultDateTime { get; set; }
}
