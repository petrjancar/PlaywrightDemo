using Microsoft.Playwright;
using Automation.Configuration.Logging;

namespace Automation.Model.Elements;

public class Checkbox : BaseElement
{
    public Checkbox(IPage page, string selector) : base(page, selector)
    {
    }

    public async Task<bool> IsCheckedAsync()
    {
        LoggingManager.LogMessage($"Checking if {Selector} is checked", typeof(Checkbox));
        return await WrappedElement.IsCheckedAsync();
    }

    public async Task CheckAsync()
    {
        if (!await IsCheckedAsync())
        {
            LoggingManager.LogMessage($"Checking {Selector}", typeof(Checkbox));
            await WrappedElement.CheckAsync();
        }
    }

    public async Task UncheckAsync()
    {
        if (await IsCheckedAsync())
        {
            LoggingManager.LogMessage($"Unchecking {Selector}", typeof(Checkbox));
            await WrappedElement.UncheckAsync();
        }
    }
}
