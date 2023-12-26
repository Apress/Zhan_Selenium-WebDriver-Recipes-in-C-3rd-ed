

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
    public class Ch05HiddenFieldTest
    {

        static IWebDriver driver;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            driver = new ChromeDriver(options);   
        }

        [TestInitialize]
        public void Before()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/text_field.html");
        }

        [TestMethod]
        public void TestSetAndAssertHiddenField()
        {
            IWebElement theHiddenElem = driver.FindElement(By.Name("currency"));
            Assert.AreEqual("USD", theHiddenElem.GetAttribute("value"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = 'AUD';", theHiddenElem);           
            Assert.AreEqual("AUD", theHiddenElem.GetAttribute("value"));            
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