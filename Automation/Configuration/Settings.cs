using Automation.Configuration.Tracing;

namespace Automation.Configuration;

/// <summary>
/// This class contains settings that can be used to configure the test run.
/// The settings can be set in the .runsettings file.
/// </summary>
public static class Settings
{
    public static string ResultsDirectory { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "TestResults");
    public static string LogsDirectory => Path.Combine(ResultsDirectory, "Logs");
    public static string TracesDirectory => Path.Combine(ResultsDirectory, "Traces");
    public static string ScreenshotsDirectory => Path.Combine(ResultsDirectory, "Screenshots");
    public static bool Logging { get; set; } = true;
    public static TracingOptions Tracing { get; set; } = TracingOptions.Always;
    public static bool Screenshots { get; set; } = true;
    public static int RetryCount { get; set; } = 2;

    static Settings()
    {
        string? resultsDirectory = TestContext.Parameters["ResultsDirectory"];
        string? logging = TestContext.Parameters["Logging"];
        string? tracing = TestContext.Parameters["Tracing"];
        string? screenshots = TestContext.Parameters["Screenshots"];
        string? retryCount = TestContext.Parameters["RetryCount"];

        if (resultsDirectory != null)
        {
            ResultsDirectory = resultsDirectory;
        }
        if (logging != null)
        {
            Logging = bool.Parse(logging);
        }
        if (tracing != null)
        {
            Tracing = (TracingOptions)Enum.Parse(typeof(TracingOptions), tracing);
        }
        if (screenshots != null)
        {
            Screenshots = bool.Parse(screenshots);
        }
        if (retryCount != null)
        {
            RetryCount = int.Parse(retryCount);
        }
    }

    /// <summary>
    /// This method is used to force the static constructor to run.
    /// </summary>
    public static void Initialize()
    {
        
    }
}
