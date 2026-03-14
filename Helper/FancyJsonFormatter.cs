using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using System.Text.Json;
using System.Text.Encodings.Web;

public class FancyJsonFormatter : ConsoleFormatter
{
    public FancyJsonFormatter() : base("fancyJson") { }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        var level = logEntry.LogLevel;
        
        var shortLevel = level switch
        {
            LogLevel.Trace       => "trace",
            LogLevel.Debug       => "dbug ",
            LogLevel.Information => "info ",
            LogLevel.Warning     => "warn ",
            LogLevel.Error       => "error",
            LogLevel.Critical    => "crit ",
            _                    => "unkn "
        };

        var color = level switch
        {
            LogLevel.Trace       => "\x1b[38;5;242m", 
            LogLevel.Debug       => "\x1b[1;36m",   
            LogLevel.Information => "\x1b[1;32m",   
            LogLevel.Warning     => "\x1b[1;33m", 
            LogLevel.Error       => "\x1b[1;31m",   
            LogLevel.Critical    => "\x1b[1;37;41m",  
            _                    => "\x1b[1;35m"
        };
        var reset = "\x1b[0m";

        textWriter.Write($"{color}{shortLevel}{reset}");
        textWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        var logData = new Dictionary<string, object?>
        {
            ["Timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            ["LogLevel"] = level.ToString(),
            ["Category"] = logEntry.Category,
            ["Message"] = logEntry.Formatter(logEntry.State, logEntry.Exception)
        };

        if (logEntry.State is IEnumerable<KeyValuePair<string, object>> properties)
        {
            foreach (var prop in properties)
            {
                if (prop.Key != "{OriginalFormat}")
                {
                    logData[prop.Key] = prop.Value;
                }
            }
        }

        var scopes = new List<object>();
        scopeProvider?.ForEachScope<object?>((scope, state) => {
            if (scope is IEnumerable<KeyValuePair<string, object>> scopeDict)
            {
                var dict = new Dictionary<string, object>();
                foreach (var pair in scopeDict) dict[pair.Key] = pair.Value;
                scopes.Add(dict);
            }
            else
            {
                scopes.Add(scope?.ToString() ?? "");
            }
        }, null);
        logData["Scopes"] = scopes;

        if (logEntry.Exception != null)
        {
            logData["Exception"] = logEntry.Exception.ToString();
        }

        var jsonOptions = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
        };

        try 
        {
            textWriter.WriteLine(JsonSerializer.Serialize(logData, jsonOptions));
        }
        catch (Exception)
        {
            var safeData = new Dictionary<string, object?>();
            foreach(var item in logData)
            {
                if (item.Value != null && !item.Value.GetType().IsPrimitive && item.Value is not string)
                    safeData[item.Key] = item.Value.ToString();
                else
                    safeData[item.Key] = item.Value;
            }
            textWriter.WriteLine(JsonSerializer.Serialize(safeData, jsonOptions));
        }
        textWriter.WriteLine();
    }
}