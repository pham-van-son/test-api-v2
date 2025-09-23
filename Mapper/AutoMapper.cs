using AutoMapper;
using test_lab.Entities;
using test_lab.Models;

namespace test_lab.Mapper
{
  public class AutoMapperProfile: Profile
  {
    public AutoMapperProfile()
    {
        CreateMap<VehicleVehicle, VehicleVehicleModel>();
        CreateMap<VehicleVehicleModel, VehicleVehicle>();

        CreateMap<VehicleGroup, VehicleGroupModel>();
        CreateMap<VehicleGroupModel, VehicleGroup>();

        CreateMap<VehicleVehicleGroup, VehicleVehicleGroupModel>();
        CreateMap<VehicleVehicleGroupModel, VehicleVehicleGroup>();

        CreateMap<AdminUser, AdminUserModel>();
        CreateMap<AdminUserModel, AdminUser>();

        CreateMap<AdminUserVehicleGroup, AdminUserVehicleGroupModel>();
        CreateMap<AdminUserVehicleGroupModel, AdminUserVehicleGroup>();

        CreateMap<BcaLicenseType, BcaLicenseTypeModel>();
        CreateMap<BcaLicenseTypeModel, BcaLicenseType>();

        CreateMap<HrmEmployee, HrmEmployeeModel>();
        CreateMap<HrmEmployeeModel, HrmEmployee>();
    }
  }
}
