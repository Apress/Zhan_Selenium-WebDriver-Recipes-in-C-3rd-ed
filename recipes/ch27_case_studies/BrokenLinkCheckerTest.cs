using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Net;

namespace SeleniumRecipes {

    [TestClass]
    public class Ch27BrokenLinkCheckerTest {
        static IWebDriver driver = new ChromeDriver();
        static String site_host = "https://travel.agileway.net";
        // static String site_host = "https://whenwise.agileway.net";

        [ClassInitialize]
        public static void BeforeAll(TestContext context) {
        }

        [TestInitialize]
        public void Before() {
        }

    
        static bool IsBrokenLink(string url) {
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(url);
            request.AllowAutoRedirect = true;

            try {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)  {
                    response.Close(); 
                    return false;
                } else {
                    return true;
                }
            } catch { // if request fails, treat it as broken
                return true;
             }
        }

        [TestMethod]
        public void TestCheckBrokenLinks() {
            List<string> urls = new List<string>();
            List<string> brokenLinks = new List<string>();

            urls.Add(site_host);
            int pointer = 0;
        
            while(urls.Count > pointer) {
                var url = urls[pointer];
                Console.WriteLine("URL: " + url);
                
                var flagBroken = IsBrokenLink(url);
                pointer += 1;
                if (flagBroken) { 
                    brokenLinks.Add(url);
                    continue;                    
                } 

                driver.Navigate().GoToUrl(url);
                System.Threading.Thread.Sleep(2); // allow JS to load
                                
                ReadOnlyCollection<IWebElement> linkElements = driver.FindElements(By.TagName("a"));
                Console.WriteLine("New Links count: " + linkElements.Count);
                foreach( IWebElement elem in linkElements ) {
                    string new_url = elem.GetAttribute("href");
                    if (new_url != null && new_url.Contains(site_host)) {
                        if (!urls.Contains(new_url)) {
                            Console.WriteLine("=> " + new_url);
                            urls.Add(new_url);
                        }
                    }
                }
            }     

            Console.WriteLine("Total Page Count: " + urls.Count);      
            Console.WriteLine("Total Broken Links: " + brokenLinks.Count); 
            Console.WriteLine(string.Join(", ", brokenLinks));     
        }

        [TestCleanup]
        public void After() {}

        [ClassCleanup]
        public static void AfterAll() {
            driver.Quit();
        }
    }

}