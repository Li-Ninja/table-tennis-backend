using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Const;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

[Table("Event")]
public partial class Event
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime Date { get; set; }

    public EventTypeEnum Type { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<Result> Result { get; set; } = new List<Result>();
}
