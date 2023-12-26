namespace HelloSeleniumTest;
using OpenQA.Selenium.Chrome;

[TestClass]
public class SeleniumLoginTests
{
    static IWebDriver driver;

        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>

    [ClassInitialize]
    public static void BeforeAll(TestContext context)
    {
        driver =  new ChromeDriver();
    }
      
    [TestInitialize]
    public void Before()
    {
        driver.Navigate().GoToUrl("http://travel.agileway.net");
    }


    [ClassCleanup]
    public static void AfterAll() {
        driver.Quit();
    }

    [TestMethod]
    public void TestLoginOK() 
    {
        driver.FindElement(By.Name("username")).SendKeys("agileway");
        driver.FindElement(By.Name("password")).SendKeys("testwise");
        driver.FindElement(By.Name("password")).Submit();
        Assert.IsTrue(driver.PageSource.Contains("Signed in!"));
        driver.FindElement(By.LinkText("Sign off")).Click();
    }


    [TestMethod]
    public void TestLoginFailed() 
    {
        driver.FindElement(By.Name("username")).SendKeys("agileway");
        driver.FindElement(By.Name("password")).SendKeys("badpass");
        driver.FindElement(By.Name("password")).Submit();
        Assert.IsTrue(driver.PageSource.Contains("Invalid email or password"));
    }
}