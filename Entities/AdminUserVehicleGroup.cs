using System;
using System.Collections.Generic;

namespace test_lab.Entities;

public partial class AdminUserVehicleGroup
{
    public Guid FkUserId { get; set; }

    public int FkVehicleGroupId { get; set; }

    public int? ParentVehicleGroupId { get; set; }

    public Guid? CreatedByUser { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdateByUser { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedByUser { get; set; }

    public bool? IsDeleted { get; set; }
}
