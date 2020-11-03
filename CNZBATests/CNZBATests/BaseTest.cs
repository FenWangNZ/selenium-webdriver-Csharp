using System.IO;
using System.Reflection;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NLog;

namespace CNZBATests
{
    
    [TestClass]
    public class BaseTest
    {
        public IWebDriver Driver { get; private set; }
        public TestContext TestContext { get; set; }
        private ScreenshotTaker ScreenshotTaker { get; set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [TestInitialize]
        public void SetupForEverySingleTestMethod()
        {
            Logger.Debug("*************************************** TEST STARTED");
            Logger.Debug("*************************************** TEST STARTED");
            Reporter.AddTestCaseMetadataToHtmlReport(TestContext);
            Driver = ChromeDriver();
            ScreenshotTaker = new ScreenshotTaker(Driver, TestContext);
        }

        public void TearDownForEverySingleTestMethod()
        {
            Logger.Debug(GetType().FullName + " started a method tear down");
            try
            {
                TakeScreenshotForTestFailure();
            }
            catch (Exception e)
            {
                Logger.Error(e.Source);
                Logger.Error(e.StackTrace);
                Logger.Error(e.InnerException);
                Logger.Error(e.Message);
            }
            finally
            {
                StopBrowser();
                Logger.Debug(TestContext.TestName);
                Logger.Debug("*************************************** TEST STOPPED");
                Logger.Debug("*************************************** TEST STOPPED");
            }
        }

        private void TakeScreenshotForTestFailure()
        {
            if (ScreenshotTaker != null)
            {
                ScreenshotTaker.CreateScreenshotIfTestFailed();
                Reporter.ReportTestOutcome(ScreenshotTaker.ScreenshotFilePath);
            }
            else
            {
                Reporter.ReportTestOutcome("");
            }
        }

        [TestCleanup]
        public void StopBrowser()
        {
            if (Driver == null)
                return;
            Driver.Close();
            Driver.Quit();
            Driver = null;
            Logger.Trace("Browser stopped successfully.");
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