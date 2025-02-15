using Automation.Utilities;
using Microsoft.Playwright;
using NUnit.Framework.Interfaces;

namespace Automation.Configuration.VideoRecording;

public class VideoManager
{
    public static int Width { get; set; } = 1280;
    public static int Height { get; set; } = 720;

    public static Task<IBrowserContext> GetVideoContextAsync(IBrowser browser)
    {
        var videoFileDirectory = Path.Combine(Settings.VideosDirectory, TestRunContext.TestFixture);

        return browser.NewContextAsync(new BrowserNewContextOptions
        {
            RecordVideoDir = videoFileDirectory,
            RecordVideoSize = new RecordVideoSize { Width = Width, Height = Height }
        });
    }

    public static async Task StopVideoRecordingAsync(IBrowserContext context, IPage page)
    {
        await context.CloseAsync();
   
        var videoPath = await GetVideoPathAsync(page);

        if (Settings.VideoRecording == VideoOptions.OnFail)
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
