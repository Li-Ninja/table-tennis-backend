using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[Table("Result")]
public partial class Result
{
    [Key]
    public int Id { get; set; }

    public int Event_Id { get; set; }

    public int Round { get; set; }

    public int RoundIndex { get; set; }

    public int? Player_Id_A_1 { get; set; }

    public int? Player_Id_A_2 { get; set; }

    public int? Player_Id_B_1 { get; set; }

    public int? Player_Id_B_2 { get; set; }

    public int? ScoreA { get; set; }

    public int? ScoreB { get; set; }

    public DateTimeOffset ResultDateTime { get; set; }
    public int PlayerScoreA { get; set; }
    public int PlayerScoreB { get; set; }

    [ForeignKey("Event_Id")]
    [InverseProperty("Result")]
    public virtual Event Event { get; set; } = null!;
}
