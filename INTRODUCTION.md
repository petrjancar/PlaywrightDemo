# Introduction to the PlaywrightDemo Framework

Welcome to the `PlaywrightDemo` project! This document provides an overview of the .NET test automation framework built using Playwright. It's designed not just as a demonstration but as a robust, feature-rich foundation for UI testing.

**Purpose:**

- Demonstrate best practices for building a scalable UI test automation framework with Playwright in a .NET (C#) environment.
- Provide a practical example integrating various essential testing features like logging, reporting, configuration management, different test types (functional, visual, performance, accessibility), and robust execution control.
- Serve as a learning resource and a starting point for developing production-grade UI test suites.

**Target Audience:**

This document is for developers or QA engineers who want to understand the structure, concepts, and capabilities of this framework, potentially to use it, or learn from its design. A basic understanding of C#, .NET, and general test automation concepts is assumed.

---

## Core Technologies

The framework leverages the following key technologies:

- **.NET 8 / C#:** The primary development platform and language.
- **Playwright .NET:** The core browser automation library enabling cross-browser (Chromium, Firefox, WebKit) UI interactions.
- **NUnit:** The test execution framework (`[TestFixture]`, `[Test]`, `[SetUp]`, `[TearDown]`, assertions).
- **Serilog:** A structured logging library used for detailed test execution logging.
- **Microsoft.Extensions.DependencyInjection:** (Optional - Shown as an improvement idea, but the base framework relies on manual instantiation).
- **Deque.AxeCore.Playwright:** For running accessibility scans within tests.
- **Codeuctivity.ImageSharpCompare:** For performing visual regression testing by comparing screenshots.
- **RazorLight:** Used in the separate `Reporting` project to generate HTML reports from test results.

---

## Key Features

This framework offers a comprehensive set of features out-of-the-box:

- **Page Object Model (POM):** Well-structured and enhanced POM implementation for maintainability.
- **Element Abstraction:** Reusable classes for common HTML elements (Button, TextBox, etc.).
- **Flexible Configuration:** Test run parameters (browser, headless, timeouts, feature flags) managed via `.runsettings` files.
- **Structured Logging:** Detailed, per-test log files using Serilog, including options to log Playwright requests/responses/errors.
- **Artifact Generation:** Automatic creation and management of:
  - **Screenshots:** On demand or potentially on failure.
  - **Video Recordings:** Configurable (Always, OnFail, Never).
  - **Playwright Traces:** Detailed execution traces (Always, OnFail, Never) viewable on `trace.playwright.dev`.
- **Custom Retry Logic:** Robust retry mechanism (`UIRetryAttribute`) specifically designed for flaky UI tests, retrying on various failure/error states.
- **Multiple Test Types:** Examples and support for:
  - Functional Tests
  - Visual Comparison Tests
  - Performance Metrics Collection
  - Accessibility Scans (AxeCore)
- **Custom HTML Reporting:** A dedicated project parses NUnit results (`.trx` file) and generates an HTML report linking relevant artifacts (logs, videos, traces, screenshots) to each test result.
- **Clean Setup/Teardown:** Centralized logic in base classes for managing browser contexts, pages, and associated features like logging/tracing/video.

---

## Project Structure

The solution (`PlaywrightDemo.sln`) is organized into two main projects:

1. **`Automation` Project:** Contains the core framework logic and the tests themselves.

    - `Configuration/`: Manages test run settings, logging, tracing, video, etc.
      - `RunSettings/`: Example `.runsettings` files.
      - `Logging/`, `Tracing/`, `VideoRecording/`: Managers for these features.
      - `Settings.cs`: Central class to access configured settings.
    - `Model/`: Defines the Page Object Model structure.
      - `Elements/`: Reusable abstractions for HTML elements (`BaseElement`, `Button`, `TextBox`, etc.).
      - `PageFragments/`: Reusable UI components composed of elements (e.g., `Navigation`).
      - `PageObjects/`: Represents individual pages of the application (`BasePage`, `AdminLoginPage`, etc.), often using partial classes (`.cs`, `.Methods.cs`, `.Asserts.cs`).
      - `Environment/`: Test data like `Credentials.cs`.
    - `Tests/`: Contains the actual NUnit test fixtures and tests.
      - `BaseTest.cs`: Base class for all test fixtures, handling setup/teardown.
      - `GlobalSetup.cs`: Handles one-time setup/teardown for the entire test run.
      - Specific test files (e.g., `LoginTests.cs`, `LoginVisualTests.cs`).
    - `Utilities/`: Helper classes and attributes.
      - `Attributes/`: Custom attributes like `UIRetryAttribute`.
      - `Helpers/`: Utilities for screenshots, image comparison, performance metrics.
      - `Waiters/`: Custom waiting logic (`PollingWaiter`).
      - `TestRunContext.cs`: Helper to get current test name/fixture.

2. **`Reporting` Project:** A separate console application responsible for generating the HTML report.
    - `Models/`: Data models representing the test run structure (TestRun, TestFixture, TestCase).
    - `Utilities/`: Contains the `ReportGenerator` logic.
    - `Views/`: Contains the Razor template (`.cshtml`) for the HTML report.
    - `Program.cs`: Entry point for the reporting tool.

---

## Core Concepts Explained

- **Page Object Model (POM):**

  - **Purpose:** Encapsulates UI pages/components into classes, separating test logic from UI interaction details. Improves maintainability and reduces code duplication.
  - **Implementation:**
    - `BasePage`: Abstract base class providing common page functionalities (GoTo, ExpectUrl, Screenshot, Axe Scan, Performance).
    - Specific Pages (e.g., `AdminLoginPage`): Inherit `BasePage`, define `Url`, contain element locators/abstractions, and methods representing user actions (e.g., `LoginAsync`). Uses `partial` classes to organize elements, methods, and page-specific assertions.
    - **Element Abstraction:** (`Model/Elements`) Instead of using `IPage.Locator()` directly in pages, we use wrapper classes (`Button`, `TextBox`, etc.) inheriting `BaseElement`. This adds a layer of abstraction, allows common element actions (like logging, custom waits), and makes page code cleaner.
    - **Page Fragments:** (`Model/PageFragments`) Reusable UI sections (like `Navigation`) are built as separate classes (often inheriting `BaseFragment`) and composed within Page Objects.

- **Base Test & Lifecycle (`BaseTest.cs`, `GlobalSetup.cs`):**

  - `BaseTest` inherits `PageTest` from `Microsoft.Playwright.NUnit`, providing managed `Page`, `Context`, `Browser`, `Playwright` instances per test worker.
  - `[SetUp]` (Runs before _each_ test): Initializes test-specific logging (`LoggingManager`), starts tracing (`TracingManager`).
  - `[TearDown]` (Runs after _each_ test): Stops tracing (saving conditionally), stops video recording (saving/deleting conditionally), closes the test logger.
  - `ContextOptions()`: Overridden method dynamically adds video recording settings _before_ the `BrowserContext` is created based on configuration.
  - `GlobalSetup`: Uses `[OneTimeSetUp]` to prepare the environment (e.g., clearing the `ResultsDirectory`) before _any_ tests run.

- **Configuration (`Settings.cs`, `.runsettings`):**

  - Test run parameters (Browser, Headless, Logging, Tracing, Video, Screenshots, RetryCount, ResultsDirectory) are defined in `.runsettings` files.
  - NUnit makes these parameters available via `TestContext.Parameters`.
  - `Settings.cs` reads these parameters in its static constructor, providing strongly-typed static properties to access settings throughout the framework with sensible defaults. This offers great flexibility without code changes.

- **Logging (`LoggingManager.cs`):**

  - Uses Serilog for structured logging.
  - Creates a separate log file _per test_, named with the test name and timestamp, organized under `ResultsDirectory/Logs/{FixtureName}/`.
  - Can automatically log Playwright network requests, responses, and page errors based on boolean flags in `LoggingManager`.
  - Provides a static `LogMessage` method for custom logging from anywhere in the framework.

- **Artifacts & Reporting:**

  - **Tracing/Video/Screenshots:** Managed by respective manager classes (`TracingManager`, `VideoRecordingManager`, `ScreenshotHelper`). They handle creation, conditional saving based on settings (`Always`/`OnFail`) and test outcome, and naming/organization within the `ResultsDirectory`.
  - **Reporting (`Reporting` project):** This standalone tool reads the `.trx` XML output from `dotnet test`. It parses results and uses `RazorLight` to render `ReportView.cshtml` into a final `report.html`. Crucially, `ReportGenerator` locates the associated artifact files (logs, traces, videos, screenshots) based on naming conventions and includes relative links to them in the HTML report, creating a central dashboard for analyzing test results.

- **Retry Mechanism (`UIRetryAttribute.cs`):**
  - A custom NUnit attribute applied to test methods (`[UIRetry]`).
  - It overrides NUnit's default retry logic to rerun tests not only on `Failure` but also on various `Error` states (including Setup/Teardown errors), which is vital for handling the inherent flakiness of UI interactions or environment issues.
  - The number of retries is controlled by `Settings.RetryCount`.

---

## Running Tests & Generating Reports

(As per `README.md`)

1. **Run Tests:** Execute tests using `dotnet test`, specifying a configuration file:

    ```bash
    dotnet test --settings .\Automation\Configuration\RunSettings\demo-chromium.runsettings
    ```

    (Ensure the paths in the `.runsettings` file, like `LogFileName` and `ResultsDirectory`, are valid for your environment).

2. **Generate Report:** After tests complete, run the `Reporting` project, passing the path to the generated test results XML file (default location specified in `demo-chromium.runsettings` is `C:\TestResults\test.xml`):

    ```bash
    dotnet run --project .\Reporting\Reporting.csproj -- "C:\TestResults\test.xml"
    ```

    This will generate `report.html` in the same directory as the `.xml` file (`C:\TestResults` in this example).

---

## Underlying Principles & Ideas

- **Maintainability:** Achieved through strict POM, element abstraction, and clear project structure. Changes in the UI should ideally only require updates in the corresponding Page Object or Element class.
- **Reusability:** Base classes, page fragments, element abstractions, and utility helpers promote code reuse.
- **Configurability:** `.runsettings` allows easy adaptation to different browsers, environments, and execution options without code changes.
- **Robustness:** The custom retry logic, detailed logging, and artifact generation (traces, videos) significantly aid in diagnosing and handling flaky tests or failures.
- **Readability:** Consistent naming, use of C# features, and clear separation of concerns aim for easily understandable code.
- **Comprehensiveness:** Integration of various test types (functional, visual, etc.) and supporting features provides a complete testing solution.

---

## Getting Started

To familiarize yourself further:

1. Explore `Settings.cs` and a `.runsettings` file to understand configuration.
2. Study `BaseTest.cs` and `GlobalSetup.cs` to see the test execution lifecycle.
3. Examine a specific Page Object (e.g., `AdminLoginPage.cs`) and its associated Element/Fragment classes (`Button.cs`, `Navigation.cs`).
4. Look at a test file (e.g., `LoginTests.cs`) to see how Page Objects are used and assertions are made.
5. Run the tests and generate the report to see the output and artifacts.

---
