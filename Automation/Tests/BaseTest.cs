using Microsoft.Playwright.NUnit;
using Automation.Configuration.Logging;
using Automation.Configuration;
using Automation.Configuration.Tracing;

namespace Automation.Tests;

/// <summary>
/// BaseTest class is the base class for all test classes in the project.
/// It contains the SetUp and TearDown methods that are executed before
/// and after each test method.
/// </summary>
public class BaseTest : PageTest
{
    [SetUp]
    public async Task SetUp()
    {      
        if (Settings.Logging)
        {
            LoggingManager.SetUpTestLogging(Page);
        }
        if (Settings.Tracing != TracingOptions.Never)
        {
            await TracingManager.StartTracingAsync(Context);
        }
    }
 
    [TearDown]
    public async Task TearDown()
    {
        if (Settings.Tracing != TracingOptions.Never)
        {
            await TracingManager.StopTracingAsync(Context);
        }
    }
}
