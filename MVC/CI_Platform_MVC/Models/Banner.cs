using System;
using System.Collections.Generic;

namespace CI_Platform_MVC.Models;

public partial class Banner
{
    public long BannerId { get; set; }

    public string? Image { get; set; }

    public string? Text { get; set; }

    public int? SortOrder { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
