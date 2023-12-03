namespace table_tennis_backend.Dtos.Result;
public class GetAllResDto
{
    public int Id { get; set; }
    public int Event_Id { get; set; }
    public string Event_Name { get; set; } = string.Empty;
    public int Round { get; set; }
    public string? Player_NameA1 { get; set; }
    public string? Player_NameA2 { get; set; }
    public string? Player_NameB1 { get; set; }
    public string? Player_NameB2 { get; set; }
}
