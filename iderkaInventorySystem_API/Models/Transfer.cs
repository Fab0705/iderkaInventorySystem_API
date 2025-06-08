using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class Transfer
{
    public string IdTransf { get; set; } = null!;

    public string OriginId { get; set; } = null!;

    public string DestinyId { get; set; } = null!;

    public DateTime DateTransf { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public string StatusTransf { get; set; } = null!;

    public virtual StorageLocation Destiny { get; set; } = null!;

    public virtual ICollection<DetailTransfer> DetailTransfers { get; set; } = new List<DetailTransfer>();

    public virtual StorageLocation Origin { get; set; } = null!;
}
