using Microsoft.AspNetCore.Mvc;
using TestLab.IRepositories;
using TestLab.Models;

namespace TestLab.Controllers
{
    /// <summary>
    /// Controller quản lý các API liên quan đến phương tiện (Vehicle),
    /// nhóm phương tiện (VehicleGroup), mối quan hệ xe - nhóm, và hình ảnh phương tiện.
    /// </summary>
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        /// <summary>
        /// Khởi tạo VehicleController với repository.
        /// </summary>
        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        #region VehicleGroup

        /// <summary>
        /// Lấy danh sách nhóm phương tiện.
        /// </summary>
        [HttpGet("groups")]
        public async Task<IActionResult> GetVehicleGroups()
        {
            try
            {
                var response = await _vehicleRepository.GetVehicleGroupsAsync(15076);
                return Ok(new ResponseSingleContentModel<List<VehicleGroupModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách nhóm xe thành công",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý: " + ex.Message,
                    Data = null
                });
            }
        }

        /// <summary>
        /// Lấy thông tin nhóm phương tiện theo ID.
        /// </summary>
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetVehicleGroupById(int groupId)
        {
            try
            {
                var response = await _vehicleRepository.GetVehicleGroupByIdAsync(groupId, 15076);
                return Ok(new ResponseSingleContentModel<VehicleGroupModel>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin nhóm xe thành công",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý: " + ex.Message,
                    Data = null
                });
            }
        }

        #endregion

        #region Vehicle

        /// <summary>
        /// Lấy danh sách phương tiện theo danh sách ID nhóm.
        /// </summary>
        [HttpGet("vehicles")]
        public async Task<IActionResult> GetVehicles([FromQuery] List<int> groupIds)
        {
            try
            {
                var response = await _vehicleRepository.GetVehiclesAsync(15076, groupIds);
                return Ok(new ResponseSingleContentModel<List<VehicleVehicleModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách xe thành công",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý: " + ex.Message,
                    Data = null
                });
            }
        }

        /// <summary>
        /// Lấy thông tin phương tiện theo ID.
        /// </summary>
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicleById(int vehicleId)
        {
            try
            {
                var response = await _vehicleRepository.GetVehicleByIdAsync(vehicleId, 15076);
                return Ok(new ResponseSingleContentModel<VehicleVehicleModel>
                {
                    StatusCode = 200,
                    Message = "Lấy thông tin xe thành công",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý: " + ex.Message,
                    Data = null
                });
            }
        }

        #endregion

        #region VehicleVehicleGroup

        /// <summary>
        /// Lấy danh sách quan hệ giữa phương tiện và nhóm phương tiện.
        /// </summary>
        [HttpGet("vehicle-groups")]
        public async Task<IActionResult> GetVehicleVehicleGroups()
        {
            try
            {
                var response = await _vehicleRepository.GetVehicleVehicleGroupsAsync(15076);
                return Ok(new ResponseSingleContentModel<List<VehicleVehicleGroupModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách quan hệ xe - nhóm xe thành công",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý: " + ex.Message,
                    Data = null
                });
            }
        }

        #endregion

        #region Image

        /// <summary>
        /// Lấy danh sách hình ảnh phương tiện theo yêu cầu, có phân trang.
        /// </summary>
        [HttpPost("vehicle-images")]
        public async Task<IActionResult> GetVehicleImages([FromBody] ImageRequestModel model, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var images = await _vehicleRepository.GetVehicleImagesAsync(model, page, pageSize);
                return Ok(new ResponseSingleContentModel<PaginationSet<ImageResponseModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách ảnh xe thành công",
                    Data = images
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Lỗi trong GetVehicleImages: " + ex.ToString());
                return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi trong quá trình xử lý: " + ex.Message,
                    Data = null
                });
            }
        }

        #endregion
    }
}
