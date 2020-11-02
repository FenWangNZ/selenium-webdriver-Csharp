using System;
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
        public IWebElement LoginButton => Driver.FindElement(By.XPath("//*[@class='mat-raised-button mat-button-base mat-warn']"));
        public IWebElement AlertforEmailAddress => Driver.FindElement(By.XPath("//*[@id='mat-error-0']"));
        public IWebElement AlertforPassword => Driver.FindElement(By.XPath("//*[@id='mat-error-1']"));
        public IWebElement LoginToYourAccount => Driver.FindElement(By.XPath("//*[@class='title']"));
       
        internal void Open()
        {
            Driver.Navigate().GoToUrl(URL.LoginUrl);
            _logger.Info($"Opened url=>{URL.LoginUrl}");
        }

        internal void InputUserInfoAndLogin(TestUser user)
        {
            EmailAddress.SendKeys(user.EmailAddress);
            _logger.Info($"Input email address=>{user.EmailAddress}");
            PassWord.SendKeys(user.PassWord);
            _logger.Info($"Input password=>{user.PassWord}");

            LoginButton.Click();
            //Thread.Sleep(6000);
        }

        internal void NoInput()
        {
            Actions actionsObj = new Actions(Driver);
            //Move the cursor From the user account to password field
            actionsObj.Click(EmailAddress).Perform();
            _logger.Info($"Inputed no mail address=>{EmailAddress}");
            actionsObj.Click(PassWord).Perform();
            _logger.Info($"Inputed no password=>{PassWord}");
            actionsObj.Click(EmailAddress).Perform();
            _logger.Info($"moved mouse back to login field");
        }
    }
}