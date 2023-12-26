namespace SeleniumRecipes;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
// Replace the version to match the Chrome version
using OpenQA.Selenium.DevTools.V115.Emulation;
using OpenQA.Selenium.DevTools.V115.Security;

[TestClass]
public class Ch24DevToolsSecurityTest
{
    static IWebDriver driver = new ChromeDriver();

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }

   [TestMethod]
    public void TestIgnoreCertificateErrors() {
        driver.Url = "https://expired.badssl.com";
        System.Threading.Thread.Sleep(1000);
        Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Your connection is not private"));

        IDevTools devTools = driver as IDevTools;
        DevToolsSession devToolsSession = devTools.GetDevToolsSession();
        SetIgnoreCertificateErrorsCommandSettings securitySettings = new SetIgnoreCertificateErrorsCommandSettings();
        securitySettings.Ignore = true;
        var domains = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
        domains.Security.Enable();
        domains.Security.SetIgnoreCertificateErrors(securitySettings);
        driver.Navigate().GoToUrl("https://expired.badssl.com");
        System.Threading.Thread.Sleep(1000);
        Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("Your connection is not private"));
    }

}