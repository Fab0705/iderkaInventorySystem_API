using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class Region
{
    public string IdReg { get; set; } = null!;

    public string DescReg { get; set; } = null!;

    public virtual ICollection<StorageLocation> StorageLocations { get; set; } = new List<StorageLocation>();
}
