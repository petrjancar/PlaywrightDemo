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
public class BaseTest : BrowserTest
{
    public IBrowserContext Context { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;

    [SetUp]
    public async Task SetUp()
    {      
        if (Settings.VideoRecording != VideoRecordingOptions.Never)
        {
            Context = await Browser.NewContextAsync(VideoRecordingManager.VideoContextOptions());
        }
        else
        {
            Context = await Browser.NewContextAsync(ContextOptions());    
        }
        Page = await Context.NewPageAsync().ConfigureAwait(false);

        if (Settings.Logging)
        {
            LoggingManager.SetUpTestLogging(Page);
            Settings.LogSettings();
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
        if (Settings.VideoRecording != VideoRecordingOptions.Never)
        {
            await VideoRecordingManager.StopVideoRecordingAsync(Context, Page);
        }
    }

    /// <returns>New browser context options with the default configuration.</returns>
    public virtual BrowserNewContextOptions ContextOptions()
    {
        return new()
        {
            Locale = "en-US",
            ColorScheme = ColorScheme.Light
        };
    }
}
