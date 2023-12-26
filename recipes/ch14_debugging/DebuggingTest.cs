using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.IO;

namespace SeleniumRecipes
{
    [TestClass]
    public class Ch14DebuggingTest
    {

        static IWebDriver driver = new ChromeDriver();

        String site_root_url;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/assert.html");
        }

        [TestMethod]
        public void TestPrintOutText()
        {
            Console.WriteLine("Now on page: " + driver.Title);
            String app_no = driver.FindElement(By.Id("app_id")).Text;
            Console.WriteLine("Application number is " + app_no);
        }

        [TestMethod]
        public void TestWritePageOrElementHtmlToFile()
        {
            var imageLoginPage = "";
            var imageLoginParent = "";
            if (OperatingSystem.IsWindows()) {
                imageLoginPage = TestHelper.TempDir() +  @"\login_page.html";
                imageLoginParent = TestHelper.TempDir() +  @"\login_parent.xhtml";
            } else {
                imageLoginPage = TestHelper.TempDir() +  "/login_page.html";
                imageLoginParent = TestHelper.TempDir() +  "/login_parent.xhtml";
            }
            using (StreamWriter outfile = new StreamWriter(imageLoginPage))
            {
                outfile.Write(driver.PageSource);
            }

            IWebElement the_element = driver.FindElement(By.Id("div_parent"));
            String the_element_html = (String)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].outerHTML;", the_element);

            using (StreamWriter outfile = new StreamWriter(imageLoginParent))
            {
                outfile.Write(the_element_html);
            }

        }

        [TestMethod]
        public void TestTakeScreenshot()
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            var imageFilePath = "";
            if (OperatingSystem.IsWindows()) {
                imageFilePath = TestHelper.TempDir() +  @"\screenshot.png";
            } else {
                imageFilePath = TestHelper.TempDir() +  "/screenshot.png";
            }
            ss.SaveAsFile(imageFilePath, ScreenshotImageFormat.Png);        
        }

        [TestMethod]
        public void TestTakeScreenshotWithTimestamp()
        {
            ITakesScreenshot ssdriver = driver as ITakesScreenshot;
            Screenshot screenshot = ssdriver.GetScreenshot();
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
            var imageFilePath = "";
            if (OperatingSystem.IsWindows()) {
                imageFilePath = TestHelper.TempDir() + @"\Exception-" + timestamp + ".png";
            } else {
                imageFilePath = TestHelper.TempDir() + "/Exception-" + timestamp + ".png";
            }
            screenshot.SaveAsFile(imageFilePath, ScreenshotImageFormat.Png);
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

    }

}