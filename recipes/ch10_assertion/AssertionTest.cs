

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace SeleniumRecipes
{
    [TestClass]
    public class Ch10AssertionTest
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
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/assert.html");
        }

        [TestMethod]
        public void TestAssertTitle()
        {
            Assert.AreEqual("Assertion Test Page", driver.Title);
        }

        [TestMethod]
        public void TestAssertPageText()
        {
            // \r\n for Windows, \n for Linux/Mac
            String line_ending_character = LineEndingCharacter();

            String matching_str = "Text assertion with a  (tab before), and " + line_ending_character + "(new line before)!";

            // watir IE repalce \n with \r\n
            Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains(matching_str));
        }

        [TestMethod]
        public void TestAssertPageSource()
        {
            // \r\n for Windows, \n for Linux/Mac
            String line_ending_character = LineEndingCharacter();

            String html_fragment = "Text assertion with a  (<b>tab</b> before), and " + line_ending_character + "(new line before)!";
            Assert.IsTrue(driver.PageSource.Contains(html_fragment));
        }

        [TestMethod]
        public void TestAssertLabelText()
        {
            Assert.AreEqual("First Label", driver.FindElement(By.Id("label_1")).Text);
            Assert.AreEqual("Second Span", driver.FindElement(By.Id("span_2")).Text);
        }

        [TestMethod]
        public void TestAssertDivText()
        {
            // \r\n for Windows, \n for Linux/Mac
            String line_ending_character = LineEndingCharacter();

            Assert.AreEqual("TestWise", driver.FindElement(By.Id("div_child_1")).Text);
            Assert.AreEqual("Wise Products" + line_ending_character + "TestWise" + line_ending_character + "BuildWise", driver.FindElement(By.Id("div_parent")).Text);
        }

        [TestMethod]
        public void TestAssertDivHtml()
        {
            // \r\n for Windows, \n for Linux/Mac
            String line_ending_character = LineEndingCharacter();

            IWebElement the_element = driver.FindElement(By.Id("div_parent"));
            Object html = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].outerHTML;", the_element);
            Console.WriteLine("html = " + html);
            String expected = "<div id=\"div_parent\">" + line_ending_character
                    + "	   Wise Products" + line_ending_character
                    + "	   <div id=\"div_child_1\">" + line_ending_character
                    + "	   	 TestWise" + line_ending_character
                    + "	   </div>" + line_ending_character
                    + "	   <div id=\"div_child_2\">" + line_ending_character
                    + "	   	 BuildWise" + line_ending_character
                    + "	   </div>" + line_ending_character
                    + "	 </div>";
            Assert.AreEqual(expected, html);
        }

        [TestMethod]
        public void TestAssertTextInTable()
        {
            // \r\n for Windows, \n for Linux/Mac
            String line_ending_character = LineEndingCharacter();

            IWebElement the_element = driver.FindElement(By.Id("alpha_table"));
            Assert.AreEqual("A B" + line_ending_character + "a b", the_element.Text);
            Object html = ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].outerHTML;", the_element);
            String htmlStr = (String)html;
            Assert.IsTrue(htmlStr.Contains("<td id=\"cell_1_1\">A</td>"));
        }

        [TestMethod]
        public void TestAssertTextInTableCell()
        {
            Assert.AreEqual("A", driver.FindElement(By.Id("cell_1_1")).Text);
        }

        [TestMethod]
        public void TestAssertTextInTableCellWithIndex()
        {
            Assert.AreEqual("b", driver.FindElement(By.XPath("//table/tbody/tr[2]/td[2]")).Text);
        }

        [TestMethod]
        public void TestAssertTextInTableRow()
        {
            Assert.AreEqual("A B", driver.FindElement(By.Id("row_1")).Text);
        }


        [TestMethod]
        public void TestAssertImagePresent()
        {
            Assert.IsTrue(driver.FindElement(By.Id("next_go")).Displayed);
        }

        [TestMethod]
        public void TestAssertElementLocationAndWidth()
        {
            IWebElement imageElem = driver.FindElement(By.Id("next_go"));
            Assert.AreEqual(imageElem.Size.Width, 46);
            Assert.IsTrue(imageElem.Location.X > 100);
        }

        [TestMethod]
        public void TestAssertElementCSSStyle()
        {
            IWebElement elem = driver.FindElement(By.Id("highlighted"));
            Assert.AreEqual(elem.GetCssValue("font-size"), "15px");
            Assert.AreEqual(elem.GetCssValue("background-color"), "rgba(206, 218, 227, 1)");
        }

        [TestMethod]
        public void TestAssertJSErrorsOnWebPage()
        {
            driver.Navigate().GoToUrl("https://agileway.com.au/demo/customer-interview");
            ReadOnlyCollection<LogEntry> logEntries = driver.Manage().Logs.GetLog(LogType.Browser);
            Assert.AreEqual(logEntries.Count, 1);
            Console.WriteLine(logEntries.First().ToString());
            Assert.IsTrue(logEntries.First().ToString().Contains("Failed to load resource"));
        }

        [TestMethod]
        public void TestAssertNoJSErrorsOnWebPage()
        {
            driver.Navigate().GoToUrl("https://agileway.com.au/demo");
            ReadOnlyCollection<LogEntry> logEntries = driver.Manage().Logs.GetLog(LogType.Browser);
            Assert.AreEqual(logEntries.Count, 0);
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

        public static String LineEndingCharacter() {
            // \r\n for Windows, \n for Linux/Mac
            String line_ending_character = "\r\n";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                line_ending_character = "\n";
            }
            return line_ending_character;
        }
    }

}

