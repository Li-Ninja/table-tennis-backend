using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using table_tennis_backend.Const;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[Table("DoublePlayer")]
public partial class DoublePlayer
{
    [Key]
    public int Id { get; set; }
    [StringLength(50)]
    public string TeamName { get; set; } = "";
    public int Player_Id_1 { get; set; }
    public int Player_Id_2 { get; set; }
    public int Player_Id_1_Score { get; set; }
    public int Player_Id_2_Score { get; set; }
    public int Score { get; set; }
    public DateTimeOffset? TeamAssemblyDateTime { get; set; }
    public DateTimeOffset? UpdateDateTime { get; set; }
    public DateTimeOffset? LatestResultDateTime { get; set; }
    public DoublePlayerStatusEnum Status { get; set; }
}
