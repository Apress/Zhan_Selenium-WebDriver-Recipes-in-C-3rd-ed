

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

using System.Collections.ObjectModel;

namespace SeleniumRecipes
{
    [TestClass]
    public class Ch09NavigationTest
    {

        static IWebDriver driver = new ChromeDriver();
       
        String siteRootUrl = "https://agileway.com.au";

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/index.html");
        }

        [TestCleanup]
        public void After()
        {
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            driver.Quit();
        }

        [TestMethod]
        public void TestGotoURL()
        {
            driver.Navigate().GoToUrl("https://bing.com");
        }

        [TestMethod]
        public void TestSetURL()
        {
            driver.Url = "https://agileway.com.au";
        }

        [TestMethod]
        public void TestBackRefreshForward()
        {
            driver.Navigate().Back();
            driver.Navigate().Refresh();
            driver.Navigate().Forward();
        }

        [TestMethod]
        public void TestResizeWindow()
        {
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);
        }

        [TestMethod]
        public void TestMaximizeWindow()
        {
            driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(1000);
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);
        }

        [TestMethod]
        public void TestMinimizeBrowser()
        {
            driver.Manage().Window.Position = new System.Drawing.Point(-2000, 0);
            driver.FindElement(By.LinkText("Hyperlink")).Click();
            System.Threading.Thread.Sleep(2000);
            driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
        }

        [TestMethod]
        public void TestMoveBrowser()
        {
            driver.Manage().Window.Position = new System.Drawing.Point(100, 100); 
            System.Threading.Thread.Sleep(1000);
            driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
        }

        public void Visit(String path)
        {
            driver.Navigate().GoToUrl(siteRootUrl + path);
        }

        [TestMethod]
        public void TestGoToPageWithinSiteUsingFunction()
        {
            Visit("/demo");
            Visit("/demo/survey");
            Visit("/"); // home page   
        }

        [TestMethod]
        public void TestSwitchWindowOrTab()
        {
            driver.FindElement(By.LinkText("Hyperlink")).Click();
            driver.FindElement(By.LinkText("Open new window")).Click();
            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String firstTab = (String)windowHandles[0];
            String lastTab = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(lastTab);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("This is url link page"));
            driver.SwitchTo().Window(firstTab); // back to first tab/window
            Assert.IsTrue(driver.FindElement(By.LinkText("Open new window")).Displayed);
        }

        [TestMethod]
        public void TestCreateNewWindow()
        {
            //  Opens a new window and switches to new window
            driver.SwitchTo().NewWindow(WindowType.Window);
            driver.Navigate().GoToUrl("https://whenwise.com");
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles[0]);
        }

        [TestMethod]
        public void TestCreateNewTab()
        {
            driver.SwitchTo().NewWindow(WindowType.Tab);
            driver.Navigate().GoToUrl("https://testwisely.com");
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles[0]);
        }

        [TestMethod]
        public void TestCreateNewTabWithJavaScript()
        {
            String newWebPageUrl = "https://whenwise.com";
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open('" + newWebPageUrl + "', '_blank')");

            ReadOnlyCollection<String> windowHandles = driver.WindowHandles;
            String firstTab = (String)windowHandles[0];
            String lastTab = windowHandles[windowHandles.Count - 1];
            driver.SwitchTo().Window(lastTab); // switch to the last tab
            System.Threading.Thread.Sleep(2000);
            driver.FindElement(By.LinkText("SIGN IN")).Click(); // works in new tab
            
            // now try to close the first tab
            driver.SwitchTo().Window(firstTab);
            driver.FindElement(By.LinkText("Hyperlink")).Click();
            driver.Close();
            Assert.IsTrue(driver.WindowHandles.Count() == windowHandles.Count - 1);

            // refocus
            driver.SwitchTo().Window(driver.WindowHandles[driver.WindowHandles.Count - 1]);
            driver.FindElement(By.LinkText("Register")).Click();
        }

        [TestMethod]
        public void TestScrollToElement()
        {
            driver.FindElement(By.LinkText("Button")).Click();
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);      
            IWebElement elem = driver.FindElement(By.Name("submit_action_2"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", elem);
            System.Threading.Thread.Sleep(2000);
            elem.Click();            
        }

        [TestMethod]
        public void TestScrollToElementWithPosition()
        {
            driver.FindElement(By.LinkText("Button")).Click();
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);            
            IWebElement elem = driver.FindElement(By.Name("submit_action_2"));
            int elemPos = elem.Location.Y;
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scroll(0, " + elemPos + ");");
            System.Threading.Thread.Sleep(2000);
            elem.Click();            
        }

        [TestMethod]
        public void TestScrollToPageBottom()
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }

        [TestMethod]
        public void TestScrollToPageTop()
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);");
        }

        [TestMethod]
        public void TestScrollWheelActions()
        {
            driver.FindElement(By.LinkText("Button")).Click();
            IWebElement elem = driver.FindElement(By.Name("submit_action_2"));
            new Actions(driver).ScrollToElement(elem).Perform();
            
            // scroll down by 100 pixels
            new Actions(driver).ScrollByAmount(0, 100).Perform();
        }
    }

}