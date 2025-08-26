using test_lab.Models;

namespace test_lab.IRepositories
{
  public interface IVehicleRepository
  {
    //VehicleGroup
    Task<List<VehicleGroupModel>> vehicleGroupList(int CompanyId);
    Task<VehicleGroupModel> vehicleGroupById(int groupId, int CompanyId);

    //Vehicle
    Task<List<VehicleVehicleModel>> vehicleVehicleList(int CompanyId);
    Task<VehicleVehicleModel> vehicleVehicleById(int VehicleId, int CompanyId);

    //VehicleVehicleGroup
    Task<List<VehicleVehicleGroupModel>> vehicleVehicleGroupList(int CompanyId);

    //Images
    Task<PaginationSet<ImageResponseModel>> GetVehicleImagesAsync(ImageRequestModel model, int page, int pageSize);
  }
}
