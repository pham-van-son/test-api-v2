using AutoMapper;
using TestLab.Entities;
using TestLab.Models;

namespace TestLab.Mapper
{
    /// <summary>
    /// Cấu hình AutoMapper cho các Entity và Model.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Khởi tạo các mapping giữa Entity và Model.
        /// </summary>
        public AutoMapperProfile()
        {
            // Mapping cho Vehicle
            CreateMap<VehicleVehicle, VehicleVehicleModel>();
            CreateMap<VehicleVehicleModel, VehicleVehicle>();

            // Mapping cho VehicleGroup
            CreateMap<VehicleGroup, VehicleGroupModel>();
            CreateMap<VehicleGroupModel, VehicleGroup>();

            // Mapping cho VehicleVehicleGroup
            CreateMap<VehicleVehicleGroup, VehicleVehicleGroupModel>();
            CreateMap<VehicleVehicleGroupModel, VehicleVehicleGroup>();
        }
    }
}
