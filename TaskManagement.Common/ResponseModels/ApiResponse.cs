namespace TaskManagement.Common.ResponseModels;

public class ApiResponse<T>
{
    public ApiResponse()
    {
        this.Success = true;
    }

    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public T Data { get; set; } = default!;

    public List<string> ErrorMessages { get; set; } = new List<string>();
}