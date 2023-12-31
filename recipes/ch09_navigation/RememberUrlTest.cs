
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;


namespace SeleniumRecipes
{
    [TestClass]
    public class Ch09RememberUrlTest
    {

        static IWebDriver driver = new ChromeDriver();
        String url;

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
        public void TestRememberUrl()
        {
            url = driver.Url;
            driver.FindElement(By.LinkText("Button")).Click();
            //...
            driver.Navigate().GoToUrl(url);
        }

    }

}

