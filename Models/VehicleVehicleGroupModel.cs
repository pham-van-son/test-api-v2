namespace TestLab.Models
{
    /// <summary>
    /// Model liên k?t gi?a Ph??ng ti?n (Vehicle) và Nhóm ph??ng ti?n (VehicleGroup).
    /// </summary>
    public class VehicleVehicleGroupModel
  {
    public int FkCompanyId { get; set; }

    public int FkVehicleGroupId { get; set; }

    public int FkVehicleId { get; set; }

    public bool? IsDeleted { get; set; }
  }
}
