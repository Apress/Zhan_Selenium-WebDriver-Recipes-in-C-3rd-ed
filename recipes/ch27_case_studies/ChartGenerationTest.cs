using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.IO;

namespace SeleniumRecipes {

    [TestClass]
    public class Ch27ChartGenerationTest {
        static IWebDriver driver = new ChromeDriver();
        static string savedChartFilePath = "/tmp/chart.png";

        [ClassInitialize]
        public static void BeforeAll(TestContext context) {

            if (OperatingSystem.IsWindows()) {
                savedChartFilePath = @"C:\temp\chart.png";
            }

            // a better way is to use relative path
            // 
            // savedChartFilePath = Path.Combine(Environment.CurrentDirectory, "chart.png"); 
            // Console.WriteLine("Outputing file to " + savedChartFilePath);
           
        }

        [TestInitialize]
        public void Before() {
        }

        [TestMethod]
        public void TestSaveChart() {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/dynamic-chart.html");
            try {
              driver.FindElement(By.TagName("svg"));
              Assert.Fail("The SVG should not existed yet");
            } catch (OpenQA.Selenium.NoSuchElementException e) {
                Console.WriteLine("As expected");
            }

            driver.FindElement(By.Id("gen-btn")).Click();
            System.Threading.Thread.Sleep(1000); // load JS

            WebElement svg_elem = (WebElement) driver.FindElement(By.TagName("svg"));
            Screenshot screenshot = svg_elem.GetScreenshot();

            screenshot.SaveAsFile(savedChartFilePath, ScreenshotImageFormat.Png); 
            Assert.IsTrue(File.Exists(savedChartFilePath));

            // Add nuget System.Drawing.Common
            // which only supported on Windows https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only
            if (OperatingSystem.IsWindows()) {
                System.Drawing.Image img = System.Drawing.Image.FromFile(savedChartFilePath);
                Console.WriteLine("Width: " + img.Width + ", Height: " + img.Height);
                Assert.AreEqual(400, img.Height);
            }
        }

        [TestCleanup]
        public void After() {}

        [ClassCleanup]
        public static void AfterAll() {
            driver.Quit();
        }
    }

}