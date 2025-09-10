using AutoMapper;
using Microsoft.EntityFrameworkCore;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Models;

namespace test_lab.Reposotpries
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

        public Task AssignVehicleGroups(Guid userId, List<int> vehicleGroupIds)
        {
            throw new NotImplementedException();
        }

        public Task<List<VehicleGroupModel>> ListAssignedVehicleGroups(int companyId, Guid userId, string? searchTerm)
        {
            throw new NotImplementedException();
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

            var groups = await query.OrderBy(u => u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
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

            var users = await query.OrderBy(u => u.Fullname, StringComparer.OrdinalIgnoreCase)
                .ToListAsync();
            return _mapper.Map<List<AdminUserModel>>(users);
        }

        public Task UnassignVehicleGroups(Guid userId, List<int> vehicleGroupIds)
        {
            throw new NotImplementedException();
        }
    }
}
