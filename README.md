# PlaywrightDemo (.NET)

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build & Test](https://github.com/petrjancar/PlaywrightDemo/actions/workflows/playwright.yml/badge.svg)](https://github.com/petrjancar/PlaywrightDemo/actions/workflows/playwright.yml)

A comprehensive demonstration framework showcasing advanced UI test automation using **Playwright with .NET (C#) and NUnit**.

This project serves as a practical example and learning resource, illustrating best practices for building robust, maintainable, and feature-rich test suites.

This project uses Alan Richardson's ["Evil Tester" Simple Todo List](https://eviltester.github.io/simpletodolist/adminlogin.html) as the application under test. It serves as a practical example and learning resource.

## ✨ Features

This framework comes packed with features essential for effective UI testing:

- **Solid Foundation:**
  - Advanced Page Object Model (POM) for clean UI interaction logic.
  - Reusable Element Abstractions (Button, TextBox, etc.) and Page Fragments (e.g. Navigation).
  - Core Base Classes (BaseTest, BasePage) simplifying test setup and common page actions.
- **Flexible Configuration:**
  - Easily configure test runs (browser, headless mode, timeouts, etc.) via `.runsettings` files.
  - Centralized `Settings.cs` for accessing configuration values.
- **Rich Diagnostics & Artifacts:**
  - Detailed logging per test using **Serilog**.
  - Configurable **Playwright Tracing** (Always, OnFail, Never) for easy debugging via `trace.playwright.dev`.
  - Configurable **Video Recording** (Always, OnFail, Never).
  - On-demand **Screenshot** generation for pages or elements.
- **Robust Execution:**
  - Custom **Retry Logic** (`UIRetryAttribute`) specifically designed for flaky UI tests.
  - Support for parallel execution via NUnit attributes.
- **Diverse Test Capabilities:**
  - Examples of **Functional**, **Visual Comparison** (using ImageSharpCompare), **Accessibility** (using AxeCore), and **Performance** metric tests.
- **Custom HTML Reporting:**
  - Standalone reporting tool generates a detailed HTML report from NUnit results.
  - Automatically **links artifacts** (logs, videos, traces, screenshots) directly to test results in the report.

## ⚙️ Continuous Integration (CI)

- **GitHub Actions** are used to automatically build and run the tests on push/pull requests to the `main` branch.
- The workflow (`.github/workflows/playwright.yml`) performs:
  - Code checkout
  - .NET setup
  - Playwright browser installation
  - Dependency restoration
  - Project build
  - Test execution (using configured `.runsettings`)
- You can view the status and logs of the CI runs in the [Actions tab](https://github.com/petrjancar/PlaywrightDemo/actions).

## 🛠️ Tech Stack

- **.NET** / **C#**
- **Playwright .NET:** Core browser automation library.
- **NUnit:** Test framework and runner.
- **Serilog:** Structured logging.
- **Deque.AxeCore.Playwright:** Accessibility testing engine.
- **Codeuctivity.ImageSharpCompare:** Visual comparison library.
- **RazorLight:** HTML templating engine for the reporting project.

## 📂 Project Structure

- **`Automation/`**: Contains the core test framework, page objects, tests, and utilities.
- **`Reporting/`**: A separate console application to parse test results and generate the HTML report.

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed.
- Ensure Playwright browsers are installed (run `pwsh bin/Debug/net8.0/playwright.ps1 install` or `dotnet build` in the `Automation` project directory after cloning).

### Running Tests

The tests target the public ["Evil Tester" Simple Todo List](https://eviltester.github.io/simpletodolist/adminlogin.html).

1. **Clone the repository:**

   ```bash
   git clone https://github.com/petrjancar/PlaywrightDemo.git
   cd PlaywrightDemo
   ```

2. **Run tests using `dotnet test`:**
   Specify a `.runsettings` file for configuration (examples provided in `Automation/Configuration/RunSettings/`).

   ```bash
   # Example using Chromium, non-headless
   dotnet test --settings Automation/Configuration/RunSettings/demo-chromium.runsettings
   ```

   _Note: Adjust paths in `.runsettings` (like `LogFileName` and `ResultsDirectory`) if needed for your environment._

### Generating the Report

1. After the test run completes, generate the HTML report using the `Reporting` project. Pass the path to the test result XML file (e.g., `test.xml` specified in the `runsettings`).

   ```bash
   # Assuming default ResultsDirectory C:\TestResults from demo-chromium.runsettings
   dotnet run --project Reporting/Reporting.csproj -- "C:\TestResults\test.xml"
   ```

2. Open the generated `report.html` (located in the same directory as the `.xml` file, e.g., `C:\TestResults`) in your browser.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

_Application Under Test: [Evil Tester Simple Todo List](https://eviltester.github.io/simpletodolist/adminlogin.html) by Alan Richardson._
