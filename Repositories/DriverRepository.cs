using Dapper;
using Microsoft.Data.SqlClient;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Models;

namespace test_lab.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly string _connectionString;

        public DriverRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Helper mở connection mỗi lần cần
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<HrmEmployee>> GetAllEmployees(int companyId)
        {
                using var conn = GetConnection();
                await conn.OpenAsync();

                var sql = @"
                SELECT PK_EmployeeID AS PkEmployeeId,
                       DisplayName,
                       Mobile,
                       DriverLicense
                FROM [HRM.Employees]
                WHERE FK_CompanyID = @CompanyId
                  AND IsDeleted = 0
                  AND IsLocked = 0
                ORDER BY DisplayName ASC;
            ";

                return await conn.QueryAsync<HrmEmployee>(sql, new { CompanyId = companyId });
        }

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

        public async Task<PaginationSet<HrmEmployee>> GetPagedList(int companyId, int page, int pageSize, string? searchTerm, string? driverLicense)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = @"
                SELECT PK_EmployeeID AS PkEmployeeId,
                       DisplayName,
                       Mobile,
                       DriverLicense
                FROM [HRM.Employees]
                WHERE FK_CompanyID = @CompanyId
                  AND IsDeleted = 0
                  AND IsLocked = 0
                  AND (@SearchTerm IS NULL OR DisplayName LIKE '%' + @SearchTerm + '%')
                  AND (@DriverLicense IS NULL OR DriverLicense LIKE '%' + @DriverLicense + '%')
                ORDER BY DisplayName ASC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*)
                FROM [HRM.Employees]
                WHERE FK_CompanyID = @CompanyId
                  AND IsDeleted = 0
                  AND IsLocked = 0
                  AND (@SearchTerm IS NULL OR DisplayName LIKE '%' + @SearchTerm + '%')
                  AND (@DriverLicense IS NULL OR DriverLicense LIKE '%' + @DriverLicense + '%');
            ";

            var parameters = new
            {
                CompanyId = companyId,
                SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm,
                DriverLicense = string.IsNullOrWhiteSpace(driverLicense) ? null : driverLicense,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
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

        public async Task<int> UpdateEmployees(int companyId, IEnumerable<int> employeeIds, HrmEmployee model)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();

            var sql = @"
                UPDATE [HRM.Employees]
                SET DisplayName = @DisplayName,
                    Mobile = @Mobile,
                    DriverLicense = @DriverLicense,
                    IssueLicenseDate = @IssueLicenseDate,
                    ExpireLicenseDate = @ExpireLicenseDate,
                    IssueLicensePlace = @IssueLicensePlace,
                    LicenseType = @LicenseType,
                    UpdatedDate = GETDATE()
                WHERE FK_CompanyID = @CompanyId
                  AND PK_EmployeeID IN @EmployeeIds;
            ";

            return await conn.ExecuteAsync(sql, new
            {
                model.DisplayName,
                model.Mobile,
                model.DriverLicense,
                model.IssueLicenseDate,
                model.ExpireLicenseDate,
                model.IssueLicensePlace,
                model.LicenseType,
                CompanyId = companyId,
                EmployeeIds = employeeIds
            });
        }

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
