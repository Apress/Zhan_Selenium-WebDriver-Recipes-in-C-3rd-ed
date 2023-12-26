using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Diagnostics;

// Reference: https://www.selenium.dev/documentation/webdriver/drivers/options/

namespace SeleniumRecipes
{

    [TestClass]
    public class Ch16BrowserProfileTest
    {
        static IWebDriver driver;

        [ClassInitialize]
        public static void BeforeAll(TestContext context)
        {
        }

        [TestInitialize]
        public void Before()
        {
        }

        [TestMethod]
        public void TestGetBrowserTypeAndVersion()
        {
            driver = new FirefoxDriver();
            ICapabilities caps = ((OpenQA.Selenium.WebDriver)driver).Capabilities;
            String browserName = caps.GetCapability("browserName").ToString();
            String browserVersion = caps.GetCapability("browserVersion").ToString();
            Assert.AreEqual("firefox", browserName);
            Console.WriteLine("browserVersion = " + browserVersion); // 111.0.1
            driver.Quit();

            driver = new ChromeDriver();
            caps = ((OpenQA.Selenium.WebDriver)driver).Capabilities;
            browserName = caps.GetCapability("browserName").ToString();
            browserVersion = caps.GetCapability("browserVersion").ToString();
            Assert.AreEqual("chrome", browserName);
            Console.WriteLine("browserVersion = " + browserVersion); // 115.0.5790.114
            driver.Quit();
        }

       [TestMethod]
        public void TestLoadStrategy()
        {
            Stopwatch sw1 = new Stopwatch();
            
            var chromeOptions = new ChromeOptions();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            IWebDriver driver = new ChromeDriver(chromeOptions);
            sw1.Start();
            driver.Navigate().GoToUrl("https://whenwise.com");
            sw1.Stop();
            long nanoseconds = sw1.ElapsedTicks / (Stopwatch.Frequency / (1000L*1000L*1000L));
            Console.WriteLine("Normal Operation completed in: " + sw1.ElapsedMilliseconds + " (ns)");
            driver.Quit();

            Stopwatch sw2 = new Stopwatch();
            chromeOptions = new ChromeOptions();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
            driver = new ChromeDriver(chromeOptions);
            sw2.Start();
            driver.Navigate().GoToUrl("https://whenwise.com");
            sw2.Stop();
            Console.WriteLine("Eager Operation completed in: " + sw2.ElapsedMilliseconds + " (ns)");
  driver.Quit();
            driver.Quit();

            Stopwatch sw3 = new Stopwatch();
            chromeOptions = new ChromeOptions();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.None;
            driver = new ChromeDriver(chromeOptions);
            sw3.Start();
            driver.Navigate().GoToUrl("https://whenwise.com");
            sw3.Stop();
            Console.WriteLine("None Operation completed in: " + sw3.ElapsedMilliseconds + " (ns)");
            driver.Quit();
        }

        // this test will fail unless have proper proxy settings
        // [TestMethod]
        // public void TestUseHTTPProxy()
        // {
        //     FirefoxOptions capabilities = new FirefoxOptions();
        //     FirefoxProfile firefoxProfile = new FirefoxProfile();
        //     firefoxProfile.SetPreference("network.proxy.type", 1);
        //     // See http://kb.mozillazine.org/Network.proxy.type

        //     firefoxProfile.SetPreference("network.proxy.http", "myproxy.com"); // put your proxy server here
        //     firefoxProfile.SetPreference("network.proxy.http_port", 3128);
        //     capabilities.Profile = firefoxProfile;
        //     driver = new FirefoxDriver(capabilities);
        //     driver.Navigate().GoToUrl("http://itest2.com/svn-demo/");
        // }

        [TestMethod]
        public void TestVerifyDownloadForChrome()
        {
            String myDownloadFolder = "/Users/courtney/tmp/";
            if (OperatingSystem.IsWindows())
               myDownloadFolder = @"c:\temp\";
            
            String expectedDownloadFile = myDownloadFolder + "practical-web-test-automation-sample.pdf";
            if(File.Exists(expectedDownloadFile)) {
                File.Delete(expectedDownloadFile);
            }

            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", myDownloadFolder);
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("http://zhimin.com/books/pwta");
            driver.FindElement(By.LinkText("Download")).Click();
            System.Threading.Thread.Sleep(10000); // wait 10 seconds for downloading to complete

            Assert.IsTrue(File.Exists(expectedDownloadFile));
        }

