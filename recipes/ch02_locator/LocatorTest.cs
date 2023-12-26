namespace SeleniumRecipes;
using OpenQA.Selenium.Chrome;
using static OpenQA.Selenium.RelativeBy;
using System.Collections.ObjectModel;

[TestClass]
public class Ch02LocatorTest
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
        driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/locators.html");
    }


    [ClassCleanup]
    public static void AfterAll() {
        driver.Quit();
    }

    [TestMethod]
    public void TestGetURL() {
        driver.Url = "https://agileway.com.au/demo";
    }
   
    [TestMethod]
    public void TestByID() {
        driver.FindElement(By.Id("submit_btn")).Click();
    }

    [TestMethod]
    public void TestByName() {
        driver.FindElement(By.Name("comment")).SendKeys("Selenium Cool");
    }

    [TestMethod]
    public void TestByLinkedText() {
        driver.FindElement(By.LinkText("Cancel")).Click();
    }

        [TestMethod]
        public void TestByXPath() {
            driver.FindElement(By.XPath("//*[@id='div2']/input[@type='checkbox']")).Click();
        }
    
        [TestMethod]
        public void TestByTag() {
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Selenium Locators"));
        }

    
        [TestMethod]
        public void TestByClassName() {
            driver.FindElement(By.ClassName("btn-primary")).Click();        
             System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.ClassName("btn")).Click();
    
            // the below will return error "Compound class names not permitted"
            // driver.FindElement(By.className("btn btn-deault btn-primary")).Click()
        }
  
        [TestMethod]
        public void TestByCSSSelector() {    
            driver.FindElement(By.CssSelector("#div2 > input[type='checkbox']")).Click();
        }

        [TestMethod]
        public void TestByRelative()
        {
            // Locate the comment box above the "submit" button
            IWebElement submitButton = driver.FindElement(By.ClassName("btn"));
            IWebElement textField = driver.FindElement(RelativeBy.WithLocator(By.Name("comment")).Above(submitButton));
            textField.SendKeys("Selenium is cool");
        }
  
        [TestMethod]
        public void TestFindMultipleElements() {
            ReadOnlyCollection<IWebElement> checkbox_elems = driver.FindElements(By.XPath("//div[@id='container']//input[@type='checkbox']"));
            System.Console.WriteLine(checkbox_elems); // => 2
            checkbox_elems[1].Click();
        }

        [TestMethod]
        public void TestChainFindElement()
        {
            driver.FindElement(By.Id("div2")).FindElement(By.Name("same")).Click();
        }

}