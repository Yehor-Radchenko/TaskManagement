namespace TaskManagement.Common.ResponseModels;

using System.Text.Json;

public class ErrorDetails
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public override string ToString() => JsonSerializer.Serialize(this);
}