# PlaywrightDemo
.NET Playwright demo.

## Features
- configuration (`.runsettings` file)
- logging (options: `true`, `false`)
- screenshots (options: `true`, `false`)
- tracing (options: `never`, `on error`, `always`)
- retry logic
- parallel execution (only `ParallelScope.Self` is supported)
- Page Object Model pattern
- additional utilities
- generate report from test results
- easy setup, suitable for CI/CD

## How to use
1. Run tests:
	- `dotnet test --settings .\Automation\Configuration\RunSettings\Demo.runsettings`
2. Generate report:
	- `dotnet run --project .\Reporting\Reporting.csproj -- "C:\TestResults\test.xml"`
