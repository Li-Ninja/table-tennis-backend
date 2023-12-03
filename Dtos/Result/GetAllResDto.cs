namespace table_tennis_backend.Dtos.Result;
public class GetAllResDto
{
    public int Id { get; set; }
    public int Event_id { get; set; }
    public string Event_name { get; set; } = string.Empty;
    public int Round { get; set; }
    public int? Player_Id_A_1 { get; set; }
    public int? Player_Id_A_2 { get; set; }
    public int? Player_Id_B_1 { get; set; }
    public int? Player_Id_B_2 { get; set; }
}
