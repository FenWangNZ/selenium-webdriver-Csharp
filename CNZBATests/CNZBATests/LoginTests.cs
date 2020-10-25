using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CNZBATests
{
    [TestClass]
    [TestCategory("LoginModule")]
    public class LoginTests
    {
        [TestMethod]
        [Description("Validate that the user is able to login successfully using validate date")]
        public void Test_1()
        {
            var userInfo = new TestUser();
            userInfo.EmailAddress = "guest@guest.com";
            userInfo.PassWord = "q1111111";

            var driver = ChromeDriver();
            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open();
            Thread.Sleep(1000);
            driver.Manage().Window.Maximize();

            loginPage.InputUserInfoAndLogin(userInfo);
            Assert.AreEqual("CBA Invoicing", driver.Title);

            driver.Close();
            driver.Quit();

        }
        [TestMethod]
        [Description("Validate no input")]
        public void Test_0_NoInput()
        {
            var driver = ChromeDriver();
            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open();
            Thread.Sleep(1000);
            driver.Manage().Window.Maximize();

            loginPage.NoInput();
            Assert.IsTrue(loginPage.AlertforEmailAddress.Text.Contains("You Must Enter A Value."));
            Assert.IsTrue(loginPage.AlertforPassword.Text.Contains("You Must Enter A Value."));
            Assert.IsFalse(loginPage.LoginButton.Enabled);

            driver.Close();
            driver.Quit();
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
