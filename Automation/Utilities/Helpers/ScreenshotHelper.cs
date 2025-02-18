using Microsoft.Playwright;
using Automation.Configuration;

namespace Automation.Utilities.Helpers;

/// <summary>
/// Helper class for taking screenshots.
/// </summary>
public static class ScreenshotHelper
{
    /// <summary>
    /// Takes a screenshot of the page and saves it to the screenshots directory.
    /// </summary>
    /// <param name="page">The page to take a screenshot of.</param>
    /// <param name="screenshotName">The name of the screenshot.</param>
    public static async Task TakeScreenshotAsync(IPage page, string screenshotName)
    {
        if (!Settings.Screenshots)
        {
            return;
        }

        var screenshotFileDirectory = Path.Combine(Settings.ScreenshotsDirectory, TestRunContext.TestFixture);
        var screenshotFilePath = Path.Combine(screenshotFileDirectory, $"{screenshotName}_{TestRunContext.TestName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");
        
        await page.ScreenshotAsync(new () {
            Path = screenshotFilePath
        });
    }

    /// <summary>
    /// Takes a screenshot of the element and saves it to the screenshots directory.
    /// </summary>
    /// <param name="locator">The locator of the element to take a screenshot of.</param>
    /// <param name="screenshotName">The name of the screenshot.</param>
    public static async Task TakeScreenshotAsync(ILocator locator, string screenshotName)
    {
        if (!Settings.Screenshots)
        {
            return;
        }

        var screenshotFileDirectory = Path.Combine(Settings.ScreenshotsDirectory, TestRunContext.TestFixture);
        var screenshotFilePath = Path.Combine(screenshotFileDirectory, $"{screenshotName}_{TestRunContext.TestName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");
        
        await locator.ScreenshotAsync(new () {
            Path = screenshotFilePath
        });
    }
}
