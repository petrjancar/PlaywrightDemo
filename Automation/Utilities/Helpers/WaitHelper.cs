using Microsoft.Playwright;
using Automation.Configuration.Logging;

namespace Automation.Utilities.Helpers;

/// <summary>
/// Helper class for waiting operations.
/// </summary>
public static class WaitHelper
{
    /// <summary>
    /// Waits for a network request with the specified URL pattern and a 200 status code.
    /// </summary>
    /// <param name="urlPattern">The URL pattern to match the network request.</param>
    /// <param name="timeout">Timeout for the wait operation. Default: 30 seconds.</param>
    /// <example>
    /// <code>
    /// await apiWaiters.WaitForApiRequestAsync("/api/users");
    /// </code>
    /// </example>
    public static async Task WaitForApiRequestAsync(IPage page, string urlPattern, TimeSpan? timeout = null)
    {
        if (timeout == null)
        {
            timeout = TimeSpan.FromSeconds(30);
        }

        var response = await page.WaitForResponseAsync(
            response => response.Url.Contains(urlPattern) && response.Status == 200,
            new() { Timeout = (float)timeout.Value.TotalMilliseconds }
        );
        
        if (response != null)
        {
            LoggingManager.LogMessage("API call succeeded!", typeof(WaitHelper));
        }
        else
        {
            LoggingManager.LogMessage("API call failed.", typeof(WaitHelper));
        }
    }

    /// <summary>
    /// Wait for an API response with a specific status code.
    /// </summary>
    /// <param name="urlPattern">The URL pattern to match the network request.</param>
    /// <param name="expectedStatus">Expected status code.</param>
    /// <param name="timeout">Timeout for the wait operation. Default: 30 seconds.</param>
    /// <example>
    /// <code>
    /// await apiWaiters.WaitForApiRequestAsync("/api/users", 201);
    /// </code>
    /// </example>
    public static async Task WaitForApiResponseStatusAsync(IPage page, string urlPattern, int expectedStatus, TimeSpan? timeout = null)
    {
        if (timeout == null)
        {
            timeout = TimeSpan.FromSeconds(30);
        }
        
        var response = await page.WaitForResponseAsync(
            response => response.Url.Contains(urlPattern) && response.Status == expectedStatus,
            new() { Timeout = (float)timeout.Value.TotalMilliseconds }
        );
        
        if (response != null)
        {
            LoggingManager.LogMessage($"API call succeeded with status {expectedStatus}!", typeof(WaitHelper));
        }
        else
        {
            LoggingManager.LogMessage($"API call did not match the expected status.", typeof(WaitHelper));
        }
    }
}
