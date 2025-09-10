using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_lab.IRepositories;
using test_lab.Models;
using static Azure.Core.HttpHeader;

namespace test_lab.Controllers
{
  [Route("api/vehicle")]
  [ApiController]
  public class VehicleController : ControllerBase
  {
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleController(IVehicleRepository vehicleRepository)
    {
      _vehicleRepository = vehicleRepository;
    }

    #region VehicleGroup
    [HttpGet("groups")]
    public async Task<IActionResult> vehicleGroupList()
    {
      try
      {
        var response = await _vehicleRepository.vehicleGroupList(15076);
        return Ok(new ResponseSingleContentModel<List<VehicleGroupModel>>
        {
          StatusCode = 200,
          Message = "Lấy danh sách nhóm xe thành công",
          Data = response,
        });
      }
      catch (Exception ex)
      {
        return Ok(new ResponseSingleContentModel<string>
        {
          StatusCode = 500,
          Message = "Có lỗi xảy ra trong quá trình xử lý!!!" + ex.Message,
          Data = null,
        });
      }
    }

    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> vehicleGroupById(int groupId)
    {
      try
      {
        var response = await _vehicleRepository.vehicleGroupById(groupId, 15076);
        return Ok(new ResponseSingleContentModel<VehicleGroupModel>
        {
          StatusCode = 200,
          Message = "Lấy thông tin nhóm xe thành công",
          Data = response,
        });
      }
      catch (Exception ex)
      {
        return Ok(new ResponseSingleContentModel<string>
        {
          StatusCode = 500,
          Message = "Có lỗi xảy ra trong quá trình xử lý!!!" + ex.Message,
          Data = null,
        });
      }
    }
    #endregion

    #region vehicle
    [HttpGet("vehicles")]
    public async Task<IActionResult> vehicleVehicleList([FromQuery] List<int> groupIds)
    {
      try
      {
        var response = await _vehicleRepository.vehicleVehicleList(15076, groupIds);
        return Ok(new ResponseSingleContentModel<List<VehicleVehicleModel>>
        {
          StatusCode = 200,
          Message = "Lấy danh sách xe thành công",
          Data = response,
        });
      }
      catch (Exception ex)
      {
        return Ok(new ResponseSingleContentModel<string>
        {
          StatusCode = 500,
          Message = "Có lỗi xảy ra trong quá trình xử lý!!!" + ex.Message,
          Data = null,
        });
      }
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> vehicleVehicleById(int vehicleId)
    {
      try
      {
        var response = await _vehicleRepository.vehicleVehicleById(vehicleId, 15076);
        return Ok(new ResponseSingleContentModel<VehicleVehicleModel>
        {
          StatusCode = 200,
          Message = "Lấy thông tin xe thành công",
          Data = response,
        });
      }
      catch (Exception ex)
      {
        return Ok(new ResponseSingleContentModel<string>
        {
          StatusCode = 500,
          Message = "Có lỗi xảy ra trong quá trình xử lý!!!" + ex.Message,
          Data = null,
        });
      }
    }
    #endregion

    #region VehicleVehicleGroup
    [HttpGet("vehicle-groups")]
    public async Task<IActionResult> vehicleVehicleGroupList()
    {
      try
      {
        var response = await _vehicleRepository.vehicleVehicleGroupList(15076);
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
          Message = "Có lỗi trong quá trình xử lý" + ex.Message,
          Data = null
        });
      }
    }
    #endregion

    #region Image
    [HttpPost("vehicle-images")]
    public async Task<IActionResult> GetVehicleImagesAsync([FromBody] ImageRequestModel model, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
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
        Console.WriteLine("❌ Error in GetVehicleImagesAsync: " + ex.ToString());
        return Ok(new ResponseSingleContentModel<string>
        {
          StatusCode = 500,
          Message = "Có lỗi trong quá trình xử lý",
          Data = null
        });
      }
    }
    #endregion
  }
}
