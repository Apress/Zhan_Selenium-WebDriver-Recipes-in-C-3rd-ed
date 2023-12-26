namespace SeleniumRecipes;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
// Replace the version to match the Chrome version
using OpenQA.Selenium.DevTools.V115.Emulation;
using OpenQA.Selenium.DevTools.V115.Page;

[TestClass]
public class Ch24DevToolsPageTest
{
    static IWebDriver driver = new ChromeDriver();

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }

    [TestMethod]
    public void TestConsoleLog() {
        IDevTools devTools = driver as IDevTools;
        DevToolsSession devToolsSession = devTools.GetDevToolsSession();
        System.Threading.Thread.Sleep(1000);
        var domains  = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
//        domains.Page.Enable();
  //      domains.Page.Navigate("https://whenwise.agileway.net");
    //    domains.Console.ClearMessages();

      }

   [TestMethod]
    public void TestPrintPDF() {
        IDevTools devTools = driver as IDevTools;
        DevToolsSession devToolsSession = devTools.GetDevToolsSession();
        PrintToPDFCommandSettings printPdfSettings = new PrintToPDFCommandSettings();
        driver.Navigate().GoToUrl("https://whenwise.agileway.net");
        System.Threading.Thread.Sleep(1000);
        var domains  = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
        // TODO: how to get data out
        domains.Page.PrintToPDF(printPdfSettings);
    }

}