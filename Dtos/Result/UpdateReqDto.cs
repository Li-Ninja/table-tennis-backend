namespace table_tennis_backend.Dtos.Result;
public class UpdateReqDto
{
    public required int Id { get; set; }
    public int? Player_Id_A_1 { get; set; }
    public int? Player_Id_A_2 { get; set; }
    public int? Player_Id_B_1 { get; set; }
    public int? Player_Id_B_2 { get; set; }
    public int? ScoreA { get; set; }
    public int? ScoreB { get; set; }
}
