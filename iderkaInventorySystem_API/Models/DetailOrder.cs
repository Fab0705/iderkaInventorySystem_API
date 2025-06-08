using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class DetailOrder
{
    public string IdDetOrd { get; set; } = null!;

    public string IdOrd { get; set; } = null!;

    public string IdSpare { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Order IdOrdNavigation { get; set; } = null!;

    public virtual SparePart IdSpareNavigation { get; set; } = null!;
}
