using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class SparePart
{
    public string IdSpare { get; set; } = null!;

    public string NumberPart { get; set; } = null!;

    public string DescPart { get; set; } = null!;

    public bool Rework { get; set; }

    public virtual ICollection<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();

    public virtual ICollection<DetailTransfer> DetailTransfers { get; set; } = new List<DetailTransfer>();

    public virtual ICollection<SparePartStock> SparePartStocks { get; set; } = new List<SparePartStock>();
}
