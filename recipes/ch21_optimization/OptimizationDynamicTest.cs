
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
    public class Ch21OptimizationDynamicTest
    {

        static IWebDriver driver;
        static String siteRootUrl = "https://wisephysio.agileway.net"; // en
        // static String siteRootUrl = "https://books.agileway.net"; // cn

        private static readonly Dictionary<string, string> natalieUserDict = new Dictionary<string, string> {
            { "english", "natalie" },
            { "chinese", "tuo" },
            { "french", "dupont" }
        };

        private static readonly Dictionary<string, string> markUserDict = new Dictionary<string, string> {
            { "english", "mark" },
            { "chinese", "li" },
            { "french", "marc" }
        };

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
        }

        [TestCleanup]
        public void After()
        {
            driver.Quit();
        }

        [ClassCleanup]
        public static void AfterAll()
        {
        }

        [TestMethod]
        public void TestUseEnvironmentVariableToChangeTestBehaviourDynamically()
        {
            // Warning: Visual Studio Caching Environment Variables

            String browserTypeSetInEnv = Environment.GetEnvironmentVariable("BROWSER");
            Console.WriteLine(browserTypeSetInEnv);
            if (!String.IsNullOrEmpty(browserTypeSetInEnv) && browserTypeSetInEnv.Equals("Firefox"))
            {
                driver = new FirefoxDriver();
            }
            else
            {
                driver = new ChromeDriver();
            }

            if (!String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SITE_URL")))
            {
                siteRootUrl = Environment.GetEnvironmentVariable("SITE_URL");
            }
            driver.Navigate().GoToUrl(siteRootUrl);
        }


        [TestMethod]
        public void TestMultiLangWithDifferentTestUserAccounts()
        {
            if (!String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("BASE_URL")))
            {
                siteRootUrl = Environment.GetEnvironmentVariable("BASE_URL");
            }
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl + "/admin");

            driver.FindElement(By.Id("sign_in_email")).SendKeys(IsChinese() ? "tuo" : "natalie");
            driver.FindElement(By.Id("sign_in_password")).SendKeys("test");
            driver.FindElement(By.Id("sign_in_btn")).Click();
            Assert.IsTrue(driver.PageSource.Contains(IsChinese() ? "不正确的用户名或密码" : "Invalid login or password"));                            
        }

        [TestMethod]
        public void TestTwoLanguagesUsingIfElse()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl + "/admin"); // may be dynamically set

            if (siteRootUrl.Contains("wisephysio"))
            {
                driver.FindElement(By.Id("sign_in_email")).SendKeys( "natalie");
                driver.FindElement(By.Id("sign_in_password")).SendKeys("test");
                driver.FindElement(By.Id("sign_in_btn")).Click();
                Assert.IsTrue(driver.PageSource.Contains("Invalid login or password"));
            }
            else if (siteRootUrl.Contains("books"))
            {
                 driver.FindElement(By.Id("sign_in_email")).SendKeys( "tuo");
                driver.FindElement(By.Id("sign_in_password")).SendKeys("test");
                driver.FindElement(By.Id("sign_in_btn")).Click();
                Assert.IsTrue(driver.PageSource.Contains("不正确的用户名或密码"));
            }

        }


        public bool IsChinese()
        {
            return siteRootUrl.Contains("books");
        }

        [TestMethod]
        public void TestTwoLanguagesUsingTernaryOperator()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl + "/admin"); // may be dynamically set

            driver.FindElement(By.Id("sign_in_email")).SendKeys(IsChinese() ? "tuo" : "natalie");
            driver.FindElement(By.Id("sign_in_password")).SendKeys("test");
            driver.FindElement(By.Id("sign_in_btn")).Click();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains(IsChinese() ? "不正确的用户名或密码": "Invalid login or password"));
        }

        public string SiteLang()
        {
            if (siteRootUrl.Contains("books"))
            {
                return "chinese";
            } else if (siteRootUrl.Contains("sandbox"))
            {
                return "french";
            } else
            {
                return "english";
            }
        }


        [TestMethod]
        public void TestMultipleLanguagesUsingIfElse()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl + "/admin"); // may be dynamically set

            if (SiteLang() == "chinese")
            {
                driver.FindElement(By.Id("sign_in_email")).SendKeys("yake@biz.com");
            }
            else if (SiteLang() == "french")
            {
                driver.FindElement(By.Id("sign_in_email")).SendKeys("dupont");
            }
            else { // default
                driver.FindElement(By.Id("sign_in_email")).SendKeys("yoga@biz.com");
            }
            driver.FindElement(By.Id("sign_in_password")).SendKeys("test01");
            driver.FindElement(By.Id("sign_in_btn")).Click();
        }

        public String UserLookup(string username)
        {
            switch (SiteLang())
            {
                case "chinese":
                    return "tuo";
                  
                case "french":
                    return "dupont";

                default:
                    return username;
            }
        }

        [TestMethod]
        public void TestMultipleLanguagesUsingLookup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl + "/admin"); // may be dynamically set

            driver.FindElement(By.Id("sign_in_email")).SendKeys(UserLookup("natalie"));
            driver.FindElement(By.Id("sign_in_password")).SendKeys("test");
            driver.FindElement(By.Id("sign_in_btn")).Click();
        }


        public String UserLookupDict(string username)
        {
            switch (username)
            {
                case "natalie":
                    return natalieUserDict[SiteLang()];

                case "mark":
                    return markUserDict[SiteLang()];

                default:
                    return username;
            }
        }

        [TestMethod]
        public void TestMultipleLanguagesUsingHashLookup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(siteRootUrl + "/admin"); // may be dynamically set
            driver.FindElement(By.Id("sign_in_email")).SendKeys(UserLookupDict("natalie"));
            driver.FindElement(By.Id("sign_in_password")).SendKeys("test");
            driver.FindElement(By.Id("sign_in_btn")).Click();
        }


    }
}