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
        IWebDriver driver;
        LoginPage loginPage;
        TestUser userInfo;

        [TestInitialize]
        public void SetupBeforEverySingleMethod()
        {
            driver = ChromeDriver();
            loginPage = new LoginPage(driver);
            loginPage.Open();
            driver.Manage().Window.Maximize();
            userInfo = new TestUser();
        }

        [TestCleanup]
        public void CleanupAfterEverySingleMethod()
        {
            driver.Close();
            driver.Quit();
        }

        [TestMethod]
        [Description("Validate that the user is able to login successfully using validate data")]
        public void Test_1()
        {
            userInfo.EmailAddress = "guest@guest.com";
            userInfo.PassWord = "q1111111";

            loginPage.InputUserInfoAndLogin(userInfo);
            Assert.AreEqual("CBA Invoicing", driver.Title);
        }

        [TestMethod]
        [Description("Validate alert if there is no input into email address and password fields")]
        public void Test_0_NoInput()
        {
            loginPage.NoInput();
            Assert.IsTrue(loginPage.AlertforEmailAddress.Text.Contains("You Must Enter A Value."));
            Assert.IsTrue(loginPage.AlertforPassword.Text.Contains("You Must Enter A Value."));
            Assert.IsFalse(loginPage.LoginButton.Enabled);

        }

        [TestMethod]
        [Description("Validate that the user will get an alert while only inputting password field ")]
        public void Test_0_OnlyInputPassword()
        {
            userInfo.PassWord = "q1111111";
            loginPage.PassWord.SendKeys(userInfo.PassWord);
            loginPage.LoginButton.Click();
            Assert.IsFalse(loginPage.LoginButton.Enabled);
        }

        [TestMethod]
        [Description("Validate that the user will get an alert while only inputting email address field ")]
        public void Test_0_OnlyInputEmailAddress()
        {
            userInfo.EmailAddress = "guest@guest.com";
            loginPage.EmailAddress.SendKeys(userInfo.EmailAddress);
            loginPage.LoginButton.Click();
            Assert.IsFalse(loginPage.LoginButton.Enabled);
        }

        [TestMethod]
        [Description("Validate that the user is not able to login successfully using invalid data")]
        public void Test_0_WrongPassword()
        {
            userInfo.EmailAddress = "guest@guest.com";
            userInfo.PassWord = "guest111";

            loginPage.InputUserInfoAndLogin(userInfo);
            Thread.Sleep(3000);
            driver.SwitchTo().Alert().Accept();
            Assert.AreEqual("Login to Your Account!", loginPage.LoginToYourAccount.Text);
        }

        [TestMethod]
        [Description("Validate that the user will get an alert if inputting an inexisting email address")]
        public void Test_0_InputWrongEmailAddress()
        {
            userInfo.EmailAddress = "guest@opeu.com";
            userInfo.PassWord = "guest111";

            loginPage.InputUserInfoAndLogin(userInfo);
            Thread.Sleep(3000);
            driver.SwitchTo().Alert().Accept();
            Assert.AreEqual("Login to Your Account!", loginPage.LoginToYourAccount.Text);
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
