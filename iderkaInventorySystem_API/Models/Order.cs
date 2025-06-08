using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class Order
{
    public string IdOrd { get; set; } = null!;

    public string IdLoc { get; set; } = null!;

    public string WorkOrd { get; set; } = null!;

    public string DescOrd { get; set; } = null!;

    public DateTime DateOrd { get; set; }

    public string StatusOrd { get; set; } = null!;

    public virtual ICollection<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();

    public virtual StorageLocation IdLocNavigation { get; set; } = null!;
}
