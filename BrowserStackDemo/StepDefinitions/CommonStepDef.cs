namespace LLAutomation.StepDefinitions
{
    using LLAutomation.Util;
    using Microsoft.Playwright;
    using NUnit.Framework;
    using System.Text;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class CommonStepDef
    {
        private CommonStepFunction commonstepfunction;
        private IPage _page;
        public readonly string downloadFilePath;


        public CommonStepDef(IPage page, CommonStepFunction common)
        {
            _page = page;
            commonstepfunction = common;
        }


        [When("the user click on element (.*) on page (.*)")]
        public async Task ClickOnElement(string loc, string pagename)
        {
            try
            {
                await commonstepfunction.clickElement(loc, pagename);
                Logger.Info(pagename, "the user click on element" + loc + " on page " + pagename);
            }
            catch (Exception ex)
            {
                await commonstepfunction.TakeScreenshot();
                Logger.Error(pagename, "Not able to click on element" + loc + " on page " + pagename);
                Assert.Fail(ex.Message);
            }

        }

        [Then("the user verify on element (.*) is present on page (.*)")]
        public async Task VerifyElementisPresent(string loc, string pagename)
        {

            try
            {
                await commonstepfunction.VerifyElementIspresent(loc, pagename);
                Logger.Info(pagename, "The user verify on element" + loc + " on page " + pagename);
            }
            catch (Exception ex)
            {
                await commonstepfunction.TakeScreenshot();
                Logger.Error(pagename, "The user verify on element" + loc + " on page " + pagename);
                Assert.Fail(ex.Message);
            }

        }

        [When("the user login into Application")]
        public async Task Login()
        {
            try
            {
                // Ensure _page is initialized
                if (_page == null)
                {
                    throw new Exception("_page is not initialized. Ensure the browser and context are set up before this step.");
                }

                await commonstepfunction.LoginInToApplication();
                Logger.Info("RIQ", "The user is logged in");
                await _page.EvaluateAsync("_ => {}", "browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Step Passed\"}}");
            }
            catch (Exception ex)
            {
                await _page.EvaluateAsync("_ => {}", "browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Step Failed\"}}");

                // Assert failure
                Assert.Fail(ex.Message);
            }
        }


    }
}

