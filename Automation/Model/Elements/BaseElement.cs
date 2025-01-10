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
}
