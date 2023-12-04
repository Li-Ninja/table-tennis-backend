namespace table_tennis_backend.Dtos.ResultItem;
public class GetResDto
{
    public required int Result_Id { get; set; }
    public required int MatchIndex { get; set; }
    public required int ScoreA { get; set; }
    public required int ScoreB { get; set; }
}
