namespace SeleniumRecipes;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
// Replace the version to match the Chrome version
using OpenQA.Selenium.DevTools.V115.Emulation;

[TestClass]
public class Ch24DevToolsEmulateLocaleTest
{
    static IWebDriver driver = new ChromeDriver();

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }

   [TestMethod]
    public void TestEmulateLocale() {
        driver.Url = "https://www.localeplanet.com/support/browser.html";
        System.Threading.Thread.Sleep(1000);
        Assert.IsFalse(driver.FindElement(By.TagName("body")).Text.Contains("オーストラリア東部標準時"));

        IDevTools devTools = driver as IDevTools;
        DevToolsSession devToolsSession = devTools.GetDevToolsSession();
        SetLocaleOverrideCommandSettings localeSettings = new SetLocaleOverrideCommandSettings();
        localeSettings.Locale = "ja_JP";
        var domains = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
        domains.Emulation.SetLocaleOverride(localeSettings);
        driver.Navigate().GoToUrl("https://www.localeplanet.com/support/browser.html");
        System.Threading.Thread.Sleep(21000);
        Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("オーストラリア東部標準時"));
    }

}