using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

public class RequestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestMiddleware> _logger;
    private static readonly string[] SensitiveKeys = { "password", "token", "secret", "creditcard", "cvv" };

    public RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = Stopwatch.GetTimestamp();
        var requestId = context.Request.Headers["X-Request-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

        string requestBody = await ReadRequestBody(context);

        using (_logger.BeginScope(new Dictionary<string, object> 
        { 
            ["RequestId"] = requestId,
            ["Method"] = context.Request.Method,
            ["Path"] = context.Request.Path,
            ["RequestBody"] = requestBody
        }))
        {
            _logger.LogInformation("HTTP Request Started: {Method} {Path}", context.Request.Method, context.Request.Path);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorElapsed = Stopwatch.GetElapsedTime(startTime);
                _logger.LogError(ex, "HTTP Request Crashed: {Method} {Path} in {Duration}ms", 
                    context.Request.Method, context.Request.Path, errorElapsed.TotalMilliseconds);
                throw;
            }

            var elapsed = Stopwatch.GetElapsedTime(startTime);
            var statusCode = context.Response.StatusCode;

            if (statusCode >= 500)
                _logger.LogError("HTTP Request Failed: {Method} {Path} responded {StatusCode} in {Duration}ms", 
                    context.Request.Method, context.Request.Path, statusCode, elapsed.TotalMilliseconds);
            else if (statusCode >= 400)
                _logger.LogWarning("HTTP Request Client Error: {Method} {Path} responded {StatusCode} in {Duration}ms", 
                    context.Request.Method, context.Request.Path, statusCode, elapsed.TotalMilliseconds);
            else
                _logger.LogInformation("HTTP Request Finished: {Method} {Path} responded {StatusCode} in {Duration}ms", 
                    context.Request.Method, context.Request.Path, statusCode, elapsed.TotalMilliseconds);
        }
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();

        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        
        context.Request.Body.Position = 0;

        if (string.IsNullOrEmpty(body)) return string.Empty;

        return MaskSensitiveData(body);
    }

    private string MaskSensitiveData(string json)
    {
        try 
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            foreach (var key in SensitiveKeys)
            {
                var pattern = $"(\"{key}\"\\s*:\\s*\")([^\"]+)(\")";
                json = Regex.Replace(json, pattern, "$1******$3", RegexOptions.IgnoreCase);
            }
            return json;
        }
        catch 
        {
            return "[Non-JSON or Empty Body]";
        }
    }
}