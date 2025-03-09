using Automation.Utilities.Attributes;
using Automation.Utilities.Helpers;
using Automation.Model.PageObjects.AdminLoginPage;
using Automation.Configuration;
using SixLabors.ImageSharp;

namespace Automation.Tests.LoginTests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class LoginVisualTests : BaseTest
{
    [Test]
    [Category(TestCategories.Smoke)]
    [UIRetry]
    public async Task CompareLoginPageScreenshotsPassTest()
    {
        var adminLoginPage = new AdminLoginPage(Page);
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
        var adminLoginPage = new AdminLoginPage(Page);
        await adminLoginPage.GotoAsync();
        
        using var actual = Image.Load(await Page.ScreenshotAsync());
        // for Demo purposes, we are using the images saved in the `Automation/Tests/TestData/Images` folder
        string expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Tests\TestData\Images\CompareLoginPageFail.png");
        using var expected = Image.Load(expectedPath);
        Assert.That(ImageCompareHelper.ImagesAreEqual(actual, expected), Is.True);
    }
}
