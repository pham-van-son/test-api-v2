using AutoMapper;
using Azure.Core;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Models;

namespace test_lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;

        public DriverController(IDriverRepository driverRepository, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Cập nhật thông tin lái xe
        /// </summary>
        [HttpPut("update-drivers")]
        public async Task<IActionResult> UpdateDrivers([FromBody] UpdateDriversRequest request)
        {
            try
            {
                var entity = _mapper.Map<HrmEmployee>(request.UpdateData);

                var response = await _driverRepository.UpdateEmployees(15076, request.EmployeeIds, entity);

                if (response > 0)
                {
                    return Ok(new ResponseSingleContentModel<int>
                    {
                        StatusCode = 200,
                        Message = $"Cập nhật thành công {response} lái xe",
                        Data = response
                    });
                }
                else
                {
                    return NotFound(new ResponseSingleContentModel<int>
                    {
                        StatusCode = 404,
                        Message = "Không tìm thấy lái xe để cập nhật",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình xử lý!!! " + ex.Message,
                    Data = null
                });
            }
        }

        /// <summary>
        /// Lấy danh sách lái xe có phân trang + filter theo DisplayName, DriverLicense
        /// </summary>
        [HttpGet("list-drivers")]
        public async Task<IActionResult> GetDrivers([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? searchTerm = null, [FromQuery] string? driverLicense = null)
        {
            try
            {
                var result = await _driverRepository.GetPagedList(15076, page, pageSize, searchTerm, driverLicense);

                var models = _mapper.Map<List<HrmEmployeeModel>>(result.Items);

                return Ok(new ResponseSingleContentModel<PaginationSet<HrmEmployeeModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách lái xe thành công",
                    Data = new PaginationSet<HrmEmployeeModel>
                    {
                        Page = result.Page,
                        PageSize = result.PageSize,
                        TotalCount = result.TotalCount,
                        Items = models
                    }
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

        /// <summary>
        /// Lấy danh sách loại bằng lái xe (đang hoạt động, chưa xóa)
        /// </summary>
        [HttpGet("list-license")]
        public async Task<IActionResult> GetLicenseTypes([FromQuery] string? searchTerm = null)
        {
            try
            {
                var result = await _driverRepository.GetLicenseTypes(searchTerm);

                return Ok(new ResponseSingleContentModel<IEnumerable<BcaLicenseTypeModel>>
                {
                    StatusCode = 200,
                    Message = "Lấy danh sách loại bằng lái thành công",
                    Data = result
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

        /// <summary>
        /// Xuất file Excel danh sách lái xe
        /// </summary>
        [HttpGet("export-drivers")]
        public async Task<IActionResult> ExportDrivers([FromQuery] string? searchTerm = null, [FromQuery] string? driverLicense = null)
        {
            try
            {
                var employees = await _driverRepository.GetAllEmployees(15076);
                var models = _mapper.Map<List<HrmEmployeeModel>>(employees);

                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Drivers");

                // Header
                ws.Cell(1, 1).Value = "STT";
                ws.Cell(1, 2).Value = "Họ tên";
                ws.Cell(1, 3).Value = "SĐT";
                ws.Cell(1, 4).Value = "Số GPLX";
                ws.Cell(1, 5).Value = "Ngày cấp";
                ws.Cell(1, 6).Value = "Ngày hết hạn";
                ws.Cell(1, 7).Value = "Nơi cấp";
                ws.Cell(1, 8).Value = "Loại bằng";
                ws.Cell(1, 9).Value = "Ngày cập nhật";

                int row = 2;
                int stt = 1;
                foreach (var d in models)
                {
                    ws.Cell(row, 1).Value = stt++;
                    ws.Cell(row, 2).Value = d.DisplayName;
                    ws.Cell(row, 3).Value = d.Mobile;
                    ws.Cell(row, 4).Value = d.DriverLicense;
                    ws.Cell(row, 5).Value = d.IssueLicenseDate?.ToString("dd/MM/yyyy");
                    ws.Cell(row, 6).Value = d.ExpireLicenseDate?.ToString("dd/MM/yyyy");
                    ws.Cell(row, 7).Value = d.IssueLicensePlace;
                    ws.Cell(row, 8).Value = d.LicenseType;
                    ws.Cell(row, 9).Value = (d.UpdatedDate ?? d.CreatedDate).ToString("HH:mm dd/MM/yyyy");
                    row++;
                }

                ws.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Drivers.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Có lỗi xảy ra trong quá trình export: " + ex.Message,
                    Data = null
                });
            }
        }
    }
}
