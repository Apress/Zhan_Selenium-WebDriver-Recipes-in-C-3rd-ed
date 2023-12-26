namespace SeleniumRecipes;
using OpenQA.Selenium.Chrome;


[TestClass]
public class Ch23Selenium4HeadlessTest
{
    static WebDriver driver;

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }


    [TestMethod]
    public void TestSavePdf() {
        PrintOptions printOptions = new PrintOptions
        {
          Orientation = PrintOrientation.Portrait
        };

        ChromeOptions chromeOptions = new ChromeOptions();
        // some doc says works in headless mode, but it seems works on both
        // chromeOptions.AddArguments("headless"); 

        driver = new ChromeDriver(chromeOptions);
        driver.Navigate().GoToUrl(TestHelper.SiteUrl() + "/image-map.html");
        
        //printing...
        PrintDocument printDocument = driver.Print(printOptions);

        //saving the file
        string printFinalPath = "/tmp/sample.pdf";
        printDocument.SaveAsFile(printFinalPath);
    }
 

}