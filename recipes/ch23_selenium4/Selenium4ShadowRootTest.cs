namespace SeleniumRecipes;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

[TestClass]
public class Ch23Selenium4ShadowRootTest
{
    static WebDriver driver;

    [ClassCleanup]
    public static void AfterAll() {
    //   if (driver != null)
        // driver.Quit();
    }



    [TestMethod]
    public void TestShadowRoot() {
        ChromeOptions options = new ChromeOptions();
        IWebDriver driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://fiddle.luigi-project.io/#/home/wc1");
        // NOTE:
        //  add *1 to get its first child
        IWebElement elem = driver.FindElements(By.XPath("//div[contains(@class, 'wcContainer svelte-')]/*[1]"))[0];

        // NOTE: Selenium 4 has a new method to get Shadow Root easy, compared Selenium 3's using JavaScript way
        ISearchContext shadowRoot = elem.GetShadowRoot();
        var inputElem = shadowRoot.FindElement(By.CssSelector("input.add-new-list-item-input"));
        inputElem.SendKeys("Peach here I come!");
        shadowRoot.FindElement(By.CssSelector("button.editable-list-add-item.icon")).Click();
        System.Threading.Thread.Sleep(500);
        Assert.IsTrue(driver.FindElement(By.TagName("body")).Text.Contains("Peach here I come!"));

        // Clear up
        
        // NOTE: for shadown root, seem only can use "CssSelector", XPath/Tag returns "invalid locator" error 
        // var lastRemoveBtn = shadowRoot.FindElements(By.XPath("div/ul/li/button")).Last(); 
        
        var lastRemoveBtn = shadowRoot.FindElement(By.CssSelector("div > ul > li:last-child > button"));
        lastRemoveBtn.Click();
    }

// OLD JS Way
// IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
//var element = js.ExecuteScript("return document.querySelector('selector_outside_shadow_root').shadowRoot.querySelector('selector_inside_shadow_root');");


}