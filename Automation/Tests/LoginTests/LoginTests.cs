using Automation.Model.PageObjects;
using Automation.Model.Environment;
using Automation.Model.FormData;
using Automation.Utilities.Helpers;
using Automation.Configuration.Logging;
using Automation.Utilities.Attributes;
using Automation.Utilities.Waiters;
using Automation.Configuration;

namespace Automation.Tests.LoginTests;

[TestFixture]
public class LoginTests : BaseTest
{
    [Test]
    [Category(TestCategories.Smoke)]
    [UIRetry]
    public async Task LoginLogoutTest()
    {
        var adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.GotoAsync();
        await adminLoginPage.LoginAsync(new AdminLoginFormData
        {
            Username = Credentials.AdminUserName,
            Password = Credentials.AdminPassword,
            RememberMe = false
        });

        var adminViewPage = new AdminView(Page);
        await adminViewPage.ExpectUrlAsync();
        Assert.That(await PollingWaiter.TryWaitAsync(async () => await adminViewPage.HeaderContentAsync() == "Admin View"), Is.True);
        await adminViewPage.Navigation.LogoutAsync();

        adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.ExpectUrlAsync();
        Assert.That(await PollingWaiter.TryWaitAsync(async () => await adminLoginPage.HeaderContentAsync() == "Admin Login"), Is.True);

        LoggingManager.LogMessage("Hello World", GetType());
        await ScreenshotHelper.TakeScreenshotAsync(Page, "HelloWorld");
    }
}