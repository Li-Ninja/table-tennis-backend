namespace table_tennis_backend.Dtos.ResultItem;
public class AddReqDto
{
    public required int Result_Id { get; set; }
    public required int MatchIndex { get; set; }
    public required int ScoreA { get; set; }
    public required int ScoreB { get; set; }

    public required int? Player_Id_A_1 { get; set; }
    public required int? Player_Id_A_2 { get; set; }
    public required int? Player_Id_B_1 { get; set; }
    public required int? Player_Id_B_2 { get; set; }
}
