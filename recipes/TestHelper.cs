using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumRecipes
{
    public class TestHelper
    {

        public static String SiteUrl() {
            // change to your installed location for the book site (http://zhimin.com/books/selenium-recipes-csharp)
            if (OperatingSystem.IsWindows()) {
                return "file:///C:/work/books/SeleniumRecipes-C%23/site";
            } else {
                return "file:///Users/zhimin/work/books/SeleniumRecipes-C%23/site";
            }
        }

        // change to yours
        public static String ScriptDir()
        {
            // return @"C:\agileway\books\SeleniumRecipes-C#\recipes"; // Windows
            // return "/Users/zhimin/work/books/selenium-webdriver-recipes-in-csharp-3ed/recipes"; // macOS/Linux
            return Environment.CurrentDirectory + "/../../..";
        }

        public static String TempDir()
        {     
           if (OperatingSystem.IsWindows()) {
             return @"C:\temp\";
           } else {
             return "/tmp/";
           }
        }

    }
}
