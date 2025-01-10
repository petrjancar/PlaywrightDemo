namespace Reporting.Models;

/// <summary>
/// Model representing a test run.
/// </summary>
public class TestRunModel
{
    public string? TestRunName { get; set; }
    public int TotalTests => TestFixtures.Sum(f => f.TotalTests);
    public int PassedTests => TestFixtures.Sum(f => f.PassedTests);
    public int FailedTests => TestFixtures.Sum(f => f.FailedTests);
    public int SkippedTests => TestFixtures.Sum(f => f.SkippedTests);
    public TimeSpan TotalDuration => TimeSpan.FromSeconds(TestFixtures.Sum(f => f.TotalDuration.TotalSeconds));
    public List<TestFixtureModel> TestFixtures { get; set; } = new();
}
