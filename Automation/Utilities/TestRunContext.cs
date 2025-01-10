namespace Automation.Utilities;

/// <summary>
/// Provides information about the current test run.
/// </summary>
public static class TestRunContext
{
    public static string? TestFixture => TestContext.CurrentContext.Test.ClassName;
    public static string? TestName => TestContext.CurrentContext.Test.MethodName;
}
