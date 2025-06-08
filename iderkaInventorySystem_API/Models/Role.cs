using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class Role
{
    public string IdRol { get; set; } = null!;

    public string RolName { get; set; } = null!;

    public virtual ICollection<User> IdUsrs { get; set; } = new List<User>();
}
