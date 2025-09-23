using AutoMapper;
using Azure.Core;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Models;

namespace test_lab.Controllers
{
    [Route("api/driver")]
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
        [HttpPost("export-drivers-custom")]
        public async Task<IActionResult> ExportDriversCustom([FromBody] ExportConfig config = null)
        {
            try
            {
                // Lấy dữ liệu từ repository
                var employees = await _driverRepository.GetAllEmployees(15076);
                var models = _mapper.Map<List<HrmEmployeeModel>>(employees);

                // Nếu config null, dùng default
                config ??= new ExportConfig
                {
                    Title = "THÔNG TIN LÁI XE",
                    LicenseCategories = "A, A1, A2, A3, A4, B1, B2, C, D, E, F, FC, FB2, FI, FD, FE",
                    MergeTitleRows = "1:2", // Merge hàng 1-2
                    MergeCategoriesRows = "3:5" // Merge hàng 3-5
                };

                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Drivers");

                // Merge hàng 1-2 cho tiêu đề, center, bold
                var titleRange = ParseRange(config.MergeTitleRows, 9); // 9 cột (A-I)
                ws.Range(titleRange).Merge();
                var titleCell = ws.Cell("A1");
                titleCell.Value = config.Title;
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Merge hàng 3-5 cho danh sách hạng bằng lái, center, vertical top
                var categoriesRange = ParseRange(config.MergeCategoriesRows, 9); // 9 cột (A-I)
                ws.Range(categoriesRange).Merge();
                var categoriesCell = ws.Cell("A3");
                categoriesCell.Value = $"Danh sách hạng bằng lái: {config.LicenseCategories}";
                categoriesCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                categoriesCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                categoriesCell.Style.Font.Italic = true;

                // Hàng 6: Header cột, màu xanh chỉ cho các ô từ STT đến Ngày cập nhật
                ws.Cell(6, 1).Value = "STT";
                ws.Cell(6, 2).Value = "Họ tên";
                ws.Cell(6, 3).Value = "SĐT";
                ws.Cell(6, 4).Value = "Số GPLX";
                ws.Cell(6, 5).Value = "Ngày cấp";
                ws.Cell(6, 6).Value = "Ngày hết hạn";
                ws.Cell(6, 7).Value = "Nơi cấp";
                ws.Cell(6, 8).Value = "Loại bằng";
                ws.Cell(6, 9).Value = "Ngày cập nhật";

                // Style header: Màu xanh và bold chỉ cho cột 1-9
                for (int col = 1; col <= 9; col++)
                {
                    var cell = ws.Cell(6, col);
                    cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                    cell.Style.Font.Bold = true;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Từ hàng 7: Đổ dữ liệu và áp dụng All Borders cho toàn bộ bảng (hàng 6 trở đi)
                int row = 7;
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

                    // Áp dụng All Borders cho từng ô trong hàng hiện tại
                    for (int col = 1; col <= 9; col++)
                    {
                        var cell = ws.Cell(row, col);
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }
                    row++;
                }

                // Áp dụng All Borders cho header hàng 6
                for (int col = 1; col <= 9; col++)
                {
                    var cell = ws.Cell(6, col);
                    cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }

                // Adjust columns width
                ws.Columns().AdjustToContents();

                // Export stream
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Drivers_Custom.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Hàm helper để parse range từ "start:end" thành "Astart:Iend"
        private string ParseRange(string rangeStr, int columnCount)
        {
            var parts = rangeStr.Split(':');
            if (parts.Length < 2) throw new ArgumentException("Range must be in format 'start:end'");

            int startRow = int.Parse(parts[0]);
            int endRow = int.Parse(parts[parts.Length - 1]);
            string startCol = "A";
            string endCol = GetColumnLetter(columnCount - 1);

            return $"{startCol}{startRow}:{endCol}{endRow}";
        }

        private string GetColumnLetter(int columnIndex)
        {
            return columnIndex < 26
                ? ((char)('A' + columnIndex)).ToString()
                : throw new ArgumentException("Column index too high for single letter");
        }
    }
}

public class ExportConfig
{
    public string Title { get; set; }
    public string LicenseCategories { get; set; }
    public string MergeTitleRows { get; set; }
    public string MergeCategoriesRows { get; set; }
}
