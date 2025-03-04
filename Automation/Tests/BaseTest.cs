using Microsoft.Playwright.NUnit;
using Automation.Configuration.Logging;
using Automation.Configuration;
using Automation.Configuration.Tracing;
using Automation.Configuration.VideoRecording;
using Microsoft.Playwright;

namespace Automation.Tests;

/// <summary>
/// Base class for all test classes in the project.
/// It contains the SetUp and TearDown methods that are executed before
/// and after each test method.
/// </summary>
public abstract class BaseTest : PageTest
{
    [SetUp]
    public async Task SetUp()
    {            
        if (Settings.Tracing != TracingOptions.Never)
        {
            await TracingManager.StartTracingAsync(Context);
        }
        if (Settings.Logging)
        {
            LoggingManager.SetUpTestLogging(Page);
            Settings.LogSettings();
        } 
        /*
            For video recording initialization, see 
            the overriden ContextOptions method below.
        */
    }
 
    [TearDown]
    public async Task TearDown()
    {   
        if (Settings.Tracing != TracingOptions.Never)
        {
            await TracingManager.StopTracingAsync(Context);
        }
        if (Settings.VideoRecording != VideoRecordingOptions.Never)
        {
            await VideoRecordingManager.StopVideoRecordingAsync(Context, Page);
        }      
        if (Settings.Logging)
        {
            LoggingManager.CloseLogger();
        }
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        if (Settings.VideoRecording != VideoRecordingOptions.Never)
        {
            return VideoRecordingManager.VideoContextOptions();
        }
        else
        {
            return new()
            {
                Locale = "en-US",
                ColorScheme = ColorScheme.Light,
            };
        }
    }
}
