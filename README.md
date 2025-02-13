# PlaywrightDemo
.NET Playwright demo.

## Features
- configuration (`.runsettings` file)
- logging (options: `true`, `false`)
- screenshots (options: `true`, `false`)
- tracing (options: `Never`, `OnFail`, `Always`)
- retry logic
- Page Object Model pattern
- additional utilities
- generate report from test results
- easy setup, suitable for CI/CD

## Limitations
- video recording support is not implemented
- parallel execution:
	- only `ParallelScope.Self` is supported (Playwright limitation)
	- when tests are run in parallel, logging is not working as expected

## How to use
1. Run tests:
	- `dotnet test --settings .\Automation\Configuration\RunSettings\Demo.runsettings`
2. Generate report:
	- `dotnet run --project .\Reporting\Reporting.csproj -- "C:\TestResults\test.xml"`
