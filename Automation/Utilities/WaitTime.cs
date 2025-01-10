namespace Automation.Utilities;

/// <summary>
/// Wait times for the automation tests.
/// </summary>
public static class WaitTime
{
    public static readonly TimeSpan ExtraShort = TimeSpan.FromSeconds(5);
    public static readonly TimeSpan Short = TimeSpan.FromSeconds(10);
    public static readonly TimeSpan Medium = TimeSpan.FromSeconds(30);
    public static readonly TimeSpan Long = TimeSpan.FromMinutes(1);
    public static readonly TimeSpan ExtraLong = TimeSpan.FromMinutes(5);
}
