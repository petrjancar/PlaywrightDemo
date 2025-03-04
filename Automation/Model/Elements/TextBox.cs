using Microsoft.Playwright;
using Automation.Configuration.Logging;

namespace Automation.Model.Elements;

public class TextBox : BaseElement
{
    public TextBox(IPage page, string selector) : base(page, selector)
    {
    }

    public async Task<string?> GetTextAsync()
    {
        LoggingManager.LogMessage($"Getting text from {Selector}", typeof(TextBox));
        return await WrappedElement.GetAttributeAsync("value");
    }

    public async Task SetTextAsync(string text)
    {
        LoggingManager.LogMessage($"Setting text on {Selector} to {text}", typeof(TextBox));
        await WrappedElement.FillAsync(text);
    }
    
    public async Task ClearTextAsync()
    {
        LoggingManager.LogMessage($"Clearing text on {Selector}", typeof(TextBox));
        await WrappedElement.FillAsync("");
    }
}
