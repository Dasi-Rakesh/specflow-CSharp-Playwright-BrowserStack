namespace LLAutomation.Pages
{
    using System.Reflection;
    public class PageMapping
    {
        Dictionary<string, object> AllPage;
        static Type type = null;
        public Dictionary<string, object> getpageobject()
        {
            Dictionary<string, object> pagename = new Dictionary<string, object>();
            pagename.Add("LoginPage", new LoginPageObject());
            return pagename;
        }
        public object Getpagename(string pagename1)
        {
            return null;
        }
        public string AttemptToFindElement(string pagename, string element)
        {
            return "string";
        }
    }
}