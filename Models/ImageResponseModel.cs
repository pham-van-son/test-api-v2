using System.Text.Json.Serialization;

namespace test_lab.Models
{
  public class ImageResponseModel
  {
    [JsonPropertyName("v")]
    public string VehiclePlate { get; set; } = string.Empty;

    [JsonPropertyName("c")]
    public DateTime CaptureTime { get; set; }

    [JsonPropertyName("u")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("s")]
    public int Speed { get; set; }

    [JsonPropertyName("k")]
    public int Channel { get; set; }

    [JsonPropertyName("w")]
    public int Width { get; set; }

    [JsonPropertyName("h")]
    public int Height { get; set; }

    [JsonPropertyName("i")]
    public long Id { get; set; }

    [JsonPropertyName("t")]
    public int Type { get; set; }

    [JsonPropertyName("l")]
    public string License { get; set; } = string.Empty;

    [JsonPropertyName("n")]
    public string DriverName { get; set; } = string.Empty;
  }

  public class ImageRequestModel
  {
    public int CustomerId { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public int[] Channels { get; set; } = Array.Empty<int>();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Frequency { get; set; }
    public int StorageTime { get; set; }
    public string SortOrder { get; set; } = "desc";
  }

  public class ImageApiResponse
  {
    public bool IsSuccess { get; set; }

    public List<ImageResponseModel> Data { get; set; } = new();
  }
}
