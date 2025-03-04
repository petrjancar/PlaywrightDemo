using Automation.Utilities.Helpers;
using Automation.Utilities.Helpers.Performance;
using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;

namespace Automation.Model.PageObjects;

/// <summary>
/// Base class for all page objects.
/// Page objects are classes that represent a page in the application.
/// </summary>
public abstract class BasePage
{
    public readonly IPage Page;
    public abstract string Url { get; }

    public BasePage(IPage page)
    {
        Page = page;
    }

    /// <summary>
    /// Navigates to the page and waits for URL to match.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task GotoAsync()
    {
        if (Url == null)
        {
            throw new NullReferenceException("Url was null.");         
        }

        await Page.GotoAsync(Url);
        await ExpectUrlAsync();
    }

    /// <summary>
    /// Waits for URL to match.
    /// </summary>
    public async Task ExpectUrlAsync()
    {
        if (Url == null)
        {
            throw new NullReferenceException("Url was null.");         
        }

        await Assertions.Expect(Page).ToHaveURLAsync(Url);
    }

    /// <summary>
    /// Waits for title to match.
    /// </summary>
    /// <param name="expected">The expected title.</param>
    public async Task ExpectTitleAsync(string expected)
    {
        await Assertions.Expect(Page).ToHaveTitleAsync(expected);
    }

    /// <summary>
    /// Takes a screenshot of the page.
    /// </summary>
    /// <param name="name">The name of the screenshot.</param>
    public async Task TakeScreenshotAsync(string name)
    {
        await ScreenshotHelper.TakeScreenshotAsync(Page, name);
    }

    /// <summary>
    /// Runs an accessibility scan on the page.
    /// </summary>
    /// <returns>AxeResult object.</returns>
    public async Task<AxeResult> RunAccessibilityScanAsync()
    {
        var result = await Page.RunAxe();
        return result;
    }

    /// <summary>
    /// Captures performance metrics for the page.
    /// The performance metrics are calculated relative to the navigation start time.
    /// </summary>
    /// <remarks>
    /// Ensure that the page is fully loaded before calling this method.
    /// </remarks>
    /// <returns>PerformanceMetrics object.</returns>
    public Task<PerformanceMetrics> GetPerformanceMetricsAsync()
    {
        return PerformanceHelper.GetPerformanceMetricsAsync(Page);
    }
}

