using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using OpenQA.Selenium;

namespace CNZBATests
{
    internal class InvoiceListsPage : BasePage
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public InvoiceListsPage(IWebDriver driver) : base(driver) { }
        IReadOnlyList<IWebElement> DefaultLocators => Driver.FindElements(By.XPath("//*[@class='col-6']"));
        IReadOnlyList<IWebElement> ValueUpdatedOnTopRight => Driver.FindElements(By.XPath("//*[@class='col-6']"));
        
        internal void AssertInvoiceInfoOnInvoiceLists(InvoicePage invoicePage)
        {
            //verify if new added invoice can be listed on invoice tracker page by query this invoice
            string invoiceNumber = DefaultLocators[1].Text;
            
            string newTotalOnTopRight = ValueUpdatedOnTopRight[5].Text;
            Driver.Navigate().GoToUrl(URL.InvoiceListsUrl);
            Thread.Sleep(2000);
            invoicePage.SearchField.SendKeys(invoiceNumber);
            //verify the ivoice is the searched one
            Assert.AreEqual(invoiceNumber, invoicePage.InvoiceNumberOfNewAdded.Text);
            Assert.AreEqual(invoicePage.currentDate, invoicePage.InvoiceCreationDateOfNewAdded.Text);
            Assert.AreEqual(invoicePage.clientName, invoicePage.InvoiceClientOfNewAdded.Text);
            string newTotalOnTopRightWithDollarNtion = "$" + newTotalOnTopRight;
            Assert.AreEqual(newTotalOnTopRightWithDollarNtion, invoicePage.InvoiceTotalAmountOfNewAdded.Text);
            /*
             * Get omvoice due date on the list
             * string list_duedate = driver.FindElement(By.XPath("//[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[5]")).Text;
             * Assert.AreEqual(duedate, list_duedate);
             * Get invoice status on the list
             */          
            Assert.AreEqual("Draft", invoicePage.InvoiceStatus.Text);
            Thread.Sleep(2000);
        }
    }
}