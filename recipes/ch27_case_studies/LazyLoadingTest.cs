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
    public class Ch27LazyLoadingTest {
        static IWebDriver driver = new ChromeDriver();

        [ClassInitialize]
        public static void BeforeAll(TestContext context) {
        }

        [TestInitialize]
        public void Before() {
        }

        [TestMethod]
        public void TestLazyLoading() {
            driver.Navigate().GoToUrl("https://agileway.substack.com/archive");
            // OK
            driver.FindElement(By.PartialLinkText("Micro-ISV 01"));
           
            // not shown yet
            for (int i = 1; i <= 100; i++) 
            {
                Console.WriteLine("Scroll: " + i);
                driver.FindElement(By.TagName("body")).SendKeys(Keys.PageDown);
                System.Threading.Thread.Sleep(1000);
                try {
                    driver.FindElement(By.PartialLinkText("Page Object Model is universally applicable"));
                    // quit the loop if found
                    break;
                } catch (OpenQA.Selenium.NoSuchElementException e) {
                    // not found, continue
                }
            }
       
            driver.FindElement(By.PartialLinkText("Page Object Model is universally applicable")).Click();
            System.Threading.Thread.Sleep(1000); // loading JS
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("a well-known test design pattern in test automation"));
        }

        [TestCleanup]
        public void After() {}

        [ClassCleanup]
        public static void AfterAll() {
            // driver.Quit();
        }
    }

}