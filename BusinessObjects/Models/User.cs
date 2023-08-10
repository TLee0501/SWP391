using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid RoleId { get; set; }

    public bool IsBan { get; set; }
}
