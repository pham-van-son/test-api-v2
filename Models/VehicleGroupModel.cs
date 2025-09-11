namespace test_lab.Models
{
  public class VehicleGroupModel
  {
    public int FkCompanyId { get; set; }

    public int PkVehicleGroupId { get; set; }

    public int? ParentVehicleGroupId { get; set; }

    public string Name { get; set; } = null!;

    public Guid? CreatedByUser { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedByUser { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public double? DistanceA { get; set; }

    public double? DistanceB { get; set; }

    public int? MinuteA { get; set; }

    public int? MinuteB { get; set; }

    public int? FkBgtprovinceId { get; set; }

    public bool? IsDeleted { get; set; }

    public int Flag { get; set; }

    public bool? Status { get; set; }
    public int VehicleCount { get; set; }
  }

    public class AssignVehicleGroupRequest
    {
        public Guid UserId { get; set; }
        public List<int> VehicleGroupIds { get; set; }
    }
}
