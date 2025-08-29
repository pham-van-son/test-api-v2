using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Models;

namespace test_lab.Reposotpries
{
  public class VehicleRepository : IVehicleRepository
  {
    private readonly Gps3LabContext _context;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public VehicleRepository(Gps3LabContext context, IMapper mapper, HttpClient httpClient)
    {
      _context = context;
      _mapper = mapper;
      _httpClient = httpClient;
    }

    #region Images
    public async Task<PaginationSet<ImageResponseModel>> GetVehicleImagesAsync(ImageRequestModel model, int page, int pageSize)
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
          Items = new List<ImageResponseModel>(),
        };
      }

      var apiResponse = JsonSerializer.Deserialize<ImageApiResponse>(rawJson,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      var images = apiResponse?.Data ?? new List<ImageResponseModel>();

      if (model.SortOrder?.ToLower() == "asc")
      {
        images = images.OrderBy(v => v.CaptureTime).ToList();
      }
      else
      {
        images = images.OrderByDescending(v => v.CaptureTime).ToList();
      }

      var total = images.Count;
      var paged = images.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      return new PaginationSet<ImageResponseModel>
      {
        Page = page,
        PageSize = pageSize,
        TotalCount = total,
        Items = paged,
      };
    }
    #endregion

    #region VehicleGroup
    public async Task<VehicleGroupModel> vehicleGroupById(int groupId, int CompanyId)
    {
      var response = await _context.VehicleGroups
        .FirstOrDefaultAsync(v => v.PkVehicleGroupId == groupId && v.FkCompanyId == CompanyId && v.IsDeleted == false);
      return _mapper.Map<VehicleGroupModel>(response);
    }

    public async Task<List<VehicleGroupModel>> vehicleGroupList(int CompanyId)
    {
      var group = await _context.VehicleGroups
        .Where(v => v.FkCompanyId == CompanyId && v.IsDeleted == false)
        .ToListAsync();
      var groupIds = group.Select(v => v.PkVehicleGroupId).ToList();
      var counts = await _context.VehicleVehicleGroups
        .Where(v => v.FkCompanyId == CompanyId && v.IsDeleted == false)
        .GroupBy(v => v.FkVehicleGroupId)
        .Select(v => new { GroupId = v.Key, Count = v.Count() })
        .ToListAsync();
      var response = group.Select(g =>
      {
        var model = _mapper.Map<VehicleGroupModel>(g);
        model.VehicleCount = counts.FirstOrDefault(v => v.GroupId == g.PkVehicleGroupId)?.Count ?? 0;
        return model;
      }).ToList();
      return response;
    }
    #endregion

    #region VehicleVehicle
    public async Task<VehicleVehicleModel> vehicleVehicleById(int VehicleId, int CompanyId)
    {
      var response = await _context.VehicleVehicles
        .FirstOrDefaultAsync(v => v.PkVehicleId == VehicleId && v.FkCompanyId == CompanyId && v.IsDeleted == false);
      return _mapper.Map<VehicleVehicleModel>(response);
    }

    public async Task<List<VehicleVehicleModel>> vehicleVehicleList(int CompanyId, List<int> groupIds)
    {
            IQueryable<VehicleVehicle> query = _context.VehicleVehicles
                    .Where(x => x.FkCompanyId == CompanyId && x.IsDeleted == false && x.IsLocked == false);
            if (groupIds != null && groupIds.Any())
            {
                var vehicleIds = await _context.VehicleVehicleGroups
                    .Where(x => x.FkCompanyId == CompanyId && x.IsDeleted == false && groupIds.Contains(x.FkVehicleGroupId))
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
    public async Task<List<VehicleVehicleGroupModel>> vehicleVehicleGroupList(int CompanyId)
    {
      var response = await _context.VehicleVehicleGroups
        .Where(v => v.FkCompanyId == CompanyId && v.IsDeleted == false)
        .ToListAsync();
      return _mapper.Map<List<VehicleVehicleGroupModel>>(response); 
    }
    #endregion
  }
}
