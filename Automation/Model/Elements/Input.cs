using Microsoft.Playwright;
using Automation.Configuration.Logging;

namespace Automation.Model.Elements;

public class Input : BaseElement
{
    public Input(IPage page, string selector) : base(page, selector)
    {
    }

    public async Task<string?> GetTextAsync()
    {
        LoggingManager.LogMessage($"Getting text from {Selector}", typeof(Input));
        return await WrappedElement.GetAttributeAsync("value");
    }

    public async Task SetTextAsync(string text)
    {
        LoggingManager.LogMessage($"Setting text on {Selector} to {text}", typeof(Input));
        await WrappedElement.FillAsync(text);
    }
    
    public async Task ClearTextAsync()
    {
        LoggingManager.LogMessage($"Clearing text on {Selector}", typeof(Input));
        await WrappedElement.FillAsync("");
    }
}
