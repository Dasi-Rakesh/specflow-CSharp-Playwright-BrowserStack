namespace LLAutomation.Util
{
    using LLAutomation.Pages;
    using Microsoft.Playwright;
    using Microsoft.Playwright.NUnit;
    using NUnit.Framework;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Text.Json.Nodes;
    using System.Text.RegularExpressions;
    using System.Threading;
    using TechTalk.SpecFlow;
    

    public class CommonStepFunction : PageTest
    {
        public static IPlaywright? _playwright;
        public static string? scresnhotimagename = null;
        private readonly string workingDirectory;
        private readonly string projectDirectory;
        private readonly string configFilePath;
        private readonly string EnvConfigpath;
        private readonly string DirectnavigateURL;
        public static string getEnvFromvariable;
        private readonly IPage _page;
        public readonly ScenarioContext _scenarioContext;
        public readonly string downloadFilePath;
        private List<string> downloadedFiles = new List<string>();
        private List<string> fileDownloadSession = new();

        public CommonStepFunction(IPage page, ScenarioContext scenariocontext)
        {
            _scenarioContext = scenariocontext;
            _page = page;
            workingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); ;             // This will get the current PROJECT directory
            projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            configFilePath = projectDirectory + Path.DirectorySeparatorChar + "configuration.json";
        }

        public CommonStepFunction()
        {
            workingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); ;             // This will get the current PROJECT directory
            projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            configFilePath = projectDirectory + Path.DirectorySeparatorChar + "configuration.json";

        }

        public PageMapping pagemapping = new PageMapping();
        private readonly Random _random = new Random();
        private static int Timeout;
        private static int FileDownlodingTimeout;
        public async Task clickElement(String locator, string pagename)
        {
            string locatorvalue = pagemapping.AttemptToFindElement(pagename, locator);
            await waitForElementAttached(locatorvalue);
            await _page.ClickAsync(locatorvalue, new() { Timeout = Timeout });
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        }

        public async Task VerifyElementIspresent(String locator, string pagename)
        {
            await WaitUntilLocatorIsPresent(locator, pagename);
        }
        public async Task WaitUntilLocatorIsPresent(String locator, string pagename)

        {

            await _page.Locator(pagemapping.AttemptToFindElement(pagename, locator)).WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = Timeout });

        }
        public async Task LoginInToApplication()
        {

            Console.WriteLine("Test Passed");
        }

        public async Task waitForElementAttached(String locator)
        {
            Timeout = (Timeout == 0) ? Timeout = 20000 : Timeout;
            await _page.WaitForSelectorAsync(locator, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = Timeout });
            await _page.Locator(locator).IsEnabledAsync(new LocatorIsEnabledOptions { Timeout = Timeout });
        }

        public async Task TakeScreenshot()
        {
            DateTimeOffset datetime = (DateTimeOffset)DateTime.UtcNow;
            string? scenarioname = _scenarioContext.ScenarioInfo.Title.ToLower().Trim();
            string time = datetime.ToString("yyyyMMddHHmmssfff");
            string ddd = System.AppDomain.CurrentDomain.BaseDirectory;
            scenarioname = scenarioname.Replace(" ", "");
           // scresnhotimagename = Hooks.reportPath + "screenshot/" + scenarioname + ".png";
            if (scresnhotimagename != null)
            {
                await _page.ScreenshotAsync(new()
                {
                    Path = scresnhotimagename,
                    FullPage = true,
                });
                TestContext.AddTestAttachment(scresnhotimagename);
            }
        }
    }
}

