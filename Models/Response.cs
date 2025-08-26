using Microsoft.AspNetCore.Mvc;

namespace test_lab.Models
{
  public class PaginationSet<T> where T : class
  {
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPage => (int)Math.Ceiling((double)TotalCount / PageSize);

    public List<T> Items { get; set; } = new();
  }

  public class ResponseSingleContentModel<T>
  {
    public string Message { get; set; } = "Success";

    public int StatusCode { get; set; } = 200;

    public T? Data { get; set; }

    public IActionResult ToResult()
    {
      return StatusCode switch
      {
        200 => new OkObjectResult(new { Message, Data }),
        201 => new CreatedResult(string.Empty, new { Message, Data }),
        400 => new BadRequestObjectResult(new { Message, Data }),
        404 => new NotFoundObjectResult(new { Message, Data }),
        _ => new ObjectResult(new { Message, Data }) { StatusCode = StatusCode }
      };
    }
  }
}
