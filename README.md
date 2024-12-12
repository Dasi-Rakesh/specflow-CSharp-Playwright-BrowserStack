# Playwright Test Automation

## Overview
This project automates browser interactions using Playwright with C#. It integrates with BrowserStack for cross-browser testing and SpecFlow for BDD-style test cases.

---

## Setup
1. Install [.NET SDK](https://dotnet.microsoft.com/download).
2. Add `Microsoft.Playwright` and other required NuGet packages.
3. Configure `single.conf.json` with your BrowserStack credentials.

---

## How to Run

### Locally:
```bash
dotnet test
```

On BrowserStack:
```bash
dotnet test --config single.conf.json
```

Key Features
Automates login and other workflows.
Captures screenshots for failed tests.
Marks BrowserStack session status based on test results.
