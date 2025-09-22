using System;
using System.Collections.Generic;

namespace TestLab.Entities;

public partial class VehicleVehicleGroup
{
    public int FkCompanyId { get; set; }

    public int FkVehicleGroupId { get; set; }

    public int FkVehicleId { get; set; }

    public bool? IsDeleted { get; set; }
}
