namespace SeleniumRecipes;
using OpenQA.Selenium.Chrome;

[TestClass]
public class Ch23Selenium4NewTest
{
    static WebDriver driver;

   [TestCleanup]
    public void After()
    {    
        if (driver != null)
            driver.Quit();
    }

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }

     [TestMethod]
    public void TestScreenshotElement() {

        ChromeOptions options = new ChromeOptions();
        IWebDriver driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/coupon.html");
        driver.FindElement(By.Id("get_coupon_btn")).Click();
        System.Threading.Thread.Sleep(1000);
        WebElement elem = (WebElement)  driver.FindElement(By.Id("details"));
        Screenshot screenshot =  elem.GetScreenshot();
        screenshot.SaveAsFile("/tmp/coupon.png", ScreenshotImageFormat.Png);
    }

    [TestMethod]
    public void TestElementLocation() {
        ChromeOptions options = new ChromeOptions();
        IWebDriver driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/image-map.html");
        IWebElement elem = driver.FindElement(By.Id("agileway_software"));
        Console.WriteLine(elem.Location.X);
        Console.WriteLine(elem.Location.Y);
        Console.WriteLine(elem.Size.Height);
        Console.WriteLine(elem.Size.Width);

       // puts elem.attribute("src")  # v3
       Assert.AreEqual("images/agileway_software.png",elem.GetDomAttribute("src") );
    }
 

}