using Automation.Model.PageObjects;
using Automation.Model.Environment;
using Automation.Model.FormData;
using Automation.Configuration.Logging;
using Automation.Utilities.Attributes;
using Automation.Utilities.Waiters;
using Automation.Utilities.Helpers;
using Automation.Configuration;
using SixLabors.ImageSharp;
using Automation.Utilities.Helpers.Performance;

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
        /*
            - we could just write `Assert.That(await adminViewPage.HeaderContentAsync(), Is.EqualTo("Admin View"));`, but
            this way test may potentially become flaky, because it may fail if the page is not loaded yet
            - Playwright has a built-in `Expect` class that can be used to wait for a condition to be true, but
            it can not be used directly in tests, as elements `Locator` is not accessible from the test
            - so we use `PollingWaiter` to wait for the condition to be true (in `Assert.That` for better readability)
        */
        Assert.That(await PollingWaiter.TryWaitAsync(async () => await adminViewPage.HeaderContentAsync() == "Admin View"), Is.True);
        await adminViewPage.Navigation.LogoutAsync();

        adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.TakeScreenshotAsync("AdminLoginPage");
        Assert.That(await PollingWaiter.TryWaitAsync(async () => await adminLoginPage.HeaderContentAsync() == "Admin Login"), Is.True);
    }

    [Test]
    [Category(TestCategories.Smoke)]
    [UIRetry]
    public async Task CompareLoginPageScreenshotsPassTest()
    {
        var adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.GotoAsync();
        
        using var actual = Image.Load(await Page.ScreenshotAsync());
        /*
            - for Demo purposes, we are using the images saved in the `Automation/Tests/TestData/Images` folder
            - please replace the path below with the actual path to the `CompareLoginPagePass.png` image
        */
        string? expectedPath = null;
        if (expectedPath == null)
        {
            Assert.Ignore("Please provide the path to the `CompareLoginPageFail.png` image");
        }
        using var expected = Image.Load(expectedPath);
        Assert.That(ImageCompareHelper.ImagesAreEqual(actual, expected), Is.True);
    }

    [Test]
    [Category(TestCategories.Smoke)]
    [UIRetry]
    public async Task CompareLoginPageScreenshotsFailTest()
    {
        var adminLoginPage = new AdminLogin(Page);
        await adminLoginPage.GotoAsync();
        
        using var actual = Image.Load(await Page.ScreenshotAsync());
        /*
            - for Demo purposes, we are using the images saved in the `Automation/Tests/TestData/Images` folder
            - please replace the path below with the actual path to the `CompareLoginPageFail.png` image
        */
        string? expectedPath = null;
        if (expectedPath == null)
        {
            Assert.Ignore("Please provide the path to the `CompareLoginPageFail.png` image");
        }
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

        var performanceMetrics = await adminLoginPage.GetPerformanceMetrics();
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