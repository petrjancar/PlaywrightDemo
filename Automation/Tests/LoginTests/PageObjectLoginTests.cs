using Automation.Model.PageObjects;
using Automation.Model.Environment;
using Automation.Model.FormData;
using Automation.Utilities.Helpers;
using Automation.Configuration.Logging;

namespace Automation.Tests.LoginTests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class PageObjectLoginTests : BaseTest
{
    [Test]
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
        await adminViewPage.ExpectHeaderContentAsync("Admin View");
        await adminViewPage.Navigation.LogoutAsync();

        adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.ExpectUrlAsync();
        await adminLoginPage.ExpectHeaderContentAsync("Admin Login");

        LoggingManager.LogMessage("Hello World", GetType());
        await ScreenshotHelper.TakeScreenshotAsync(Page, "HelloWorld");
    }
}