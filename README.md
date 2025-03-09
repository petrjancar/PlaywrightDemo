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
- accessibility tests
- performance metrics
- Page Object Model pattern
- generate report from test results
- easy setup, suitable for CI/CD

## Limitations

- parallel execution:
  - only `ParallelScope.Self` on TestFixture-s is supported (Playwright limitation)
  - each test must have a unique name

## Dependencies

- [Codeuctivity.ImageSharpCompare](https://www.nuget.org/packages/Codeuctivity.ImageSharpCompare/) - only for visual comparisons
- [Deque.AxeCore.Playwright](https://www.nuget.org/packages/Deque.AxeCore.Playwright) - only for accessibility tests

## How to use

1. Run tests:
   - `dotnet test --settings .\Automation\Configuration\RunSettings\demo-chromium.runsettings`
2. Generate report:
   - `dotnet run --project .\Reporting\Reporting.csproj -- "C:\TestResults\test.xml"`
