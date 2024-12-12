using NUnit.Framework;
using BoDi;
using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

[assembly: Parallelizable(ParallelScope.Fixtures)] // Enables parallel execution at the fixture level

namespace BrowserStackDemo.Hooks
{
    [Binding]
    public class Hooks
    {
        public IPage? _page;
        public IBrowser? _browser;
        public IPlaywright _playwright;
        public IBrowserContext? _BrowserContext;
        private static int _capabilitiesIndex = 0;
        private static readonly object _lock = new object();

        private readonly IObjectContainer _objectContainer;
        public readonly ScenarioContext _scenarioContext;
        public static string? reportPath = Directory.GetParent("../../../").FullName + Path.DirectorySeparatorChar + "Reports" + Path.DirectorySeparatorChar + "Report_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + Path.DirectorySeparatorChar;
        protected string? configFile;

        // Centralized credentials
        private static readonly string BrowserStackUsername = "BROWSERSTACK_USERNAME";
        private static readonly string BrowserStackAccessKey = "BROWSERSTACK_ACCESS_KEY";

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;

            // Default the configFile variable
            configFile = Directory.GetParent("../../../").FullName + Path.DirectorySeparatorChar + "Hooks" + Path.DirectorySeparatorChar + "single.conf.json";
        }

        // Before each scenario: Initialize Browser and Playwright for Parallel Execution
        [BeforeScenario]
        public async Task<IPage> InvokeBrowserBeforeScenario()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, configFile);
            JObject config = JObject.Parse(File.ReadAllText(path));

            if (config == null)
                throw new Exception("Configuration not found!");

            // Get the next capabilities set
            Dictionary<string, string> selectedCapabilities;
            lock (_lock)
            {
                ArrayList capabilitiesList = GetCapabilitiesList();
                selectedCapabilities = (Dictionary<string, string>)capabilitiesList[_capabilitiesIndex];
                _capabilitiesIndex = (_capabilitiesIndex + 1) % capabilitiesList.Count;
            }

            string capsJson = JsonConvert.SerializeObject(selectedCapabilities);
            string cdpUrl = "wss://cdp.browserstack.com/playwright?caps=" + Uri.EscapeDataString(capsJson);

            // Initialize Playwright for browser interaction
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.ConnectAsync(cdpUrl);
            _BrowserContext = await _browser.NewContextAsync(new()
            {
                ViewportSize = new ViewportSize() { Width = 1920, Height = 1080 }
            });

            await _BrowserContext.GrantPermissionsAsync(new[] { "clipboard-read", "clipboard-write", "geolocation" });

            _page = await _BrowserContext.NewPageAsync();
            await _page.GotoAsync("https://www.linklogistics.com/");

            // Register instances for dependency injection
            _objectContainer.RegisterInstanceAs(_page);
            _objectContainer.RegisterInstanceAs(_browser);
            _objectContainer.RegisterInstanceAs(_BrowserContext);

            return _page;
        }

        // After each scenario: Cleanup Browser and Playwright resources
        [AfterScenario]
        public async Task AfterScenario()
        {
            if (_BrowserContext != null)
            {
                await _BrowserContext.CloseAsync();
            }
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
            if (_playwright != null)
            {
                _playwright.Dispose();
            }
        }

        // Before the test run: Optional clean up of previous reports or setup
        [BeforeTestRun(Order = 1)]
        public static void DeleteAllPreviousReports()
        {
            try
            {
                string reportsPath = Directory.GetParent("../../../").FullName + Path.DirectorySeparatorChar + "Reports";
                if (Directory.Exists(reportsPath))
                {
                    Directory.Delete(reportsPath, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during BeforeTestRun: {ex.Message}");
            }
        }

        private static ArrayList GetCapabilitiesList()
        {
            ArrayList capabilitiesList = new ArrayList();

            Dictionary<string, string> catalinaChromeCap = new Dictionary<string, string>
            {
                { "browser", "chrome" },
                { "browser_version", "latest" },
                { "os", "osx" },
                { "os_version", "catalina" },
                { "name", "Branded Google Chrome on Catalina" },
                { "build", "playwright-dotnet-3" },
                { "browserstack.username", BrowserStackUsername },
                { "browserstack.accessKey", BrowserStackAccessKey }
            };
            capabilitiesList.Add(catalinaChromeCap);

            Dictionary<string, string> catalinaEdgeCap = new Dictionary<string, string>
            {
                { "browser", "edge" },
                { "browser_version", "latest" },
                { "os", "osx" },
                { "os_version", "catalina" },
                { "name", "Branded Microsoft Edge on Catalina" },
                { "build", "playwright-dotnet-3" },
                { "browserstack.username", BrowserStackUsername },
                { "browserstack.accessKey", BrowserStackAccessKey }
            };
            capabilitiesList.Add(catalinaEdgeCap);

                       Dictionary<string, string> catalinaChromiumCap = new Dictionary<string, string>
            {
                { "browser", "playwright-chromium" },
                { "os", "osx" },
                { "os_version", "catalina" },
                { "name", "Playwright chromium on Catalina" },
                { "build", "playwright-dotnet-3" },
                { "browserstack.username", BrowserStackUsername },
                { "browserstack.accessKey", BrowserStackAccessKey }
            };
            capabilitiesList.Add(catalinaChromiumCap);

            return capabilitiesList;
        }

        public static async Task MarkTestStatus(string status, string reason, IPage page)
        {
            await page.EvaluateAsync("_ => {}", $"browserstack_executor: {{\"action\": \"setSessionStatus\", \"arguments\": {{\"status\":\"{status}\", \"reason\": \"{reason}\"}}}}");
        }
    }
}
