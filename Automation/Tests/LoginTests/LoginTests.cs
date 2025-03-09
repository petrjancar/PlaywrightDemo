using Automation.Model.Environment;
using Automation.Configuration.Logging;
using Automation.Utilities.Attributes;
using Automation.Configuration;
using Automation.Model.PageObjects.AdminLoginPage;
using Automation.Model.PageObjects.AdminLoginPage.Data;
using Automation.Model.PageObjects.AdminViewPage;

namespace Automation.Tests.LoginTests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class LoginTests : BaseTest
{
    [Test]
    [Category(TestCategories.Smoke)]
    [UIRetry]
    public async Task LoginLogoutTest()
    {
        var adminLoginPage = new AdminLoginPage(Page);
        await adminLoginPage.GotoAsync();
        await adminLoginPage.LoginAsync(new AdminLoginFormData
        {
            Username = Credentials.AdminUserName,
            Password = Credentials.AdminPassword,
            RememberMe = false
        });

        var adminViewPage = new AdminViewPage(Page);
        await adminViewPage.AssertHeaderContentAsync("Admin View");
        await adminViewPage.Navigation.LogoutAsync();

        adminLoginPage = new AdminLoginPage(Page);
        await adminLoginPage.TakeScreenshotAsync("AdminLoginPage");
        await adminLoginPage.AssertHeaderContentAsync("Admin Login");
    }

    [Test]
    [Category(TestCategories.Performance)]
    [UIRetry]
    public async Task LoginPagePerformanceTest()
    {
        var adminLoginPage = new AdminLoginPage(Page);
        await adminLoginPage.GotoAsync();

        var performanceMetrics = await adminLoginPage.GetPerformanceMetricsAsync();
        LoggingManager.LogMessage($"Performance Metrics:\n{performanceMetrics}", typeof(LoginTests));
    }

    [Test]
    [Category(TestCategories.Accessibility)]
    [UIRetry]
    public async Task LoginPageAccessibilityTest()
    {
        var adminLoginPage = new AdminLoginPage(Page);
        await adminLoginPage.GotoAsync();

        var accessibilityResults = await adminLoginPage.RunAccessibilityScanAsync();
        LoggingManager.LogMessage($"Accessibility Violations:", typeof(LoginTests));
        foreach (var violation in accessibilityResults.Violations)
        {
            LoggingManager.LogMessage($"{violation}", typeof(LoginTests));
        }
    } 
}