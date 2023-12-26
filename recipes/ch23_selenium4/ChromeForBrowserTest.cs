namespace HelloSeleniumTest;
using OpenQA.Selenium.Chrome;

[TestClass]
public class Ch23SeleniumChromeForBrowserTest
{
    static IWebDriver driver;

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }

    [TestMethod]
    public void TestChromeForTesting() {
        ChromeOptions options = new ChromeOptions();
        options.BrowserVersion = "116";
        IWebDriver driver = new ChromeDriver(options);

        driver.Navigate().GoToUrl("http://www.google.com");
        driver.FindElement(By.Name("q")).SendKeys("Hello Selenium WebDriver!");
        System.Threading.Thread.Sleep(30000);

    }
}