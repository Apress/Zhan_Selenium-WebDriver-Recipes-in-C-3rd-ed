using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Drawing;

namespace SeleniumRecipes {

    [TestClass]
    public class Ch18Html5ChartTest {

        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context) {}

        [TestInitialize]
        public void Before() {
        }

        [TestMethod]
        public void TestSaveSVGAsPng() {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/charts.html");
            System.Threading.Thread.Sleep(500); // load JS
            WebElement svg_parent_elem = (WebElement) driver.FindElement(By.TagName("svg"));
            Screenshot screenshot =  svg_parent_elem.GetScreenshot();
            screenshot.SaveAsFile("/tmp/svg.png", ScreenshotImageFormat.Png); 
            Assert.IsTrue(File.Exists("/tmp/svg.png"));
        }

        [TestMethod]
        public void TestSaveCanvasAsPng() {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/canvas.html");
            System.Threading.Thread.Sleep(500); // load JS
            var canvas_elem = driver.FindElement(By.TagName("canvas"));
            //  extract canvas element's contents
            // ruby version, substring 21 works; safe to use 22, works for both
            string js = "return arguments[0].toDataURL('image/png').substring(22);";
            string canvas_base64 = (string) ((IJavaScriptExecutor) driver).ExecuteScript(js, canvas_elem);
            // Console.WriteLine(canvas_base64);
            //  decode from the base64 format, get the image binary data
            using (StreamWriter writer = new StreamWriter("/tmp/c_sharp_base64.txt"))
            {
                writer.WriteLine(canvas_base64);
            }
            byte[] canvas_png = Convert.FromBase64String(canvas_base64);
            using (BinaryWriter binWriter =
               new BinaryWriter(File.Open("/tmp/c_sharp_chart.png", FileMode.Create)))
            {
                binWriter.Write(canvas_png);
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