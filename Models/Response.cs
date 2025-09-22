using Microsoft.AspNetCore.Mvc;

namespace TestLab.Models
{
    /// <summary>
    /// Lớp chứa thông tin phân trang cho danh sách dữ liệu.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng trong danh sách.</typeparam>
    public class PaginationSet<T> where T : class
    {
        /// <summary>
        /// Trang hiện tại.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Số lượng bản ghi trên mỗi trang.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số bản ghi.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Tổng số trang (được tính toán dựa trên TotalCount và PageSize).
        /// </summary>
        public int TotalPage => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// Danh sách dữ liệu.
        /// </summary>
        public List<T> Items { get; set; } = new();
    }

    /// <summary>
    /// Lớp chuẩn hóa phản hồi khi trả về dữ liệu đơn lẻ.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu trả về.</typeparam>
    public class ResponseSingleContentModel<T>
    {
        /// <summary>
        /// Thông điệp trả về (mặc định "Success").
        /// </summary>
        public string Message { get; set; } = "Success";

        /// <summary>
        /// Mã trạng thái HTTP (mặc định 200).
        /// </summary>
        public int StatusCode { get; set; } = 200;

        /// <summary>
        /// Dữ liệu trả về.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Chuyển đối tượng phản hồi thành IActionResult phù hợp với StatusCode.
        /// </summary>
        public IActionResult ToResult()
        {
            return StatusCode switch
            {
                200 => new OkObjectResult(new { Message, Data }),
                201 => new CreatedResult(string.Empty, new { Message, Data }),
                400 => new BadRequestObjectResult(new { Message, Data }),
                404 => new NotFoundObjectResult(new { Message, Data }),
                _ => new ObjectResult(new { Message, Data })
                {
                    StatusCode = StatusCode
                }
            };
        }
    }
}
