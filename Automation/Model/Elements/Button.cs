using Microsoft.Playwright;
using Automation.Configuration.Logging;


namespace Automation.Model.Elements;

public class Button : BaseElement
{
    public Button(IPage page, string selector) : base(page, selector)
    {
    }

    public async Task ClickAsync()
    {
        LoggingManager.LogMessage($"Clicking on {Selector}", typeof(Button));
        await WrappedElement.ClickAsync();      
    }
}