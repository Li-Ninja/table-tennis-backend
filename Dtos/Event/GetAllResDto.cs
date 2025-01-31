using table_tennis_backend.Const;

namespace table_tennis_backend.Dtos.Event;
public class GetAllResDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public EventTypeEnum Type { get; set; }
}
