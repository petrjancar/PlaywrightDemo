using Microsoft.Playwright;
using Automation.Configuration.Logging;

namespace Automation.Model.Elements;

public class Header : BaseElement
{
    public Header(IPage page, string selector) : base(page, selector)
    {
    }

    public async Task<string?> GetTextAsync()
    {
        LoggingManager.LogMessage($"Getting text from {Selector}", typeof(Header));
        return await WrappedElement.TextContentAsync();
    }
}