        [TestMethod]
        public void TestVerifyDownloadForFirefox()
        {
            String myDownloadFolder = "/Users/courtney/tmp/";
            if (OperatingSystem.IsWindows())
               myDownloadFolder = "c:\\temp\\";
            
            String expectedDownloadFile = myDownloadFolder + "selenium-recipes-in-ruby-sample.pdf";
            if(File.Exists(expectedDownloadFile)) {
                File.Delete(expectedDownloadFile);
            }
            
            FirefoxOptions options = new FirefoxOptions();
            options.SetPreference("browser.download.folderList", 2);
            options.SetPreference("browser.download.dir", myDownloadFolder);
            options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");            
            // disable Firefox's built-in PDF viewer
            options.SetPreference("pdfjs.disabled", true);

            driver = new FirefoxDriver(options);
            driver.Navigate().GoToUrl("http://zhimin.com/books/selenium-recipes");
            driver.FindElement(By.LinkText("Download")).Click();
            System.Threading.Thread.Sleep(10000); // wait 10 seconds for downloading to complete

            Assert.IsTrue(File.Exists(expectedDownloadFile));
        }

        // Firefox Profile folder has a random string in the front of name, eg. "8yggbtss.testing"
        // This method returns the file path for a given profile name. 
        // public static String GetFirefoxProfileFolderByName(String name)
        // {
        //     string pathToCurrentUserProfiles = Environment.ExpandEnvironmentVariables("%APPDATA%") + @"\Mozilla\Firefox\Profiles"; // Path to profile
        //     string[] pathsToProfiles = Directory.GetDirectories(pathToCurrentUserProfiles, "*.*", SearchOption.TopDirectoryOnly);
        //     foreach (var folder in pathsToProfiles)
        //     {
        //         if (folder.EndsWith(name))
        //         {
        //             return folder;
        //         }
        //     }
        //     return null;
        // }


        /* 
        This works, but depend on more prepartion work, commented out in CT
        [TestMethod]
        public void TestByPassBasicAuthenticationWithAutoAuthPlugin()
        {
            // * you need have 'testing' profile set up in Firefox, see the book for instructions
            // change to your testing foler
            FirefoxProfile firefoxProfile = new FirefoxProfile(@"/Users/zhimin/Library/Application Support/Firefox/Profiles/4pzuakwc.testing");
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.Profile = firefoxProfile;
            driver = new FirefoxDriver(firefoxOptions);
            ((FirefoxDriver) driver).InstallAddOnFromFile(@"/Users/zhimin/Downloads/autoauth-3.1.1.xpi"); // Added in Selenium 4.0+
            
            driver.Navigate().GoToUrl("http://zhimin.com/books/bought-learn-ruby-programming-by-examples");
            // got in, see privileged link
            Assert.IsTrue(driver.FindElement(By.LinkText("Download")).Displayed);
            driver.Quit();
        }
        */

        [TestMethod]
        public void TestCookies()
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://travel.agileway.net");
            driver.Manage().Cookies.AddCookie(new Cookie("foo", "bar"));
            var allCookies = driver.Manage().Cookies.AllCookies;
            Cookie retrieved = driver.Manage().Cookies.GetCookieNamed("foo");
            Assert.AreEqual("bar", retrieved.Value);
        }

        [TestMethod]
        public void TestHeadlessChromeTest()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            driver = new ChromeDriver(chromeOptions);

            driver.Navigate().GoToUrl("http://travel.agileway.net");
            driver.FindElement(By.Id("username")).SendKeys("agileway");
            driver.FindElement(By.Id("password")).SendKeys("testwise");
            driver.FindElement(By.XPath("//input[@value='Sign in']")).Click();
            System.Threading.Thread.Sleep(500);
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Signed in!"));
        }

        [TestMethod]
        public void TestResponsiveWebsites()
        {
            driver = new ChromeDriver();

            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768); // Desktop
            driver.Url = "https://support.agileway.net"; // default 
            int widthDesktop = driver.FindElement(By.Name("email")).Size.Width; 
            driver.Manage().Window.Size = new System.Drawing.Size(768, 1024); // iPad
            int widthIPad = driver.FindElement(By.Name("email")).Size.Width;
            Console.WriteLine(widthIPad); 
            Assert.IsTrue(widthDesktop < widthIPad); // 358 vs 960
        }

        [TestCleanup]
        public void After()
        {
        }

        [ClassCleanup]
        public static void AfterAll()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }


}





