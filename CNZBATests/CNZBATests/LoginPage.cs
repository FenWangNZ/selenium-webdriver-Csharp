using System;
using AventStack.ExtentReports;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;


namespace CNZBATests
{
    internal class LoginPage : BasePage
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public LoginPage(IWebDriver driver) : base(driver) { }

        public IWebElement EmailAddress => Driver.FindElement(By.XPath("//*[@type='text']"));
        public IWebElement PassWord => Driver.FindElement(By.XPath("//*[@type='password']"));
        public IWebElement LoginButton => Driver.FindElement(By.XPath("//*[@class='login-btn mat-raised-button mat-button-base mat-warn']"));
        public IWebElement AlertforEmailAddress => Driver.FindElement(By.XPath("//*[@id='mat-error-0']"));
        public IWebElement AlertforPassword => Driver.FindElement(By.XPath("//*[@id='mat-error-1']"));
        public IWebElement LoginToYourAccount => Driver.FindElement(By.XPath("//*[@class='title']"));
       
        internal void Open()
        {
            Driver.Navigate().GoToUrl(URL.LoginUrl);
            Reporter.LogPassingTestStepToBugLogger($"Navigate to Login Page at url=>{URL.LoginUrl}");
        }

        internal void InputUserInfoAndLogin(TestUser user)
        {
            EmailAddress.SendKeys(user.EmailAddress);
            Reporter.LogTestStepForBugLogger(Status.Info, $"Input email address=>{user.EmailAddress}");
            //_logger.Info($"Input email address=>{user.EmailAddress}");
            PassWord.SendKeys(user.PassWord);
            Reporter.LogTestStepForBugLogger(Status.Info, $"Input password=>{user.PassWord}");
            //_logger.Info($"Input password=>{user.PassWord}");

            LoginButton.Click();
            //Thread.Sleep(6000);
        }

        internal void NoInput()
        {
            Actions actionsObj = new Actions(Driver);
            //Move the cursor From the user account to password field
            actionsObj.Click(EmailAddress).Perform();
            Reporter.LogTestStepForBugLogger(Status.Info,
                $"Inputed no mail address=>{EmailAddress}");
            //_logger.Info($"Inputed no mail address=>{EmailAddress}");
            actionsObj.Click(PassWord).Perform();
            Reporter.LogTestStepForBugLogger(Status.Info, $"Inputed no password=>{PassWord}"); 
            //_logger.Info($"Inputed no password=>{PassWord}");
            actionsObj.Click(EmailAddress).Perform();
            Reporter.LogTestStepForBugLogger(Status.Info, $"moved mouse back to login field");
            //_logger.Info($"moved mouse back to login field");
        }
    }
}