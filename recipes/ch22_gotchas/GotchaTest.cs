
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Interactions;

namespace SeleniumRecipes
{
    [TestClass]
    public class Ch22GotchaTest
    {
        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/emberjs-crud-rest/index.html");
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            driver.Quit();
        }
        
        [TestMethod]
        public void TestChangeLogicBasedBrowser()
        {
            ICapabilities caps = ((OpenQA.Selenium.WebDriver)driver).Capabilities;
            var driverName = caps.GetType().ToString();
            var browserName = caps.GetCapability("browserName").ToString();
            if (browserName == "chrome")
            {
                Console.WriteLine("Browser is Chrome");
                // chrome specific test statement
            }
            else if (browserName == "firefox")
            {
                // firefox specific test statement
                Console.WriteLine("Browser is FireFox");
            }
            else
            {
                throw new Exception("Unsupported browser: " + browserName);
            }
        }
    }
}
