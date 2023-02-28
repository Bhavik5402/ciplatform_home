using System;
using System.Collections.Generic;

namespace CI_Platform_MVC.Entity.Models;

public partial class MissionTheme
{
    public long MissionThemeId { get; set; }

    public string? Title { get; set; }

    public bool? Status { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();
}
