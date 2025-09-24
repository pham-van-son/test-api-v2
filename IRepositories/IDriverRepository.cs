using test_lab.Entities;
using test_lab.Models;

namespace test_lab.IRepositories
{
    public interface IDriverRepository
    {
        Task<int> UpdateEmployees(int companyId, IEnumerable<int> employeeIds, HrmEmployee model);
        Task<PaginationSet<HrmEmployee>> GetPagedList(int companyId, int page, int pageSize, string? searchTerm, string? driverLicense);
        Task<IEnumerable<BcaLicenseTypeModel>> GetLicenseTypes(string? searchTerm);
        Task<IEnumerable<HrmEmployee>> GetAllEmployees(int companyId);
        Task<int> DeleteEmployee(int companyId, int employeeId);
    }
}
