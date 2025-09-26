using test_lab.Entities;
using test_lab.Models;

namespace test_lab.IRepositories
{
    /// <summary>
    /// Interface repository cho các nghiệp vụ quản lý lái xe.
    /// </summary>
    public interface IDriverRepository
    {
        /// <summary>
        /// Cập nhật thông tin cho nhiều lái xe cùng lúc.
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <param name="employeeIds">Danh sách employeeId cần cập nhật</param>
        /// <param name="updateData">Dữ liệu cập nhật</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        Task<int> UpdateEmployees(int companyId, IEnumerable<int> employeeIds, HrmEmployeeModel updateData);

        /// <summary>
        /// Lấy danh sách lái xe có phân trang, filter động theo nhiều điều kiện.
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm theo tên</param>
        /// <param name="driverLicense">Số GPLX</param>
        /// <param name="licenseTypes">Danh sách loại bằng lái</param>
        /// <param name="employeeIds">Danh sách employeeId</param>
        /// <returns>Danh sách lái xe phân trang</returns>
        Task<PaginationSet<HrmEmployee>> GetPagedList(
            int companyId,
            int page,
            int pageSize,
            string? searchTerm,
            string? driverLicense,
            IEnumerable<int>? licenseTypes,
            IEnumerable<int>? employeeIds);

        /// <summary>
        /// Lấy danh sách loại bằng lái xe (đang hoạt động, chưa xóa).
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm theo tên loại bằng</param>
        /// <returns>Danh sách loại bằng lái</returns>
        Task<IEnumerable<BcaLicenseTypeModel>> GetLicenseTypes(string? searchTerm);

        /// <summary>
        /// Lấy toàn bộ danh sách lái xe của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <returns>Danh sách lái xe</returns>
        Task<IEnumerable<HrmEmployee>> GetAllEmployees(int companyId);

        /// <summary>
        /// Xóa mềm (soft delete) một lái xe trong hệ thống.
        /// </summary>
        /// <param name="companyId">ID công ty chứa lái xe</param>
        /// <param name="employeeId">ID lái xe cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng (1 nếu xóa thành công, 0 nếu không tìm thấy)</returns>
        Task<int> DeleteEmployee(int companyId, int employeeId);
    }
}
