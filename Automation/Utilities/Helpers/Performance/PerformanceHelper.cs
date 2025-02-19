using System.Text.Json;
using Microsoft.Playwright;

namespace Automation.Utilities.Helpers.Performance;

/// <summary>
/// Helper class to capture performance metrics.
/// </summary>
public class PerformanceHelper
{
    /// <summary>
    /// Captures performance metrics for the given page.
    /// The performance metrics are calculated relative to the navigation start time.
    /// </summary>
    /// <remarks>
    /// Ensure that the page is fully loaded before calling this method.
    /// </remarks>
    /// <param name="page"></param>
    /// <returns>PerformanceMetrics object.</returns>
    public static async Task<PerformanceMetrics> GetPerformanceMetricsAsync(IPage page)
    {
        var timing = await GetPerformanceTimingAsync(page);
        return CalculateRelativeTimings(timing);
    }

    private static async Task<JsonElement> GetPerformanceTimingAsync(IPage page)
    {
        return await page.EvaluateAsync<JsonElement>(@"
            () => {
                const timing = window.performance.timing;
                return {
                    navigationStart: timing.navigationStart,
                    domContentLoadedEventEnd: timing.domContentLoadedEventEnd,
                    loadEventEnd: timing.loadEventEnd,
                    responseStart: timing.responseStart,
                    responseEnd: timing.responseEnd,
                    requestStart: timing.requestStart
                };
            }");
    }

    private static PerformanceMetrics CalculateRelativeTimings(JsonElement timing)
    {
        var navigationStart = timing.GetProperty("navigationStart").GetDouble();
        var navigationStartDateTime = DateTimeOffset.FromUnixTimeMilliseconds((long)navigationStart).DateTime;

        return new ()
        {
            NavigationStart = navigationStartDateTime,
            DomContentLoadedEventEnd = timing.GetProperty("domContentLoadedEventEnd").GetDouble() - navigationStart,
            LoadEventEnd = timing.GetProperty("loadEventEnd").GetDouble() - navigationStart,
            ResponseStart = timing.GetProperty("responseStart").GetDouble() - navigationStart,
            ResponseEnd = timing.GetProperty("responseEnd").GetDouble() - navigationStart,
            RequestStart = timing.GetProperty("requestStart").GetDouble() - navigationStart
        };
    }
}
