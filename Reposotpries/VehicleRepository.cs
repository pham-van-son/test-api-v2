using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TestLab.Entities;
using TestLab.IRepositories;
using TestLab.Models;

namespace TestLab.Repositories
{
    /// <summary>
    /// Repository xử lý dữ liệu liên quan đến Phương tiện (Vehicle), Nhóm phương tiện (VehicleGroup),
    /// mối quan hệ giữa chúng và hình ảnh phương tiện.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly Gps3LabContext _context;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Khởi tạo VehicleRepository với DbContext, AutoMapper và HttpClient.
        /// </summary>
        public VehicleRepository(Gps3LabContext context, IMapper mapper, HttpClient httpClient)
        {
            _context = context;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        #region Images

        /// <summary>
        /// Lấy danh sách hình ảnh phương tiện theo yêu cầu, có phân trang.
        /// </summary>
        /// <param name="model">Model yêu cầu hình ảnh.</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="pageSize">Số bản ghi mỗi trang.</param>
        /// <returns>Kết quả phân trang hình ảnh phương tiện.</returns>
        public async Task<PaginationSet<ImageResponseModel>> GetVehicleImagesAsync(
            ImageRequestModel model, int page, int pageSize)
        {
            var url = "http://10.0.10.27:15000/api/v2/images/ImagesFrequency";

            var response = await _httpClient.PostAsJsonAsync(url, model);
            var rawJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new PaginationSet<ImageResponseModel>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0,
                    Items = new List<ImageResponseModel>()
                };
            }

            var apiResponse = JsonSerializer.Deserialize<ImageApiResponse>(
                rawJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var images = apiResponse?.Data ?? new List<ImageResponseModel>();

            images = model.SortOrder?.ToLower() == "asc"
                ? images.OrderBy(v => v.CaptureTime).ToList()
                : images.OrderByDescending(v => v.CaptureTime).ToList();

            var total = images.Count;
            var paged = images.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationSet<ImageResponseModel>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = paged
            };
        }

        #endregion

        #region VehicleGroup

        /// <summary>
        /// Lấy thông tin nhóm phương tiện theo ID.
        /// </summary>
        public async Task<VehicleGroupModel> GetVehicleGroupByIdAsync(int groupId, int companyId)
        {
            var response = await _context.VehicleGroups
                .FirstOrDefaultAsync(v =>
                    v.PkVehicleGroupId == groupId &&
                    v.FkCompanyId == companyId &&
                    v.IsDeleted == false);

            return _mapper.Map<VehicleGroupModel>(response);
        }

        /// <summary>
        /// Lấy danh sách nhóm phương tiện theo công ty.
        /// </summary>
        public async Task<List<VehicleGroupModel>> GetVehicleGroupsAsync(int companyId)
        {
            var groups = await _context.VehicleGroups
                .Where(v => v.FkCompanyId == companyId && v.IsDeleted == false)
                .ToListAsync();

            var counts = await _context.VehicleVehicleGroups
                .Where(v => v.FkCompanyId == companyId && v.IsDeleted == false)
                .GroupBy(v => v.FkVehicleGroupId)
                .Select(v => new { GroupId = v.Key, Count = v.Count() })
                .ToListAsync();

            var response = groups.Select(g =>
            {
                var model = _mapper.Map<VehicleGroupModel>(g);
                model.VehicleCount = counts.FirstOrDefault(v => v.GroupId == g.PkVehicleGroupId)?.Count ?? 0;
                return model;
            }).ToList();

            return response;
        }

        #endregion

        #region Vehicle

        /// <summary>
        /// Lấy thông tin phương tiện theo ID.
        /// </summary>
        public async Task<VehicleVehicleModel> GetVehicleByIdAsync(int vehicleId, int companyId)
        {
            var response = await _context.VehicleVehicles
                .FirstOrDefaultAsync(v =>
                    v.PkVehicleId == vehicleId &&
                    v.FkCompanyId == companyId &&
                    v.IsDeleted == false);

            return _mapper.Map<VehicleVehicleModel>(response);
        }

        /// <summary>
        /// Lấy danh sách phương tiện theo công ty và nhóm.
        /// </summary>
        public async Task<List<VehicleVehicleModel>> GetVehiclesAsync(int companyId, List<int> groupIds)
        {
            IQueryable<VehicleVehicle> query = _context.VehicleVehicles
                .Where(x => x.FkCompanyId == companyId && x.IsDeleted == false && x.IsLocked == false);

            if (groupIds?.Any() == true)
            {
                var vehicleIds = await _context.VehicleVehicleGroups
                    .Where(x =>
                        x.FkCompanyId == companyId &&
                        x.IsDeleted == false &&
                        groupIds.Contains(x.FkVehicleGroupId))
                    .Select(x => x.FkVehicleId)
                    .Distinct()
                    .ToListAsync();

                query = query.Where(x => vehicleIds.Contains((int)x.PkVehicleId));
            }

            var response = await query.ToListAsync();
            return _mapper.Map<List<VehicleVehicleModel>>(response);
        }

        #endregion

        #region VehicleVehicleGroup

        /// <summary>
        /// Lấy danh sách mối quan hệ Vehicle - VehicleGroup theo công ty.
        /// </summary>
        public async Task<List<VehicleVehicleGroupModel>> GetVehicleVehicleGroupsAsync(int companyId)
        {
            var response = await _context.VehicleVehicleGroups
                .Where(v => v.FkCompanyId == companyId && v.IsDeleted == false)
                .ToListAsync();

            return _mapper.Map<List<VehicleVehicleGroupModel>>(response);
        }

        #endregion
    }
}
