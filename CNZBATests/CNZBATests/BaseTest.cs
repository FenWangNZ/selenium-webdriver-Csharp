using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CNZBATests
{
    public class BaseTest
    {
        public IWebDriver Driver { get; private set; }

        [TestInitialize]
        public void SetupBeforEverySingleMethod()
        {
            Driver = ChromeDriver();
        }

        [TestCleanup]
        public void CleanupAfterEverySingleMethod()
        {
            Driver.Close();
            Driver.Quit();
        }

        private IWebDriver ChromeDriver()
        {
            //Get the name of the directory of the location of the executable;
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //Return a new ChromeDriver with the path to the ChromeDriver that we have now: the binary;
            return new ChromeDriver(outPutDirectory);
        }
    }
}