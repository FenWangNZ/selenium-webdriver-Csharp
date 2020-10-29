using OpenQA.Selenium;

namespace CNZBATests
{
    public class BasePage
    {
        protected IWebDriver Driver { get; set; }
        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
