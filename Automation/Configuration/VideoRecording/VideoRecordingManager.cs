using Automation.Utilities;
using Microsoft.Playwright;
using NUnit.Framework.Interfaces;

namespace Automation.Configuration.VideoRecording;

/// <summary>
/// This class contains methods to setup and stop video recording.
/// Produces a video file in .webm format, that can be used to analyze the test execution.
/// User can configure video Width and Height.
/// </summary>
public class VideoRecordingManager
{
    public static int Width { get; set; } = 1280;
    public static int Height { get; set; } = 720;

    /// <summary>
    /// Returns a new browser context with video recording enabled.
    /// </summary>
    /// <param name="browser"></param>
    /// <returns>New browser context.</returns>
    public static Task<IBrowserContext> GetVideoContextAsync(IBrowser browser)
    {
        var videoFileDirectory = Path.Combine(Settings.VideosDirectory, TestRunContext.TestFixture);

        return browser.NewContextAsync(new BrowserNewContextOptions
        {
            RecordVideoDir = videoFileDirectory,
            RecordVideoSize = new RecordVideoSize { Width = Width, Height = Height }
        });
    }

    /// <summary>
    /// Stops video recording.
    /// If the video recording option is set to Always, the video file is saved.
    /// If the video recording option is set to OnFail, the video file is saved only if the test fails.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public static async Task StopVideoRecordingAsync(IBrowserContext context, IPage page)
    {
        await context.CloseAsync();
   
        var videoPath = await GetVideoPathAsync(page);

        if (Settings.VideoRecording == VideoRecordingOptions.OnFail)
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            if (testResult != ResultState.Error && testResult != ResultState.Failure)
            {
                if (videoPath != null && File.Exists(videoPath))
                {
                    File.Delete(videoPath);
                }
                return;
            }
        }

        if (videoPath != null && File.Exists(videoPath))
        {
            var newVideoFileName = $"{TestRunContext.TestName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.webm";
            var newVideoPath = Path.Combine(Settings.VideosDirectory, TestRunContext.TestFixture, newVideoFileName);
            File.Move(videoPath, newVideoPath);
        }
    }

    private static async Task<string?> GetVideoPathAsync(IPage page)
    {
        return page.Video != null ? await page.Video.PathAsync() : null;
    }
}
