namespace table_tennis_backend.Dtos.Result;
public class GetReqDto
{
    public int Id { get; set; }
}

public class GetAllReqDto
{
    public int? Event_Id { get; set; }

    public int? Event_Type { get; set; }
    public DateTimeOffset? ResultDate { get; set; }
}
