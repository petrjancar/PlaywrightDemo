using Microsoft.Playwright;
using Automation.Configuration.Logging;
using Automation.Utilities.Helpers;
using Automation.Utilities.Attributes;

namespace Automation.Tests.LoginTests;

[TestFixture]
public class GeneratedLoginTests : BaseTest
{
    [Test]
    [UIRetry]
    public async Task LoginLogoutGeneratedTest()
    {
        await Page.GotoAsync("https://eviltester.github.io/simpletodolist/adminlogin.html");
        await Page.GetByPlaceholder("Enter Username").FillAsync("Admin");
        await Page.GetByPlaceholder("Enter Password").FillAsync("AdminPass");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://eviltester.github.io/simpletodolist/adminview.html");
        await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Admin View");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Logout" }).ClickAsync();
        
        await Expect(Page).ToHaveURLAsync("https://eviltester.github.io/simpletodolist/adminlogin.html");
        await Expect(Page.Locator("h1")).ToContainTextAsync("Admin Login");

        LoggingManager.LogMessage("Hello World", GetType());
        await ScreenshotHelper.TakeScreenshotAsync(Page, "HelloWorld");
    }
}
