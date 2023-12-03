using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[PrimaryKey("Id", "Round", "Event_Id")]
[Table("Result")]
public partial class Result
{
    [Key]
    public int Id { get; set; }

    [Key]
    public int Event_Id { get; set; }

    [Key]
    public int Round { get; set; }

    public int? Player_Id_A_1 { get; set; }

    public int? Player_Id_A_2 { get; set; }

    public int? Player_Id_B_1 { get; set; }

    public int? Player_Id_B_2 { get; set; }
}
