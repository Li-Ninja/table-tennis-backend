using table_tennis_backend.Const;

namespace table_tennis_backend.Dtos.Player;
public class GetComparisonReqDto
{
    public int IdA { get; set; }
    public int IdB { get; set; }
    public PlayerComparisonTypeEnum Type { get; set; }
}
