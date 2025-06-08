using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class DetailTransfer
{
    public string IdDetTransf { get; set; } = null!;

    public string IdTransf { get; set; } = null!;

    public string IdSpare { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual SparePart IdSpareNavigation { get; set; } = null!;

    public virtual Transfer IdTransfNavigation { get; set; } = null!;
}
