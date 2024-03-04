using ResultItemDto = table_tennis_backend.Dtos.ResultItem.GetResDto;

namespace table_tennis_backend.Dtos.Result;
public class GetResDto
{
    public int Id { get; set; }
    public int Event_Id { get; set; }
    public string Event_Name { get; set; } = string.Empty;
    public Boolean IsSingleMatch { get; set; }
    public int Round { get; set; }
    public int RoundIndex { get; set; }
    public string? Player_NameA1 { get; set; }
    public string? Player_NameA2 { get; set; }
    public string? Player_NameB1 { get; set; }
    public string? Player_NameB2 { get; set; }
    public int? Player_Id_A_1 { get; set; }
    public int? Player_Id_A_2 { get; set; }
    public int? Player_Id_B_1 { get; set; }
    public int? Player_Id_B_2 { get; set; }
    public int? ScoreA { get; set; }
    public int? ScoreB { get; set; }
    public string? ResultDate { get; set; }
}


public class GetRankingResDto
{
    public int Id { get; set; }
    public int Event_Id { get; set; }
    public string Event_Name { get; set; } = string.Empty;
    public string? Player_NameA1 { get; set; }
    public string? Player_NameB1 { get; set; }
    public int? Player_Id_A_1 { get; set; }
    public int? Player_Id_B_1 { get; set; }
    public int? ScoreA { get; set; }
    public int? ScoreB { get; set; }

    public string? ResultDate { get; set; }
    public List<ResultItemDto> ResultItemList { get; set; } = new List<ResultItemDto>();

}
