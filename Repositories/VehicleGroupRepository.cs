using AutoMapper;
using Microsoft.EntityFrameworkCore;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Models;

namespace test_lab.Repositories
{
    public class VehicleGroupRepository : IVehicleGroupRepository
    {
        private readonly Gps3LabContext _context;
        private readonly IMapper _mapper;

        public VehicleGroupRepository(Gps3LabContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AssignVehicleGroups(Guid userId, List<int> vehicleGroupIds)
        {
            foreach (var groupId in vehicleGroupIds)
            {
                var existing = await _context.AdminUserVehicleGroups
                    .FirstOrDefaultAsync(uvg => uvg.FkUserId == userId && uvg.FkVehicleGroupId == groupId);
                if (existing != null)
                {
                    existing.IsDeleted = false;
                    existing.UpdatedDate = DateTime.UtcNow;
                    existing.UpdateByUser = userId;
                }
                else
                {
                    _context.AdminUserVehicleGroups.Add(new AdminUserVehicleGroup
                    {
                        FkUserId = userId,
                        FkVehicleGroupId = groupId,
                        IsDeleted = false,
                        CreatedDate = DateTime.UtcNow,
                        CreatedByUser = userId,
                    });
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleGroupModel>> ListAssignedVehicleGroups(int companyId, Guid userId, string? searchTerm)
        {
            var query = from uvg in _context.AdminUserVehicleGroups
                        join vg in _context.VehicleGroups on uvg.FkVehicleGroupId equals vg.PkVehicleGroupId
                        where uvg.FkUserId == userId && (uvg.IsDeleted == null || uvg.IsDeleted == false) && vg.FkCompanyId == companyId && (vg.IsDeleted == null || vg.IsDeleted == false)
                        select vg;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var groups = await query.OrderBy(x => x.Name)
                .ToListAsync();
            var groupModel = _mapper.Map<List<VehicleGroupModel>>(groups);
            return groupModel;
        }

        public async Task<List<VehicleGroupModel>> ListAvailableVehicleGroups(int companyId, Guid userId, string? searchTerm)
        {
            var assignedGroupIds = await _context.AdminUserVehicleGroups
                .Where(u => u.FkUserId == userId && (u.IsDeleted == null || u.IsDeleted == false))
                .Select(u => u.FkVehicleGroupId)
                .ToListAsync();

            var query = _context.VehicleGroups
                .Where(u => u.FkCompanyId == companyId && (u.IsDeleted == null || u.IsDeleted == false)
                && !assignedGroupIds.Contains(u.PkVehicleGroupId));

            if (!string.IsNullOrEmpty(searchTerm) )
            {
                query = query.Where(u => u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var groups = await query.OrderBy(u => u.Name)
                .ToListAsync();
            var groupModels = _mapper.Map<List<VehicleGroupModel>>(groups);

            return groupModels;
        }

        public async Task<List<AdminUserModel>> ListUser(int companyId, string? searchTerm)
        {
            var query = _context.AdminUsers
                .Where(u => u.FkCompanyId == companyId && !u.IsLock && (u.IsDeleted == null || u.IsDeleted == false));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || u.Fullname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var users = await query.OrderBy(u => u.Fullname)
                .ToListAsync();
            return _mapper.Map<List<AdminUserModel>>(users);
        }

        public async Task UnassignVehicleGroups(Guid userId, List<int> vehicleGroupIds)
        {
            var userVehicleGroups = await _context.AdminUserVehicleGroups
                .Where(x => x.FkUserId == userId && vehicleGroupIds.Contains(x.FkVehicleGroupId) && (x.IsDeleted == null || x.IsDeleted == false))
                .ToListAsync();
            foreach (var uvg in userVehicleGroups)
            {
                uvg.IsDeleted = true;
                uvg.UpdatedDate = DateTime.UtcNow;
                uvg.UpdatedByUser = userId;
            }
            await _context.SaveChangesAsync();
        }
    }
}
