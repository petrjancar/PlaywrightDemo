using Automation.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Serilog;
using Serilog.Extensions.Logging;

namespace Automation.Configuration.Logging;

/// <summary>
/// This class contains methods to set up logging for the test run.
/// Uses a generic ILogger to log messages to a file.
/// User can configure the minimum log level, whether to log requests, responses, and page errors.
/// </summary>
public static class LoggingManager
{
    public static LogLevel MinimumLevel { get; set; } = LogLevel.Information;
    public static bool LogRequests { get; set; } = true;    
    public static bool LogResponses { get; set; } = true;
    public static bool LogPageErrors { get; set; } = true;

    private static Microsoft.Extensions.Logging.ILogger? Logger;

    private static Serilog.Events.LogEventLevel ConvertLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => Serilog.Events.LogEventLevel.Verbose,
            LogLevel.Debug => Serilog.Events.LogEventLevel.Debug,
            LogLevel.Information => Serilog.Events.LogEventLevel.Information,
            LogLevel.Warning => Serilog.Events.LogEventLevel.Warning,
            LogLevel.Error => Serilog.Events.LogEventLevel.Error,
            LogLevel.Critical => Serilog.Events.LogEventLevel.Fatal,
            _ => Serilog.Events.LogEventLevel.Information,
        };
    }

    private static Microsoft.Extensions.Logging.ILogger CreateDefaultLogger()
    {
        var logFileDirectory = Path.Combine(Settings.LogsDirectory, TestRunContext.TestFixture);
        var logFilePath = Path.Combine(logFileDirectory, $"{TestRunContext.TestName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(ConvertLogLevel(MinimumLevel))
            .WriteTo.File(logFilePath)
            .CreateLogger();

        var loggerFactory = new SerilogLoggerFactory(Log.Logger);
        return loggerFactory.CreateLogger("AutomationLogger");
    }

    /// <summary>
    /// This method sets up logging for current test.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="logger">Optional custom logger. If null, default Serilog logger will be used.</param>
    /// <returns></returns>
    public static void SetUpTestLogging(IPage page, Microsoft.Extensions.Logging.ILogger? logger = null)
    {
        Logger = logger ?? CreateDefaultLogger();

        // New page instance is created for each test
        // Hence, we need to set up logging for each page instance
        if (LogRequests)
        {
            page.Request += (_, e) => LogRequest(e);
        }
        if (LogResponses)
        {
            page.Response += async (_, e) => await LogResponseAsync(e);
        }
        if (LogPageErrors)
        {
            page.PageError += (_, e) => LogPageError(e);
        }
    }

    /// <summary>
    /// This method logs a message with the specified log level.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="originator"></param>
    /// <param name="level"></param>
    public static void LogMessage(string message, Type originator, LogLevel level = LogLevel.Information)
    {
        var originatorName = originator.FullName;

        Logger.Log(level, "{Originator}: {Message}", originatorName, message);
    }

    private static void LogRequest(IRequest request)
    {
        Logger.LogInformation("Request: {Url} - Method: {Method}", request.Url, request.Method);
    }

    private static async Task LogResponseAsync(IResponse response)
    {
        try
        {
            var responseBody = await response.TextAsync();
            Logger.LogInformation("Response body: {ResponseBody}", responseBody);
        }
        catch (PlaywrightException ex)
        {
            Logger.LogError(ex, "Failed to get response body for request {RequestUrl}", response.Url);
        }
    }

    private static void LogPageError(string error)
    {
        Logger.LogError("Page Error: {Error}", error);
    }
}