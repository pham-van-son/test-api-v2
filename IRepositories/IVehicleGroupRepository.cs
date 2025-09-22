using test_lab.Models;

namespace test_lab.IRepositories
{
    public interface IVehicleGroupRepository
    {
        Task<List<AdminUserModel>> ListUser(int companyId, string? searchTerm);
        Task<List<VehicleGroupModel>> ListAvailableVehicleGroups(int companyId, Guid userId, string? searchTerm);
        Task<List<VehicleGroupModel>> ListAssignedVehicleGroups(int companyId, Guid userId, string? searchTerm);
        Task AssignVehicleGroups(Guid userId, List<int> vehicleGroupIds);
        Task UnassignVehicleGroups(Guid userId, List<int> vehicleGroupIds);
    }
}
