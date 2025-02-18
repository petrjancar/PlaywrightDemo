namespace Automation.Utilities.Helpers.Performance;

/// <summary>
/// Model class to store performance metrics.
/// </summary>
public class PerformanceMetrics
{
    public DateTime NavigationStart { get; set; }
    public double DomContentLoadedEventEnd { get; set; }
    public double LoadEventEnd { get; set; }
    public double ResponseStart { get; set; }
    public double ResponseEnd { get; set; }
    public double RequestStart { get; set; }

    public override string ToString()
    {
        return $"Navigation Start DateTime: {NavigationStart} \n" +
               $"DOM Content Loaded: {DomContentLoadedEventEnd} ms\n" +
               $"Load Event End: {LoadEventEnd} ms\n" +
               $"Response Start: {ResponseStart} ms\n" +
               $"Response End: {ResponseEnd} ms\n" +
               $"Request Start: {RequestStart} ms\n";
    }
}
