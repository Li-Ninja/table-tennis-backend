using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[PrimaryKey("Result_Id", "MatchIndex")]
[Table("ResultItem")]
public partial class ResultItem
{
    [Key]
    public int Result_Id { get; set; }

    [Key]
    public int MatchIndex { get; set; }

    public int ScoreA { get; set; }

    public int ScoreB { get; set; }
}
