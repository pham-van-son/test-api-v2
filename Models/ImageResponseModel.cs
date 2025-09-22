using System.Text.Json.Serialization;

namespace TestLab.Models
{
    /// <summary>
    /// Model phản hồi hình ảnh từ hệ thống.
    /// </summary>
    public class ImageResponseModel
    {
        /// <summary>
        /// Biển số xe.
        /// </summary>
        [JsonPropertyName("v")]
        public string VehiclePlate { get; set; } = string.Empty;

        /// <summary>
        /// Thời điểm chụp.
        /// </summary>
        [JsonPropertyName("c")]
        public DateTime CaptureTime { get; set; }

        /// <summary>
        /// Đường dẫn URL của hình ảnh.
        /// </summary>
        [JsonPropertyName("u")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Vận tốc (km/h).
        /// </summary>
        [JsonPropertyName("s")]
        public int Speed { get; set; }

        /// <summary>
        /// Kênh chụp.
        /// </summary>
        [JsonPropertyName("k")]
        public int Channel { get; set; }

        /// <summary>
        /// Chiều rộng của ảnh.
        /// </summary>
        [JsonPropertyName("w")]
        public int Width { get; set; }

        /// <summary>
        /// Chiều cao của ảnh.
        /// </summary>
        [JsonPropertyName("h")]
        public int Height { get; set; }

        /// <summary>
        /// ID hình ảnh.
        /// </summary>
        [JsonPropertyName("i")]
        public long Id { get; set; }

        /// <summary>
        /// Loại hình ảnh.
        /// </summary>
        [JsonPropertyName("t")]
        public int Type { get; set; }

        /// <summary>
        /// Giấy phép liên quan (nếu có).
        /// </summary>
        [JsonPropertyName("l")]
        public string License { get; set; } = string.Empty;

        /// <summary>
        /// Tên tài xế.
        /// </summary>
        [JsonPropertyName("n")]
        public string DriverName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Model yêu cầu lấy hình ảnh.
    /// </summary>
    public class ImageRequestModel
    {
        /// <summary>
        /// ID khách hàng.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Tên phương tiện.
        /// </summary>
        public string VehicleName { get; set; } = string.Empty;

        /// <summary>
        /// Danh sách kênh chụp.
        /// </summary>
        public int[] Channels { get; set; } = Array.Empty<int>();

        /// <summary>
        /// Thời gian bắt đầu.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Thời gian kết thúc.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Tần suất chụp.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Thời gian lưu trữ (phút/giờ/ngày tùy hệ thống).
        /// </summary>
        public int StorageTime { get; set; }

        /// <summary>
        /// Thứ tự sắp xếp ("asc" hoặc "desc", mặc định: desc).
        /// </summary>
        public string SortOrder { get; set; } = "desc";
    }

    /// <summary>
    /// Model phản hồi API hình ảnh.
    /// </summary>
    public class ImageApiResponse
    {
        /// <summary>
        /// Có thành công hay không.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Danh sách dữ liệu hình ảnh.
        /// </summary>
        public List<ImageResponseModel> Data { get; set; } = new();
    }
}
