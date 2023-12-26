namespace SeleniumRecipes;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

[TestClass]
public class Ch25GridTest
{    
    [TestMethod]
    public void TestSeleniumGridChrome() {
        // server started with 
        //    java -jar selenium-server-4.11.0.jar hub
        // and at least one grid node up running 
        ChromeOptions opts = new ChromeOptions();
        IWebDriver driver = new RemoteWebDriver(new Uri("http://192.168.1.27:4444/"), opts);
        driver.Navigate().GoToUrl("https://whenwise.agileway.net");
        Assert.AreEqual("WhenWise - Booking Made Easy", driver.Title);
        driver.Quit();
    }

    [TestMethod]
    public void TestSeleniumGridHubFirefox() {
        FirefoxOptions opts = new FirefoxOptions();
        opts.PlatformName = "macOS";
        IWebDriver driver = new RemoteWebDriver(new Uri("http://192.168.1.27:4444/"), opts);
        driver.Navigate().GoToUrl("https://whenwise.agileway.net");
        Assert.AreEqual("WhenWise - Booking Made Easy", driver.Title);

        driver.Quit();
    } 
}