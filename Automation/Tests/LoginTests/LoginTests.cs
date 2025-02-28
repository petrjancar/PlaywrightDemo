using Automation.Model.PageObjects;
using Automation.Model.Environment;
using Automation.Model.FormData;
using Automation.Configuration.Logging;
using Automation.Utilities.Attributes;
using Automation.Utilities.Waiters;
using Automation.Utilities.Helpers;
using Automation.Configuration;
using SixLabors.ImageSharp;

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
        Assert.That(
            async () => await adminViewPage.HeaderContentAsync() == "Admin View",
            Is.True.After(WaitTime.Short.Milliseconds, PollingInterval.Default.Milliseconds)
        );
        await adminViewPage.Navigation.LogoutAsync();

        adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.TakeScreenshotAsync("AdminLoginPage");
        Assert.That(
            async () => await adminLoginPage.HeaderContentAsync() == "Admin Login", 
            Is.True.After(WaitTime.Short.Milliseconds, PollingInterval.Default.Milliseconds)
        );
    }

    [Test]
    [Category(TestCategories.Smoke)]
    [UIRetry]
    public async Task CompareLoginPageScreenshotsPassTest()
    {
        var adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.GotoAsync();
        
        using var actual = Image.Load(await Page.ScreenshotAsync());
        // for Demo purposes, we are using the images saved in the `Automation/Tests/TestData/Images` folder
        string expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Tests\TestData\Images\CompareLoginPagePass.png");
        using var expected = Image.Load(expectedPath);
        Assert.That(ImageCompareHelper.ImagesAreEqual(actual, expected, 90), Is.True);
    }

    [Test]
    [Category(TestCategories.Smoke)]
    [UIRetry]
    [Ignore("Ignoring this test due to GitHub Actions")]
    public async Task CompareLoginPageScreenshotsFailTest()
    {
        var adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.GotoAsync();
        
        using var actual = Image.Load(await Page.ScreenshotAsync());
        // for Demo purposes, we are using the images saved in the `Automation/Tests/TestData/Images` folder
        string expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Tests\TestData\Images\CompareLoginPageFail.png");
        using var expected = Image.Load(expectedPath);
        Assert.That(ImageCompareHelper.ImagesAreEqual(actual, expected), Is.True);
    }

    [Test]
    [Category(TestCategories.Performance)]
    [UIRetry]
    public async Task LoginPagePerformanceTest()
    {
        var adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.GotoAsync();

        var performanceMetrics = await adminLoginPage.GetPerformanceMetricsAsync();
        LoggingManager.LogMessage($"Performance Metrics:\n{performanceMetrics}", typeof(LoginTests));
    }

    [Test]
    [Category(TestCategories.Accessibility)]
    [UIRetry]
    public async Task LoginPageAccessibilityTest()
    {
        var adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.GotoAsync();

        var accessibilityResults = await adminLoginPage.RunAccessibilityScanAsync();
        LoggingManager.LogMessage($"Accessibility Violations:", typeof(LoginTests));
        foreach (var violation in accessibilityResults.Violations)
        {
            LoggingManager.LogMessage($"{violation}", typeof(LoginTests));
        }
    }
}