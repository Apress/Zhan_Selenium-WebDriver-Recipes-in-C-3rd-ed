namespace SeleniumRecipes;

using System.Diagnostics;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
// Replace the version to match the Chrome version
using Network = OpenQA.Selenium.DevTools.V115.Network;

[TestClass]
public class Ch24DevToolsNetworkTest
{
    IWebDriver driver;
    DevToolsSession devToolsSession;

    [TestInitialize]
    public void Before()
    {
        driver  = new ChromeDriver();
        IDevTools devTools = driver as IDevTools;
        devToolsSession = devTools.GetDevToolsSession();
    }

    [TestCleanup]
    public void After()
    {    
        if (driver != null)
            driver.Quit();
    }


   [TestMethod]
    public void TestNetworkInterception() {
        var domains = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
        domains.Network.Enable(new Network.EnableCommandSettings());
        domains.Network.SetBlockedURLs(new Network.SetBlockedURLsCommandSettings()
        {
            Urls = new string[] { "*://*/*.css", "*://*/*.jpg", "*://*/*.png" }
        });
        driver.Url = "https://whenwise.agileway.net";
        System.Threading.Thread.Sleep(1000);
        System.Console.WriteLine("Page is shown, but no image and styles");
    }

   [TestMethod]
    public void TestNetworkLatency() {
        // warm up the server
        driver.Navigate().GoToUrl("https://travel.agileway.net");

        Stopwatch sw1 = new Stopwatch();
        sw1.Start();
        driver.Navigate().GoToUrl("https://travel.agileway.net");
        sw1.Stop();
        long timing1 =  sw1.ElapsedMilliseconds;
        Console.WriteLine("Normal operation: " + timing1 + " (ns)");

        var domains = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
        var networkSettings = new Network.EmulateNetworkConditionsCommandSettings();
        networkSettings.Latency = 3000;
        domains.Network.EmulateNetworkConditions(networkSettings);
        Stopwatch sw2 = new Stopwatch();
        sw2.Start();
        driver.Navigate().GoToUrl("https://travel.agileway.net");
        sw2.Stop();
        long timing2 =  sw2.ElapsedMilliseconds;
        Console.WriteLine("With Latency operation: " + timing2 + " (ns)");
        Assert.IsTrue( timing2 > 3000);
        Assert.IsTrue( timing2 > timing1);
        Assert.IsTrue( (timing2 - timing1) > 2000 ); // not set 3000 to allow some network varitions
    }

}

// research and remove later
// var cookieSettings = new OpenQA.Selenium.DevTools.V115.Network.ClearBrowserCookiesCommandSettings();
// some references :https://stackoverflow.com/questions/76653578/how-to-clear-chrome-site-data-using-devtools-selenium-webdriver-4-c

