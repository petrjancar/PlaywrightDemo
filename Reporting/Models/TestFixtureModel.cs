namespace Reporting.Models;

/// <summary>
/// Model representing a test fixture.
/// </summary>
public class TestFixtureModel
{
    public string? Name { get; set; }
    public int TotalTests => TestCases.Count;
    public int PassedTests => TestCases.Count(tc => tc.Result == "Passed");
    public int FailedTests => TestCases.Count(tc => tc.Result == "Failed");
    public int SkippedTests => TestCases.Count(tc => tc.Result == "NotExecuted");
    public TimeSpan TotalDuration => TimeSpan.FromSeconds(TestCases.Sum(tc => tc.Duration.TotalSeconds));
    public List<TestCaseModel> TestCases { get; set; } = new();
}
