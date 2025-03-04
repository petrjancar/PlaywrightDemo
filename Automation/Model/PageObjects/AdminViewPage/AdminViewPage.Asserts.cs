using Microsoft.Playwright;

namespace Automation.Model.PageObjects.AdminViewPage;

public partial class AdminViewPage : BasePage
{
    public async Task AssertHeaderContentAsync(string expectedHeader)
    {
        await Assertions.Expect(Header.WrappedElement).ToHaveTextAsync(expectedHeader);
    }
}