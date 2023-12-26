namespace SeleniumRecipes;

using System.Collections.ObjectModel;
using OpenQA.Selenium.Chrome;
using static OpenQA.Selenium.RelativeBy;

[TestClass]
public class Ch23Selenium4RelativeLocatorNearTest
{
    static WebDriver driver;

    [ClassInitialize]
    public static void BeforeAll(TestContext context)
    {         
        ChromeOptions options = new ChromeOptions();
        driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/relative.html");
    }
    
    [ClassCleanup]
    public static void AfterAll() {
        if (driver != null)
          driver.Quit();
    }


    [TestMethod]
    public void TestRelativeNear() {
        var start_cell = driver.FindElement(By.Id("current-page"));
        ReadOnlyCollection<IWebElement> allLinks = driver.FindElements(RelativeBy.WithLocator(By.TagName("a")));
        Assert.AreEqual(9, allLinks.Count);
       
        // TODO: System.ArgumentNullException: 
        // not returning C# on macOS, not  C# on Windows, 
        // but works fine on Ruby
        ReadOnlyCollection<IWebElement> neighbours = driver.FindElements(RelativeBy.WithLocator(By.TagName("a")).Near(start_cell));
        Assert.AreEqual(2, neighbours.Count);
        var cell_texts = neighbours.Select(s => s.Text).ToArray();
        CollectionAssert.AreEqual(new string[]{"Prev", "Next"}, cell_texts);

        neighbours = driver.FindElements(RelativeBy.WithLocator(By.TagName("a")).Near(start_cell, 20));
        cell_texts = neighbours.Select(s => s.Text).ToArray();
        CollectionAssert.AreEqual(new string[]{"Prev", "Next"}, cell_texts);

        neighbours = driver.FindElements(RelativeBy.WithLocator(By.TagName("a")).Near(start_cell, 145));
        cell_texts = neighbours.Select(s => s.Text).ToArray();
        CollectionAssert.AreEqual(new string[]{"Prev", "Next", "First"}, cell_texts);

        neighbours = driver.FindElements(RelativeBy.WithLocator(By.TagName("a")).Near(start_cell, 208));
        cell_texts = neighbours.Select(s => s.Text).ToArray();
        CollectionAssert.AreEqual(new string[]{"Prev", "Next", "First", "https://whenwise.com", "http://sitewisecms.com", "https://testwisely.com/testwise"}, cell_texts);
        Assert.IsFalse(cell_texts.Contains("Last"));

        neighbours = driver.FindElements(RelativeBy.WithLocator(By.TagName("a")).Near(start_cell, 238));
        cell_texts = neighbours.Select(s => s.Text).ToArray();
        Assert.IsTrue(cell_texts.Contains("Last"));
    }

}