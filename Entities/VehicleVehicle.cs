using System;
using System.Collections.Generic;

namespace TestLab.Entities;

public partial class VehicleVehicle
{
    public int FkCompanyId { get; set; }

    public long PkVehicleId { get; set; }

    public string VehiclePlate { get; set; } = null!;

    public string PrivateCode { get; set; } = null!;

    public string? Imei { get; set; }

    public bool? IsLocked { get; set; }

    public bool? IsDeleted { get; set; }

    public int Xncode { get; set; }

    public bool IsCam { get; set; }

    public bool? IsVideoCam { get; set; }
}
