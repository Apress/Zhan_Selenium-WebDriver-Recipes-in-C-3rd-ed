using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Diagnostics;

namespace SeleniumRecipes {

    [TestClass]
    public class Ch27GenerateUserGuideTest {
        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context) {
        }

        [TestInitialize]
        public void Before() {
        }

        [TestMethod]
        public void TestGenerateUserGuide() {
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);
            driver.Navigate().GoToUrl("https://whenwise.com/become-partner");
          
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            var imageFilePath = TestHelper.TempDir() +  "step_1.png";
            ss.SaveAsFile(imageFilePath, ScreenshotImageFormat.Png);  

            driver.FindElement(By.Id("biz_name")).SendKeys("Wise Business");
            var dropdown_xpath = "//select[@name='biz[business_type]']/../..";
            driver.FindElement(By.XPath(dropdown_xpath)).Click();
            System.Threading.Thread.Sleep(250);
            WebElement elemBizTypeList = (WebElement) driver.FindElement(By.XPath(dropdown_xpath + "/ul"));
            ss = elemBizTypeList.GetScreenshot();
            ss.SaveAsFile(TestHelper.TempDir() + "step_2.png", ScreenshotImageFormat.Png);  

            driver.FindElement(By.XPath(dropdown_xpath + "/ul/li/span[text()='Driving Instructors']")).Click();
            WebElement elemCreateBtn = (WebElement) driver.FindElement(By.Id("create-account"));
            ss = elemCreateBtn.GetScreenshot();
            ss.SaveAsFile(TestHelper.TempDir() + "step_3.png", ScreenshotImageFormat.Png);  

            List<string> guideList = new List<string>();
            guideList.Add("## Guide: Business Sign up");
            guideList.Add("1. **Open the sign up page**");
            guideList.Add("    <img src='step_1.png' height='240'/>");
            guideList.Add("2. **Enter your business name**");
            guideList.Add("3. **Select business type**");
            guideList.Add("   ![](step_2.png)");
            guideList.Add("4. **Click the 'SIGN UP' button**");
            guideList.Add("   ![](step_3.png)");

            string guide_markdown = String.Join("\r\n\r\n", guideList);
            var guide_md_path = TestHelper.TempDir() + "guide.md";
            using (StreamWriter outputFile = new StreamWriter(guide_md_path))
            {
                outputFile.WriteLine(guide_markdown);
            }

            // execute "md2html" script (a part of md2html gem)
            string md2html_cmd = "/Users/zhimin/.rbenv/shims/md2html";
            if (!File.Exists(md2html_cmd)) {
                 md2html_cmd = "/opt/homebrew/opt/rbenv/shims/md2html";
            }
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = md2html_cmd,
                Arguments = guide_md_path,
                WorkingDirectory = TestHelper.TempDir()
            };
            Process proc = new Process()
            {
                StartInfo = startInfo,
            };
            proc.Start();

            // if (OperatingSystem.IsMacOS()) {
                // System.Diagnostics.Process.Start("md2html " +  md2html_cmd);
            // }
            
        }

        [TestCleanup]
        public void After() {}

        [ClassCleanup]
        public static void AfterAll() {
            // driver.Quit();
        }
    }

}