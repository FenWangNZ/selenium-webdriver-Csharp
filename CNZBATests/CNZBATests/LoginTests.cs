using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace CNZBATests
{
    [TestClass]
    [TestCategory("LoginModule")]
    public class LoginTests : BaseTest
    {
        LoginPage loginPage;
        TestUser userInfo;

        [TestMethod]
        [Description("Validate that the user is able to login successfully using validate data")]
        public void Test_1()
        {
            UserLogin();
        }

        public void UserLogin()
        {
            userInfo = new TestUser
            {
                EmailAddress = "guest@guest.com",
                PassWord = "q1111111"
            };
            loginPage = new LoginPage(Driver);
            loginPage.Open();
            Driver.Manage().Window.Maximize();

            loginPage.InputUserInfoAndLogin(userInfo);
            Assert.AreEqual("CBA Invoicing", Driver.Title);
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
            Driver.SwitchTo().Alert().Accept();
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
            Driver.SwitchTo().Alert().Accept(); 
            Assert.AreEqual("Login to Your Account!", loginPage.LoginToYourAccount.Text);
        }

    }
}
