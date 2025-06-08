using System;
using System.Collections.Generic;

namespace iderkaInventorySystem_API.Models;

public partial class StorageLocation
{
    public string IdLoc { get; set; } = null!;

    public string NameSt { get; set; } = null!;

    public string DescStLoc { get; set; } = null!;

    public string IdReg { get; set; } = null!;

    public virtual Region IdRegNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<SparePartStock> SparePartStocks { get; set; } = new List<SparePartStock>();

    public virtual ICollection<Transfer> TransferDestinies { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferOrigins { get; set; } = new List<Transfer>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
