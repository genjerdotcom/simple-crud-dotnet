namespace Core.Responses;

public class ApiError
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "";
    public object? Errors { get; set; }
}