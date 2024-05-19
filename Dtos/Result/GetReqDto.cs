namespace table_tennis_backend.Dtos.Result;
public class GetReqDto
{
    public int Id { get; set; }
}

public class GetAllReqDto
{
    public int? Event_Id { get; set; }

    public int? Event_Type { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public int? Player_Id_A_1 { get; set; }
    public int? Player_Id_B_1 { get; set; }
}
