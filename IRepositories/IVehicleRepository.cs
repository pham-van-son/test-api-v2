using TestLab.Models;

namespace TestLab.IRepositories
{
    /// <summary>
    /// Interface định nghĩa các thao tác với phương tiện (Vehicle), nhóm phương tiện (VehicleGroup)
    /// và mối quan hệ giữa chúng.
    /// </summary>
    public interface IVehicleRepository
    {
        #region VehicleGroup

        /// <summary>
        /// Lấy danh sách nhóm phương tiện theo công ty.
        /// </summary>
        /// <param name="companyId">ID công ty.</param>
        /// <returns>Danh sách nhóm phương tiện.</returns>
        Task<List<VehicleGroupModel>> GetVehicleGroupsAsync(int companyId);

        /// <summary>
        /// Lấy thông tin nhóm phương tiện theo ID.
        /// </summary>
        /// <param name="groupId">ID nhóm phương tiện.</param>
        /// <param name="companyId">ID công ty.</param>
        /// <returns>Thông tin nhóm phương tiện.</returns>
        Task<VehicleGroupModel> GetVehicleGroupByIdAsync(int groupId, int companyId);

        #endregion

        #region Vehicle

        /// <summary>
        /// Lấy danh sách phương tiện theo công ty và nhóm.
        /// </summary>
        /// <param name="companyId">ID công ty.</param>
        /// <param name="groupIds">Danh sách ID nhóm phương tiện.</param>
        /// <returns>Danh sách phương tiện.</returns>
        Task<List<VehicleVehicleModel>> GetVehiclesAsync(int companyId, List<int> groupIds);

        /// <summary>
        /// Lấy thông tin phương tiện theo ID.
        /// </summary>
        /// <param name="vehicleId">ID phương tiện.</param>
        /// <param name="companyId">ID công ty.</param>
        /// <returns>Thông tin phương tiện.</returns>
        Task<VehicleVehicleModel> GetVehicleByIdAsync(int vehicleId, int companyId);

        #endregion

        #region VehicleVehicleGroup

        /// <summary>
        /// Lấy danh sách mối quan hệ giữa phương tiện và nhóm phương tiện theo công ty.
        /// </summary>
        /// <param name="companyId">ID công ty.</param>
        /// <returns>Danh sách VehicleVehicleGroup.</returns>
        Task<List<VehicleVehicleGroupModel>> GetVehicleVehicleGroupsAsync(int companyId);

        #endregion

        #region Images

        /// <summary>
        /// Lấy danh sách hình ảnh phương tiện theo yêu cầu.
        /// </summary>
        /// <param name="model">Model yêu cầu hình ảnh.</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="pageSize">Số bản ghi mỗi trang.</param>
        /// <returns>Danh sách hình ảnh phân trang.</returns>
        Task<PaginationSet<ImageResponseModel>> GetVehicleImagesAsync(
            ImageRequestModel model, int page, int pageSize);

        #endregion
    }
}
