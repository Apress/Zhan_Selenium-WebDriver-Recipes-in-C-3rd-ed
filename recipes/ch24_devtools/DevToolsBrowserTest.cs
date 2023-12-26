namespace SeleniumRecipes;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
// Replace the version to match the Chrome version
using OpenQA.Selenium.DevTools.V115.Emulation;
using OpenQA.Selenium.DevTools.V115.Browser;

[TestClass]
public class Ch24DevToolsBrowserTest
{
    static IWebDriver driver = new ChromeDriver();

    [ClassCleanup]
    public static void AfterAll() {
    }

   [TestMethod]
    public void TestBrowserCrash() {
        driver.Url = "https://whenwise.agileway.net";
        System.Threading.Thread.Sleep(1000);
      
        IDevTools devTools = driver as IDevTools;
        DevToolsSession devToolsSession = devTools.GetDevToolsSession();
        var domains = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
        domains.Browser.Crash();
        System.Console.WriteLine("Still can reach here, browser is gone");
    }

/*
Not working
    [TestMethod]
    public void TestBrowserCrashViaSendCommand() {
        driver.Url = "https://whenwise.agileway.net";
        System.Threading.Thread.Sleep(1000);
      
        IDevTools devTools = driver as IDevTools;
        DevToolsSession devToolsSession = devTools.GetDevToolsSession();
        // https://chromedevtools.github.io/devtools-protocol/tot/
        devToolsSession.SendCommand("Browser.crash");
        System.Threading.Thread.Sleep(1000);
        System.Console.WriteLine("Still can reach here, browser is gone");
    }
*/
}