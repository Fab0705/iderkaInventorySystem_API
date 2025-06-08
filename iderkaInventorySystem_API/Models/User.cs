using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class User
{
    public string IdUsr { get; set; } = null!;

    public string Usr { get; set; } = null!;

    public string Pwd { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string IdLoc { get; set; } = null!;

    public virtual StorageLocation IdLocNavigation { get; set; } = null!;

    public virtual ICollection<Role> IdRols { get; set; } = new List<Role>();
}
