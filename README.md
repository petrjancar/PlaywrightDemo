# PlaywrightDemo
.NET Playwright demo.

## Features
- configuration (`.runsettings` file)
- logging (options: `true`, `false`)
- screenshots (options: `true`, `false`)
- video recording (options: `Never`, `OnFail`, `Always`)
- tracing (options: `Never`, `OnFail`, `Always`)
- retry logic
- visual comparisons
- performance metrics
- Page Object Model pattern
- generate report from test results
- easy setup, suitable for CI/CD

## Limitations
- parallel execution:
	- only `ParallelScope.Self` on TestFixture-s is supported (Playwright limitation)
	- when tests are run in parallel, logging is not working as expected

## Dependencies
- [Codeuctivity.ImageSharpCompare](https://www.nuget.org/packages/Codeuctivity.ImageSharpCompare/)

## How to use
1. Run tests:
	- `dotnet test --settings .\Automation\Configuration\RunSettings\Demo.runsettings`
2. Generate report:
	- `dotnet run --project .\Reporting\Reporting.csproj -- "C:\TestResults\test.xml"`
