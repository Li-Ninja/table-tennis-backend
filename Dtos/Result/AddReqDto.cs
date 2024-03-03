namespace table_tennis_backend.Dtos.Result;

public class AddReqDto
{
    public required int Event_Id { get; set; }
    public required int Round { get; set; }

}

public class ResultItemList
{
    public required int MatchIndex { get; set; }
    public required int ScoreA { get; set; }
    public required int ScoreB { get; set; }
}

public class AddRankingReqDto
{
    public required int Event_Id { get; set; }
    public int Player_Id_A_1 { get; set; }
    public int Player_Id_B_1 { get; set; }
    public DateTime ResultDate { get; set; }
    public ResultItemList[] ResultItemList { get; set; } = [];
}
