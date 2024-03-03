using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[Table("PlayerScoreHistory")]
public partial class PlayerScoreHistory
{
    [Key]
    public int Id { get; set; }

    public int Result_Id { get; set; }

    public int Player_Id_A { get; set; }

    public int Player_Id_B { get; set; }

    public int InitialScore_A { get; set; }

    public int InitialScore_B { get; set; }
}
