using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[PrimaryKey("Result_Id", "Event_Id", "Result_Round", "Id")]
[Table("ResultItem")]
public partial class ResultItem
{
    [Key]
    public int Result_Id { get; set; }

    [Key]
    public int Event_Id { get; set; }

    [Key]
    public int Result_Round { get; set; }

    [Key]
    public int Id { get; set; }

    public int Score_A { get; set; }

    public int Score_B { get; set; }
}
