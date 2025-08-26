namespace test_lab.Models
{
  public class VehicleVehicleGroupModel
  {
    public int FkCompanyId { get; set; }

    public int FkVehicleGroupId { get; set; }

    public int FkVehicleId { get; set; }

    public bool? IsDeleted { get; set; }
  }
}
