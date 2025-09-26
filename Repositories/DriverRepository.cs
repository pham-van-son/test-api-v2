using Dapper;
using Microsoft.Data.SqlClient;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Models;

namespace test_lab.Repositories
{
    /// <summary>
    /// Repository quản lý nghiệp vụ liên quan đến lái xe (HRM.Employees)
    /// </summary>
    public class DriverRepository : IDriverRepository
    {
        private readonly string _connectionString;

        public DriverRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Mở kết nối SQL Server.
        /// </summary>
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Lấy toàn bộ danh sách lái xe của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <returns>Danh sách lái xe</returns>
        public async Task<IEnumerable<HrmEmployee>> GetAllEmployees(int companyId)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = @"
                SELECT 
                    PK_EmployeeID     AS PkEmployeeId,
                    EmployeeCode,
                    FK_CompanyID      AS FkCompanyId,
                    FK_DepartmentID   AS FkDepartmentId,
                    Name,
                    DisplayName,
                    Birthday,
                    Sex,
                    Address,
                    Mobile,
                    PhoneNumber1,
                    PhoneNumber2,
                    EmployeeType,
                    IdentityNumber,
                    DriverLicense,
                    IssueLicenseDate,
                    IssueLicensePlace,
                    ExpireLicenseDate,
                    CreatedByUser,
                    CreatedDate,
                    UpdatedByUser,
                    UpdatedDate,
                    Flags,
                    IsSent,
                    LicenseType,
                    DriverImage,
                    IsLocked,
                    IsDeleted,
                    FK_UserID         AS FkUserId,
                    DriverAvatar,
                    LockDate
                FROM [HRM.Employees]
                WHERE FK_CompanyID = @CompanyId
                  AND IsDeleted = 0
                  AND IsLocked  = 0
                ORDER BY DisplayName ASC;
            ";

            return await conn.QueryAsync<HrmEmployee>(sql, new { CompanyId = companyId });
        }

        /// <summary>
        /// Lấy danh sách loại bằng lái xe (đang hoạt động, chưa xóa).
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm theo tên loại bằng</param>
        /// <returns>Danh sách loại bằng lái</returns>
        public async Task<IEnumerable<BcaLicenseTypeModel>> GetLicenseTypes(string? searchTerm)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = @"
                SELECT Pk_LicenseTypeID,
                       Name,
                       Code,
                       IsActived,
                       IsDeteted
                FROM [BCA.LicenseTypes]
                WHERE IsActived = 1
                  AND IsDeteted = 0
                  AND (@SearchTerm IS NULL OR Name LIKE '%' + @SearchTerm + '%')
                ORDER BY Name ASC;
            ";

