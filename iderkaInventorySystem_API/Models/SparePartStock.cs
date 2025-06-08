using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class SparePartStock
{
    public string IdSpare { get; set; } = null!;

    public string IdLoc { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual StorageLocation IdLocNavigation { get; set; } = null!;

    public virtual SparePart IdSpareNavigation { get; set; } = null!;
}
