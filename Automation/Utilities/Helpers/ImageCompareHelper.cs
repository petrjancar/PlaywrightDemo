using SixLabors.ImageSharp;
using Codeuctivity.ImageSharpCompare;
using Automation.Configuration.Logging;
using Automation.Configuration;

namespace Automation.Utilities.Helpers;

public class ImageCompareHelper
{
    /// <summary>
    /// Compares two images and if they are not equal, saves the difference image to the screenshots directory.
    /// </summary>
    /// <param name="actual"></param>
    /// <param name="expected"></param>
    /// <param name="pixelMatchThreshold">The percentage of pixels that must match for the images to be considered equal.</param>
    /// <returns>True if images are equal within the threshold, otherwise false.</returns>
    public static bool ImagesAreEqual(Image actual, Image expected, double pixelMatchThreshold = 100.0)
    {
        LoggingManager.LogMessage($"Comparing images within the threshold: {pixelMatchThreshold} %.", typeof(ImageCompareHelper));

        var diff = ImageSharpCompare.CalcDiff(actual, expected);
        var pixelErrorPercentage = diff.PixelErrorPercentage;

        LoggingManager.LogMessage($"PixelErrorCount: {diff.PixelErrorCount}", typeof(ImageCompareHelper));
        LoggingManager.LogMessage($"PixelErrorPercentage: {diff.PixelErrorPercentage}", typeof(ImageCompareHelper));
        LoggingManager.LogMessage($"AbsoluteError: {diff.AbsoluteError}", typeof(ImageCompareHelper));
        LoggingManager.LogMessage($"MeanError: {diff.MeanError}", typeof(ImageCompareHelper));

        if (pixelErrorPercentage <= (100.0 - pixelMatchThreshold))
        {
            LoggingManager.LogMessage("Images are equal within the threshold.", typeof(ImageCompareHelper));
            return true;
        }
        else
        {
            LoggingManager.LogMessage("Images are NOT equal within the threshold.", typeof(ImageCompareHelper));
            
            // Save the difference image to the screenshots directory
            var diffFileDirectory = Path.Combine(Settings.ScreenshotsDirectory, TestRunContext.TestFixture);          
            var diffFilePath = Path.Combine(diffFileDirectory,  $"DiffMask_{TestRunContext.TestName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");

            if (!Directory.Exists(diffFileDirectory))
            {
                Directory.CreateDirectory(diffFileDirectory);
            }

            using var fileStreamDifferenceMask = File.Create(diffFilePath);
            using var maskImage = ImageSharpCompare.CalcDiffMaskImage(actual, expected);
            ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);

            return false;
        }
    }
}
