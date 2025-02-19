using Automation.Utilities.Helpers;
using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;

namespace Automation.Model.Elements;

/// <summary>
/// Base class for all elements.
/// Elements are reusable ui components. for example: buttons, textboxes, etc.
/// </summary>
public class BaseElement
{
    protected readonly string Selector;
    public readonly ILocator WrappedElement;

    public BaseElement(IPage page, string selector)
    {
        Selector = selector;
        WrappedElement = page.Locator(selector);
    }

    /// <summary>
    /// Takes a screenshot of the element.
    /// </summary>
    /// <param name="screenshotName">Name of the screenshot.</param>
    public async Task TakeScreenshotAsync(string screenshotName)
    {
        await ScreenshotHelper.TakeScreenshotAsync(WrappedElement, screenshotName);
    }

    /// <summary>
    /// Runs an accessibility scan on the element.
    /// </summary>
    /// <returns>AxeResult object.</returns>
    public async Task<AxeResult> RunAccessibilityScanAsync()
    {
        var result = await WrappedElement.RunAxe();
        return result;
    }
}
