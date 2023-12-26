using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace SeleniumRecipes
{
    [TestClass]
    public class Ch15QueryDatabaseTest
    {
        static IWebDriver driver = new ChromeDriver();

        [TestMethod]
        public void TestDatabaseSqlite3()
        {
            driver.Navigate().GoToUrl(TestHelper.SiteUrl() +  "/text_field.html");

            String oldestUserLogin = null;
            SQLiteConnection connection = null;

            try
            {
                // copy the test data from testdata/folder to output folder
                // String sourceFile =  Path.Combine(TestHelper.ScriptDir() + @"..\testdata\sample.db");

                String dbFile = Path.Combine(Environment.CurrentDirectory, "sample.db");
                Console.WriteLine("Using database: " + dbFile);
                connection = new SQLiteConnection("Data Source=" + dbFile + ";Version=3");
                connection.Open();

                String sql = "select login from users order by age desc";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {    // read the result set
                    oldestUserLogin = (String)reader["login"];
                    System.Console.WriteLine("Old Login: " + oldestUserLogin);
                    break;
                }
            }
            catch (Exception e)
            {
                // probably means no database file is found
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }
                catch (Exception e)
                {  // connection close failed.
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine(" => " + oldestUserLogin);
            driver.FindElement(By.Id("user")).SendKeys(oldestUserLogin);
        }
    }
}
