namespace SeleniumRecipes;
using OpenQA.Selenium.Chrome;


[TestClass]
public class Ch23RegisterBasicAuthTest
{
    static WebDriver driver;

    [ClassCleanup]
    public static void AfterAll() {
      if (driver != null)
        driver.Quit();
    }

    // Doc: https://www.selenium.dev/documentation/webdriver/bidirectional/bidi_api/#register-basic-auth
    [TestMethod]
    public async Task TestRegisterBasicAuth() {
        ChromeOptions options = new ChromeOptions();
        driver = new ChromeDriver(options);

        NetworkAuthenticationHandler handler = new NetworkAuthenticationHandler()
        {
            UriMatcher = (d) => d.Host.Contains("zhimin.com"),
            Credentials = new PasswordCredentials("agileway", "SUPPORTWISE15")
        };

        INetwork networkInterceptor = driver.Manage().Network;
        networkInterceptor.AddAuthenticationHandler(handler);
        await networkInterceptor.StartMonitoring();

        driver.Navigate().GoToUrl("http://zhimin.com/books/bought-learn-ruby-programming-by-examples");
        driver.FindElement(By.LinkText("Download"));
    }

}