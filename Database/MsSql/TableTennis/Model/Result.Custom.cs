namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

public partial class Result
{
    public virtual Player? PlayerA1 { get; set; }
    public virtual Player? PlayerA2 { get; set; }
    public virtual Player? PlayerB1 { get; set; }
    public virtual Player? PlayerB2 { get; set; }
    public virtual DoublePlayer? DoublePlayerA { get; set; }
    public virtual DoublePlayer? DoublePlayerB { get; set; }
}
