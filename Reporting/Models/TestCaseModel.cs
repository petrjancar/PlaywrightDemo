namespace Reporting.Models;     

/// <summary>
/// Model representing a test case.
/// </summary>
public class TestCaseModel
{
    public string? Name { get; set; }
    public string? Result { get; set; }
    public string ResultClass => Result switch
    {
        "Passed" => "pass",
        "Failed" => "fail",
        "Skipped" => "skip",
        _ => ""
    };
    public TimeSpan Duration { get; set; }
    public string? Message { get; set; }
    public List<string> Traces { get; set; } = new();
    public List<string> Logs { get; set; } = new();
    public List<string> Screenshots { get; set; } = new();
    public List<string> Videos { get; set; } = new();
}
