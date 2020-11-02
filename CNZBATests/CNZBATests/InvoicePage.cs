using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace CNZBATests
{
    internal class InvoicePage : BasePage
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public InvoicePage(IWebDriver driver) : base(driver) { }
       
        private const string CNZBA = "ABNZ";
        private const string TotalLength = "10";
        private const string FinalSix = "6";
        private const string ZeroMoney = "0.00";
        private const string DefaultStatus = "New";

        public string clientName = "Stephen";
        string email = "zhiwen.you@gmail.com";
        string purchaOrderNumber = "0123456789";
        string clientContact = "22 Rudleigh Ave";

        string description1 = "Desks";
        string description2 = "Chairs";
        string quantity1 = "20";
        string unitPrice1 = "12";
        string quantity2 = "40";
        string unitPrice2 = "20";

        public string currentDate = DateTime.Today.ToString("d/MM/yyyy");

        IReadOnlyList<IWebElement> DefaultLocators => Driver.FindElements(By.XPath("//*[@class='col-6']"));
        IReadOnlyList<IWebElement> Values => Driver.FindElements(By.XPath("//*[@class='col-2 text-right']"));
        IReadOnlyList<IWebElement> Items => Driver.FindElements(By.XPath("//*[@class='btn btn-link btn-sm']"));
        IReadOnlyList<IWebElement> Columns => Driver.FindElements(By.XPath("//*[@class='btn btn-outline-danger btn-sm']"));
        //IReadOnlyList<IWebElement> Quantity => Driver.FindElements(By.XPath("//*[@class='form-control text-right ng-untouched ng-pristine ng-valid']"));
        IReadOnlyList<IWebElement> ValueUpdatedOnTopRight => Driver.FindElements(By.XPath("//*[@class='col-6']"));
        IReadOnlyList<IWebElement> ValueUpdatedOnTheBottom => Driver.FindElements(By.XPath("//*[@class='col-2 text-right']"));

        public IWebElement ClientName => Driver.FindElement(By.XPath("//*[@name='clientname']"));
        public IWebElement Email => Driver.FindElement(By.XPath("//*[@name='email']"));
        public IWebElement PurchaseOrderNumber => Driver.FindElement(By.XPath("//*[@name='purchaseOrderNumber']"));
        public IWebElement AddAddressButton => Driver.FindElement(By.XPath("//*[@class='btn btn-sm btn-link']"));
        public IWebElement RemoveAddressButton => Driver.FindElement(By.XPath("//*[@id='invoicelistscreen']/form/div[2]/div[1]/div[5]/div[3]/button"));

        public IWebElement ClientContact => Driver.FindElement(By.XPath("//*[@name='clientcontact']"));
        public IWebElement DeleteItemButton => Driver.FindElement(By.XPath("//*[@class='btn btn-outline-danger btn-sm']"));

        public IWebElement Description1 => Driver.FindElement(By.XPath("//*[@id='invoicelistscreen']/form/div[5]/div[1]/input"));
        public IWebElement Description2 => Driver.FindElement(By.XPath("//*[@id='invoicelistscreen']/form/div[6]/div[1]/input"));

        public IWebElement SaveInvoiceButton => Driver.FindElement(By.XPath("//*[@type='submit']"));
        public IWebElement FinalisedAndSendButton => Driver.FindElement(By.XPath("//*[@class='btn btn-primary ng-star-inserted']"));

        public IWebElement SearchField => Driver.FindElement(By.XPath("//*[@name='searchString']"));
        public IWebElement InvoiceNumberOfNewAdded => Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[1]"));
        public IWebElement InvoiceCreationDateOfNewAdded => Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[2]"));
        public IWebElement InvoiceClientOfNewAdded => Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[3]"));
        public IWebElement InvoiceTotalAmountOfNewAdded => Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[4]"));
        public IWebElement InvoiceStatus => Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[6]"));

        
        internal void Open()
        {
            Driver.Navigate().GoToUrl(URL.NewInvoiceUrl);
            _logger.Info($"Opened url=>{URL.NewInvoiceUrl}");
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(6));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@type='submit']")));
        }

        public void AssertDefaultValuesOnInvoiceCreationPage()
        {
            //verify invoice number
            string invoiceNumber = DefaultLocators[1].Text;
            Assert.AreEqual(TotalLength, Convert.ToString(invoiceNumber.Length));
            _logger.Info($"Verified the total length of default invoice number=>{invoiceNumber}");
            string firstFourDigits = invoiceNumber.Substring(0, 4);
            string finalSixDigits = invoiceNumber.Substring(4);
            Assert.AreEqual(CNZBA, firstFourDigits);
            Assert.AreEqual(FinalSix, Convert.ToString(finalSixDigits.Length));
            _logger.Info($"Verified first four digits of default invoice number=>{firstFourDigits}");
            _logger.Info($"Verified final six digits of default invoice number=>{finalSixDigits}");
            //verify current date.

            Assert.IsTrue(DefaultLocators[3].Text.Contains(currentDate));
            _logger.Info($"Got current date=>{currentDate}");
            _logger.Info($"Verified default date on invoice creation page=>{DefaultLocators[3].Text}");
            Thread.Sleep(6000);
            //verify defaut value for totals and GSTs
            string totalAmount1 = DefaultLocators[5].Text;
            string totalAmount2 = Values[1].Text;
            
            string GST1 = DefaultLocators[7].Text;
            string GST2 = Values[2].Text;
            Assert.AreEqual(ZeroMoney, totalAmount1);
            Assert.AreEqual(totalAmount1, totalAmount2);
            _logger.Info($"Default Total amount was =>{totalAmount1} and {totalAmount2}");
            Assert.AreEqual(ZeroMoney, GST1);
            Assert.AreEqual(GST1, GST2);
            _logger.Info($"Default GST amount was =>{GST1} and {GST2}");
            Thread.Sleep(2000);
            //verify the defalt status is New
            Assert.AreEqual(DefaultStatus, DefaultLocators[9].Text);
            _logger.Info($"Got default status =>{DefaultStatus}");
            Thread.Sleep(2000);
            //verify the due date value is two weeks after current date;
        }

        public void CreateAnInvoice()
        {
            //input client name
            ClientName.SendKeys(clientName);
            _logger.Info($"Inputed client name =>{clientName}");
            Email.SendKeys(email);
            _logger.Info($"Inputed Email =>{email}");
            PurchaseOrderNumber.SendKeys(purchaOrderNumber);
            _logger.Info($"Inputed purcha order number =>{purchaOrderNumber}");

            //open add address button and deleted this field then add it again
            AddAddressButton.Click();
            _logger.Info($"Address button was added");
            RemoveAddressButton.Click();
            _logger.Info($"Address button was removed");
            Assert.IsTrue(AddAddressButton.Enabled);
            _logger.Info($"Address button was removed successfully");
            AddAddressButton.Click();
            _logger.Info($"Address button was added again");
            ClientContact.SendKeys(clientContact);
            _logger.Info($"Inputed clinet contact =>{clientContact}");

            //open add item button and deleted this field then add it again
            Items[0].Click();
            Actions actionsObj = new Actions(Driver);
            actionsObj.MoveToElement(DeleteItemButton).Perform();
            Assert.IsTrue(Columns[0].Enabled);
            actionsObj.Click(DeleteItemButton).Perform();

            bool present;
            try
            {
                Driver.FindElement(By.XPath("//*[@class='btn btn-outline-danger btn-sm']"));
                present = true;
            }
            catch (NoSuchElementException)
            {
                present = false;
            }

            Assert.IsFalse(present);

            Items[0].Click();
            Thread.Sleep(3000);
            //add descriptions
            //find description element
            Description1.SendKeys(description1);

            Thread.Sleep(1000);
            //add quantity and unit prices
            IReadOnlyList<IWebElement> Quantity = Driver.FindElements(By.XPath("//*[@class='form-control text-right ng-untouched ng-pristine ng-valid']"));

            Quantity[0].SendKeys(quantity1);
            Quantity[1].SendKeys(unitPrice1);
            Thread.Sleep(1000);

            //add more items
            Items[1].Click();
            Thread.Sleep(10000);
            //add descriptions
            Description2.SendKeys(description2);
            Thread.Sleep(1000);
            //add quantity and unit prices

            IReadOnlyList<IWebElement> Quantity1 = Driver.FindElements(By.XPath("//*[@class='form-control text-right ng-untouched ng-pristine ng-valid']"));
            
            Quantity1[0].SendKeys(quantity2);
            Quantity1[1].SendKeys(unitPrice2);
            Thread.Sleep(1000);
            //verify the totals and gsts after adding items
            
            string newTotalOnTopRight = ValueUpdatedOnTopRight[5].Text;
            string newTotalOnTheBottom = ValueUpdatedOnTheBottom[1].Text;
            string newGSTOnTopRight = ValueUpdatedOnTopRight[7].Text;
            string newGSTOnTheBottom = ValueUpdatedOnTheBottom[2].Text;
            Assert.AreEqual("1,040.00", newTotalOnTopRight);
            Assert.AreEqual(newTotalOnTopRight, newTotalOnTheBottom);
            Assert.AreEqual("156.00", newGSTOnTopRight);
            Assert.AreEqual(newGSTOnTopRight, newGSTOnTheBottom);
            Thread.Sleep(2000);
            SaveInvoiceButton.Click();
            Thread.Sleep(6000);
       
            //Firstly, verify if the status is draft
            Assert.AreEqual("Draft", ValueUpdatedOnTopRight[9].Text);
            Thread.Sleep(2000);
            //if invoice total =0, stay on the current page, and no new button is added.
            
            if (Convert.ToInt32(Convert.ToDouble(newTotalOnTopRight)) > 0)
            {
                Assert.IsTrue(FinalisedAndSendButton.Enabled);
                Thread.Sleep(2000);

            }
            else
            {
                //if invoice total >0, go to the new page, and verify if there is a new button called"finalise & send invoice" on the new page
                Assert.IsFalse(FinalisedAndSendButton.Enabled);
                Thread.Sleep(2000);
            };
        }
    }
}