            return await conn.QueryAsync<BcaLicenseTypeModel>(sql, new
            {
                SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm
            });
        }

        /// <summary>
        /// Lấy danh sách lái xe có phân trang, filter theo nhiều điều kiện động.
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm theo tên</param>
        /// <param name="driverLicense">Số GPLX</param>
        /// <param name="licenseTypes">Danh sách loại bằng lái</param>
        /// <param name="employeeIds">Danh sách employeeId</param>
        /// <returns>Danh sách lái xe phân trang</returns>
        public async Task<PaginationSet<HrmEmployee>> GetPagedList(
            int companyId,
            int page,
            int pageSize,
            string? searchTerm,
            string? driverLicense,
            IEnumerable<int>? licenseTypes,
            IEnumerable<int>? employeeIds
        )
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            // Điều kiện động
            var whereEmployeeIds = (employeeIds != null && employeeIds.Any())
                ? "AND PK_EmployeeID IN @EmployeeIds"
                : "";

            var whereLicenseTypes = (licenseTypes != null && licenseTypes.Any())
                ? "AND LicenseType IN @LicenseTypes"
                : "";

            var sql = $@"
                SELECT 
                    PK_EmployeeID     AS PkEmployeeId,
                    EmployeeCode,
                    FK_CompanyID      AS FkCompanyId,
                    FK_DepartmentID   AS FkDepartmentId,
                    Name,
                    DisplayName,
                    Birthday,
                    Sex,
                    Address,
                    Mobile,
                    PhoneNumber1,
                    PhoneNumber2,
                    EmployeeType,
                    IdentityNumber,
                    DriverLicense,
                    IssueLicenseDate,
                    IssueLicensePlace,
                    ExpireLicenseDate,
                    CreatedByUser,
                    CreatedDate,
                    UpdatedByUser,
                    UpdatedDate,
                    Flags,
                    IsSent,
                    LicenseType,
                    DriverImage,
                    IsLocked,
                    IsDeleted,
                    FK_UserID         AS FkUserId,
                    DriverAvatar,
                    LockDate
                FROM [HRM.Employees]
                WHERE FK_CompanyID = @CompanyId
                  AND IsDeleted = 0
                  AND IsLocked  = 0
                  AND (@SearchTerm IS NULL OR DisplayName COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm + '%')
                  AND (@DriverLicense IS NULL OR DriverLicense COLLATE Latin1_General_CI_AI LIKE '%' + @DriverLicense + '%')
                  {whereEmployeeIds}
                  {whereLicenseTypes}
                ORDER BY DisplayName ASC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*)
                FROM [HRM.Employees]
                WHERE FK_CompanyID = @CompanyId
                  AND IsDeleted = 0
                  AND IsLocked  = 0
                  AND (@SearchTerm IS NULL OR DisplayName COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm + '%')
                  AND (@DriverLicense IS NULL OR DriverLicense COLLATE Latin1_General_CI_AI LIKE '%' + @DriverLicense + '%')
                  {whereEmployeeIds}
                  {whereLicenseTypes};
            ";

            var parameters = new
            {
                CompanyId = companyId,
                SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm,
                DriverLicense = string.IsNullOrWhiteSpace(driverLicense) ? null : driverLicense,
                LicenseTypes = (licenseTypes != null && licenseTypes.Any()) ? licenseTypes.ToArray() : null,
                EmployeeIds = (employeeIds != null && employeeIds.Any()) ? employeeIds.ToArray() : null,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize > 0 ? pageSize : 20
            };

            using var multi = await conn.QueryMultipleAsync(sql, parameters);

            var items = (await multi.ReadAsync<HrmEmployee>()).ToList();
            var total = await multi.ReadSingleAsync<int>();

            return new PaginationSet<HrmEmployee>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        /// <summary>
        /// Cập nhật thông tin nhiều lái xe cùng lúc.
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <param name="employeeIds">Danh sách employeeId cần cập nhật</param>
        /// <param name="updateData">Dữ liệu cập nhật</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        public async Task<int> UpdateEmployees(int companyId, IEnumerable<int> employeeIds, HrmEmployeeModel updateData)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var setClauses = new List<string>
            {
                "DisplayName = @DisplayName",
                "Mobile = @Mobile",
                "DriverLicense = @DriverLicense",
                "IssueLicenseDate = @IssueLicenseDate",
                "ExpireLicenseDate = @ExpireLicenseDate",
                "IssueLicensePlace = @IssueLicensePlace",
                "LicenseType = @LicenseType",
                "UpdatedDate = GETDATE()"
            };

            var sql = $@"
                UPDATE [HRM.Employees]
                SET {string.Join(", ", setClauses)}
                WHERE FK_CompanyID = @CompanyId
                  AND PK_EmployeeID IN @EmployeeIds;
            ";

            return await conn.ExecuteAsync(sql, new
            {
                updateData.DisplayName,
                updateData.Mobile,
                updateData.DriverLicense,
                updateData.IssueLicenseDate,
                updateData.ExpireLicenseDate,
                updateData.IssueLicensePlace,
                updateData.LicenseType,
                CompanyId = companyId,
                EmployeeIds = employeeIds
            });
        }

        /// <summary>
        /// Xóa mềm (soft delete) một lái xe trong hệ thống.
        /// Đánh dấu trường IsDeleted = 1 và cập nhật UpdatedDate cho bản ghi.
        /// </summary>
        /// <param name="companyId">ID công ty chứa lái xe</param>
        /// <param name="employeeId">ID lái xe cần xóa</param>
        /// <returns>
        /// Số bản ghi bị ảnh hưởng (1 nếu xóa thành công, 0 nếu không tìm thấy).
        /// </returns>
        public async Task<int> DeleteEmployee(int companyId, int employeeId)
        {
            var sql = @"
                UPDATE [HRM.Employees]
                SET IsDeleted = 1,
                    UpdatedDate = GETDATE()
                WHERE FK_CompanyID = @CompanyId
                  AND PK_EmployeeID = @EmployeeId;
            ";

            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync(sql, new
            {
                CompanyId = companyId,
                EmployeeId = employeeId
            });
        }
    }
}
