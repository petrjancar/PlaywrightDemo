using Reporting.Utilities;

namespace Reporting;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length < 1 || args.Length > 2)
        {
            Console.WriteLine("Usage: Reporting <xmlFilePath> [outputHtmlFileName]");
            return;
        }

        string xmlFilePath = args[0];
        if (!File.Exists(xmlFilePath))
        {
            Console.WriteLine("The specified XML file does not exist.");
            return;
        }

        if (args.Length > 1)
        {
            string outputHtmlFileName = args[1];
            await ReportGenerator.GenerateHtmlReport(xmlFilePath, outputHtmlFileName);
        }
        else
        {
            await ReportGenerator.GenerateHtmlReport(xmlFilePath);
        }
    }
}
