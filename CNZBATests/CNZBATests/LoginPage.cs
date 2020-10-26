using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace CNZBATests
{
    internal class LoginPage
    {
        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; }

        public IWebElement EmailAddress => Driver.FindElement(By.XPath("//*[@type='text']"));
        public IWebElement PassWord => Driver.FindElement(By.XPath("//*[@type='password']"));
        public IWebElement LoginButton => Driver.FindElement(By.XPath("//*[@class='mat-raised-button mat-button-base mat-warn']"));
        public IWebElement AlertforEmailAddress => Driver.FindElement(By.XPath("//*[@id='mat-error-0']"));
        public IWebElement AlertforPassword => Driver.FindElement(By.XPath("//*[@id='mat-error-1']"));
        public IWebElement LoginToYourAccount => Driver.FindElement(By.XPath("//*[@class='title']"));
       
        internal void Open()
        {
            Driver.Navigate().GoToUrl("https://cbaaccountingwebapptest.azurewebsites.net");
        }

        internal void InputUserInfoAndLogin(TestUser user)
        {
            EmailAddress.SendKeys(user.EmailAddress);
            PassWord.SendKeys(user.PassWord);
            LoginButton.Click();
        }

        internal void NoInput()
        {
            Actions actionsObj = new Actions(Driver);
            //Move the cursor From the user account to password field
            actionsObj.Click(EmailAddress).Perform();
            actionsObj.Click(PassWord).Perform();
            actionsObj.Click(EmailAddress).Perform();
        }
    }
}