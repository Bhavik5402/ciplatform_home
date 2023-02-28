using System;
using System.Collections.Generic;

namespace CI_Platform_MVC.Entity.Models;

public partial class StoryInvite
{
    public long StoryInviteId { get; set; }

    public long StoryId { get; set; }

    public long FromUserId { get; set; }

    public long ToUserId { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Story Story { get; set; } = null!;
}
