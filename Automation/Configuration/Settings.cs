using Automation.Configuration.Tracing;
using Automation.Configuration.Logging;
using Automation.Configuration.VideoRecording;

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
    public static string VideosDirectory => Path.Combine(ResultsDirectory, "Videos");
    public static string ScreenshotsDirectory => Path.Combine(ResultsDirectory, "Screenshots");
    public static bool Logging { get; set; } = true;
    public static TracingOptions Tracing { get; set; } = TracingOptions.Always;
    public static VideoOptions VideoRecording { get; set; } = VideoOptions.Always;
    public static bool Screenshots { get; set; } = true;
    public static int RetryCount { get; set; } = 2;

    static Settings()
    {
        string? resultsDirectory = TestContext.Parameters["ResultsDirectory"];
        string? logging = TestContext.Parameters["Logging"];
        string? tracing = TestContext.Parameters["Tracing"];
        string? videoRecording = TestContext.Parameters["VideoRecording"];
        string? screenshots = TestContext.Parameters["Screenshots"];
        string? retryCount = TestContext.Parameters["RetryCount"];

        ResultsDirectory = resultsDirectory ?? ResultsDirectory;
        Logging = bool.TryParse(logging, out var loggingResult) ? loggingResult : Logging;
        Tracing = Enum.TryParse(tracing, out TracingOptions tracingResult) ? tracingResult : Tracing;
        VideoRecording = Enum.TryParse(videoRecording, out VideoOptions videoRecordingResult) ? videoRecordingResult : VideoRecording;
        Screenshots = bool.TryParse(screenshots, out var screenshotsResult) ? screenshotsResult : Screenshots;
        RetryCount = int.TryParse(retryCount, out var retryCountResult) ? retryCountResult : RetryCount;
    }

    /// <summary>
    /// This method logs all settings.
    /// </summary>
    public static void LogSettings()
    {
        LoggingManager.LogMessage($"ResultsDirectory: {ResultsDirectory}", typeof(Settings));
        LoggingManager.LogMessage($"LogsDirectory: {LogsDirectory}", typeof(Settings));
        LoggingManager.LogMessage($"TracesDirectory: {TracesDirectory}", typeof(Settings));
        LoggingManager.LogMessage($"ScreenshotsDirectory: {ScreenshotsDirectory}", typeof(Settings));
        LoggingManager.LogMessage($"Logging: {Logging}", typeof(Settings));
        LoggingManager.LogMessage($"Tracing: {Tracing}", typeof(Settings));
        LoggingManager.LogMessage($"VideoRecording: {VideoRecording}", typeof(Settings));
        LoggingManager.LogMessage($"Screenshots: {Screenshots}", typeof(Settings));
        LoggingManager.LogMessage($"RetryCount: {RetryCount}", typeof(Settings));
    }

    /// <summary>
    /// This method is used to force the static constructor to run.
    /// </summary>
    public static void Initialize()
    {
        
    }
}
