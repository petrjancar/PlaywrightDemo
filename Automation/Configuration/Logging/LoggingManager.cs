using Automation.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Serilog;
using Serilog.Extensions.Logging;
using System.Collections.Concurrent;

namespace Automation.Configuration.Logging;

/// <summary>
/// Manages logging for the test run, ensuring thread safety and isolated logs.
/// User can configure the minimum log level, whether to log requests, responses, and page errors.
/// </summary>
public static class LoggingManager
{
    public static LogLevel MinimumLevel { get; set; } = LogLevel.Information;
    public static bool LogRequests { get; set; } = false;
    public static bool LogResponses { get; set; } = false;
    public static bool LogPageErrors { get; set; } = false;

    private static readonly ConcurrentDictionary<string, Microsoft.Extensions.Logging.ILogger> Loggers = new();

    private static Serilog.Events.LogEventLevel ConvertLogLevelToSerilogLevel(LogLevel logLevel)
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

    /// <summary>
    /// Creates a Serilog logger for the specific test based on the test name.
    /// </summary>
    private static Microsoft.Extensions.Logging.ILogger CreateSerilogLogger(string testName)
    {
        var testSuiteName = TestRunContext.TestFixture;
        var logFileDirectory = Path.Combine(Settings.LogsDirectory, testSuiteName);
        Directory.CreateDirectory(logFileDirectory);

        // Name the log file based on the test name and timestamp
        var logFilePath = Path.Combine(logFileDirectory, $"{testName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");

        // Set up Serilog logger with the configured log file
        var logger = new LoggerConfiguration()
            .MinimumLevel.Is(ConvertLogLevelToSerilogLevel(MinimumLevel))
            .WriteTo.File(logFilePath)
            .CreateLogger();

        var loggerFactory = new SerilogLoggerFactory(logger);
        return loggerFactory.CreateLogger("AutomationLogger");
    }

    /// <summary>
    /// Creates a logger for the specific test based on the test name.
    /// </summary>
    private static Microsoft.Extensions.Logging.ILogger CreateLoggerForTest(string testName)
    {
        var logger = CreateSerilogLogger(testName);
        return logger;
    }

    /// <summary>
    /// Retrieves or creates a logger for the current test based on the test name.
    /// </summary>
    private static Microsoft.Extensions.Logging.ILogger GetLogger()
    {
        return Loggers.GetOrAdd(TestRunContext.TestName, CreateLoggerForTest);
    }

    /// <summary>
    /// Sets up logging for the current test.
    /// </summary>
    /// <param name="page">The Playwright page to log events from.</param>
    public static void SetUpTestLogging(IPage page)
    {
        var testLogger = GetLogger();

        if (LogRequests)
        {
            page.Request += (_, e) => testLogger.LogInformation("Request: {Url} - Method: {Method}", e.Url, e.Method);
        }
        if (LogResponses)
        {
            page.Response += async (_, e) =>
            {
                try
                {
                    var responseBody = await e.TextAsync();
                    testLogger.LogInformation("Response body: {ResponseBody}", responseBody);
                }
                catch (PlaywrightException ex)
                {
                    testLogger.LogError(ex, "Failed to get response body for request {RequestUrl}", e.Url);
                }
            };
        }
        if (LogPageErrors)
        {
            page.PageError += (_, e) => testLogger.LogError("Page Error: {Error}", e);
        }
    }

    /// <summary>
    /// Closes the logger for the current test.
    /// </summary>
    public static void CloseLogger()
    {
        var testName = TestRunContext.TestName;

        if (Loggers.TryRemove(testName, out var logger))
        {
            if (logger is IDisposable disposableLogger)
            {
                disposableLogger.Dispose();
            }
        }
    }
    
    /// <summary>
    /// Logs a message with the specified log level.
    /// </summary>
    public static void LogMessage(string message, Type originator, LogLevel level = LogLevel.Information)
    {
        if (!Settings.Logging)
        {
            return;
        }

        var testLogger = GetLogger();
        var originatorName = originator.FullName;

        testLogger.Log(level, "{Originator}: {Message}", originatorName, message);
    }
}
