using Microsoft.Playwright;

namespace Automation.Model.PageObjects.AdminLoginPage;

public partial class AdminLoginPage
{
    public async Task AssertHeaderContentAsync(string expectedHeader)
    {
        await Assertions.Expect(Header.WrappedElement).ToHaveTextAsync(expectedHeader);
    }
}
