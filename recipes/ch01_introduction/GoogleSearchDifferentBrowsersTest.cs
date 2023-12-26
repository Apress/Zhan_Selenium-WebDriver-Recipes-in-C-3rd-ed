namespace SeleniumRecipes;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Safari;
using System.Collections.ObjectModel;
using System.IO;


[TestClass]
public class GoogleSearchDifferentBrowsersTest {

    [TestMethod]
    public void TestInFirefox() {
        IWebDriver driver = new FirefoxDriver();
        driver.Navigate().GoToUrl("https://agileway.com.au/demo");
        System.Threading.Thread.Sleep(1000);
        driver.Quit();
    }

    [TestMethod]
    public void TestInChrome() {
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("http://agileway.com.au/demo");
        System.Threading.Thread.Sleep(1000);
        driver.Quit();
    }

    [TestMethod]
    public void TestInSafari() {
        IWebDriver driver = new SafariDriver();
        driver.Navigate().GoToUrl("http://agileway.com.au/demo");
        System.Threading.Thread.Sleep(1000);
        driver.Quit();
    }

    [TestMethod]
    public void TestInEdge()
    {
        IWebDriver driver = new EdgeDriver();
        System.Diagnostics.Debug.Write("Start... ");
        driver.Navigate().GoToUrl("http://travel.agileway.net");
        driver.FindElement(By.Name("username")).SendKeys("agileway");
        IWebElement passwordElem =  driver.FindElement(By.Name("password"));
        passwordElem.SendKeys("testwise");
        passwordElem.Submit(); // not implemented 
        System.Threading.Thread.Sleep(1000);
        driver.Quit();
    }

}
