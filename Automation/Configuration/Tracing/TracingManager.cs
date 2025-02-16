using Microsoft.Playwright;
using Automation.Utilities;
using NUnit.Framework.Interfaces;

namespace Automation.Configuration.Tracing;

/// <summary>
/// Contains methods to start and stop tracing.
/// Produces a trace zip file that can be used to analyze the test execution.
/// The trace zip file can be inspected on https://trace.playwright.dev/.
/// User can configure whether to include screenshots, snapshots, and sources in the trace.
/// </summary>
public static class TracingManager
{
    public static bool IncludeScreenshots { get; set; } = true;
    public static bool IncludeSnapshots { get; set; } = true;
    public static bool IncludeSources { get; set; } = true;

    /// <summary>
    /// Starts tracing.
    /// </summary>
    /// <param name="browserContext"></param>
    /// <returns></returns>
    public static async Task StartTracingAsync(IBrowserContext browserContext)
    {
        await browserContext.Tracing.StartAsync(new()
        {
            Title = $"{TestRunContext.TestFixture}.{TestRunContext.TestName}",
            Screenshots = IncludeScreenshots,
            Snapshots = IncludeSnapshots,
            Sources = IncludeSources     
        });
    }

    /// <summary>
    /// Stops tracing.
    /// If the tracing option is set to Always, the trace file is saved.
    /// If the tracing option is set to OnFail, the trace file is saved only if the test fails.
    /// </summary>
    /// <param name="browserContext"></param>
    /// <returns></returns>
    public static async Task StopTracingAsync(IBrowserContext browserContext)
    {
        var traceFileDirectory = Path.Combine(Settings.TracesDirectory, TestRunContext.TestFixture);
        var traceFilePath = Path.Combine(traceFileDirectory, $"{TestRunContext.TestName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.zip");
        
        if (Settings.Tracing == TracingOptions.Always)
        {
            await StopAsync(browserContext, true, traceFilePath);
        }
        if (Settings.Tracing == TracingOptions.OnFail)
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            if (testResult == ResultState.Error || testResult == ResultState.Failure)
            {
                await StopAsync(browserContext, true, traceFilePath);
            }
            else
            {
                await StopAsync(browserContext, false, traceFilePath);
            }
        }
    }

    private static async Task StopAsync(IBrowserContext browserContext, bool keepTrace, string traceFilePath)
    {
        await browserContext.Tracing.StopAsync(new()
        {
            Path = keepTrace ? traceFilePath : null
        });
    }
}
