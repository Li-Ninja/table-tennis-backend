using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Const;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[Table("Player")]
public partial class Player
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;
    public int Score { get; set; }
    public Boolean IsMan { get; set; }
    public Boolean IsRightHand { get; set; }
    public RacketTypeEnum RacketType { get; set; }
    public RubberTypeEnum ForehandRubberType { get; set; }
    public RubberTypeEnum BackhandRubberType { get; set; }
    public DateTimeOffset? UpdateDateTime { get; set; }
    public DateTimeOffset? LatestResultDateTime { get; set; }
    public Boolean IsOnLeave { get; set; }
}
