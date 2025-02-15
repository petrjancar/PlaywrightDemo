using Automation.Configuration;

namespace Automation.Tests;

/// <summary>
/// GlobalSetup class is the global setup class for the project.
/// It contains the OneTimeSetUp and OneTimeTearDown methods that are executed once 
/// before and after all test fixtures.
/// </summary>
[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        SetUpResultDirectory();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {

    }

    private static void SetUpResultDirectory()
    {
        if (Directory.Exists(Settings.ResultsDirectory))
        {
            DirectoryInfo directory = new DirectoryInfo(Settings.ResultsDirectory);
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo subdirectory in directory.EnumerateDirectories())
            {
                subdirectory.Delete(true);
            }
        }
        else
        {
            Directory.CreateDirectory(Settings.ResultsDirectory);
        }
    }
}
