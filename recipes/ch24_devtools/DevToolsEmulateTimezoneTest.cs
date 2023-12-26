namespace SeleniumRecipes;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
// Replace the version to match the Chrome version
using OpenQA.Selenium.DevTools.V115.Emulation;

[TestClass]
public class Ch24DevToolsEmulateTimezoneTest
{
    static IWebDriver driver = new ChromeDriver();

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }

    [TestMethod]
    public void TestEmulateTimezone() {
        IDevTools devTools = driver as IDevTools;
        DevToolsSession devToolsSession = devTools.GetDevToolsSession();
        var timezoneSettings = new SetTimezoneOverrideCommandSettings();
        timezoneSettings.TimezoneId = "Asia/Tokyo";
        var domains = devToolsSession.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V115.DevToolsSessionDomains>();
        domains.Emulation.SetTimezoneOverride(timezoneSettings);
        driver.Navigate().GoToUrl("https://whatismytimezone.com");
        System.Threading.Thread.Sleep(1000);
        Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Japan Standard Time"));

    }


}