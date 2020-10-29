using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace CNZBATests
{
    [TestClass]
    [TestCategory("InvoiceModule")]
    public class InvoiceTests : BaseTest
    {
        LoginPage loginPage;
        InvoicePage invoicePage;
        TestUser userInfo;
        InvoiceListsPage invoiceListsPage;

        [TestMethod]
        [Description("Create a non-zero amount invoice, including testing the 2 hidden buttons")]
        public void CreateNewInvoice()
        {
            UserLogin();
            invoicePage = new InvoicePage(Driver);
            invoicePage.Open();

            invoicePage.AssertDefaultValuesOnInvoiceCreationPage();
            invoicePage.CreateAnInvoice();

            invoiceListsPage = new InvoiceListsPage(Driver);
            invoiceListsPage.AssertInvoiceInfoOnInvoiceLists(invoicePage);

        }

        private void UserLogin()
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
        [Description("Cancel an Invoice in two ways")]
        public void CancelInvoiceEditing()
        {
            UserLogin();
            //Go to invoice creation page, click on cancel button
            Driver.Navigate().GoToUrl(URL.NewInvoiceUrl);
            Thread.Sleep(1000);
            Driver.FindElements(By.XPath("//*[@class='btn btn-secondary']"))[0].Click();
            Thread.Sleep(1000);
            Assert.IsTrue(Driver.FindElements(By.XPath("//*[@class='btn btn-lg btn-success']"))[0].Displayed);
            Thread.Sleep(1000);
            //Go to invoice creation page. Do some changes, then click on cancel->yes button.
            Driver.FindElements(By.XPath("//*[@class='btn btn-lg btn-success']"))[0].Click();
            Thread.Sleep(2000);
            Driver.FindElement(By.XPath("//*[@name='clientname']")).Clear();
            Driver.FindElement(By.XPath("//*[@name='clientname']")).SendKeys("abc");
            Thread.Sleep(2000);
            Driver.FindElements(By.XPath("//*[@class='btn btn-secondary']"))[0].Click();
            Thread.Sleep(5000);
            //assert if modal if visalble.
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@class='modal-content']")).Displayed);
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@class='modal-body text-center']")).Text.Contains("All unsaved changes will be lost. Are you sure you want to cancel?"));
            //click on yes, then go back to the previous page.
            Driver.FindElement(By.XPath("//*[@class='btn btn-default']")).Click();
            Thread.Sleep(2000);
            //assert if page goes to previouse page
            Assert.IsTrue(Driver.FindElements(By.XPath("//*[@class='btn btn-lg btn-success']"))[0].Displayed);
            Thread.Sleep(2000);
            //Go to invoice creation page. Do some changes, then click on the cancel button -> click no button.
            Driver.FindElements(By.XPath("//*[@class='btn btn-lg btn-success']"))[0].Click();
            Thread.Sleep(2000);
            Driver.FindElement(By.XPath("//*[@name='clientname']")).Clear();
            Driver.FindElement(By.XPath("//*[@name='clientname']")).SendKeys("abc");
            Thread.Sleep(2000);
            string url_0 = Driver.Url;
            Driver.FindElements(By.XPath("//*[@class='btn btn-secondary']"))[0].Click();
            Thread.Sleep(5000);
            //click on no, then stay where it was.
            Driver.FindElement(By.XPath("//*[@class='btn btn-primary']")).Click();
            Assert.AreEqual(url_0, Driver.Url);
            //Thread.Sleep(1000);
        }
        [TestMethod]
        [Description("Delete an Invoice in two ways")]
        public void DeleteInvoice()
        {
            UserLogin();
            //Go to invoice lists page, click on delete button
            Driver.Navigate().GoToUrl(URL.InvoiceListsUrl);
            Thread.Sleep(6000);
            //(1)Select a draft invoice, click on delete button beside invoice number
            string invoice_number_to_delete = Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[1]")).Text;
            Driver.FindElements(By.XPath("//*[@class='btn btn-sm']"))[0].Click();
            Thread.Sleep(5000);
            string url_delete0 = Driver.Url;
            //check the modal is available.
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@id='deleteModal']")).Displayed);
            Thread.Sleep(1000);
            //check the modal is right displayed
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@class='modal-body']")).Text.Contains(invoice_number_to_delete));
            Thread.Sleep(1000);
            //a)click on No button
            Driver.FindElement(By.XPath("//*[@class='btn btn-secondary']")).Click();
            Thread.Sleep(1000);
            //Check page remains
            Assert.AreEqual(url_delete0, Driver.Url);
            //check the invoice is not deleted
            Assert.IsTrue(Driver.PageSource.Contains(invoice_number_to_delete));
            //b)click delete button, then click on Yes button
            Driver.FindElements(By.XPath("//*[@class='btn btn-sm']"))[0].Click();
            Thread.Sleep(5000);
            Driver.FindElement(By.XPath("//*[@class='btn btn-danger']")).Click();
            Thread.Sleep(5000);
            //Check page remains
            Assert.AreEqual(url_delete0, Driver.Url);
            //check the invoice has been deleted
            Assert.IsFalse(Driver.PageSource.Contains(invoice_number_to_delete));
            Thread.Sleep(1000);
            //(2)click on delete invoice button on invoice details page
            //select a drafted invoice
            string invoice_number_detail_delete = Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[1]")).Text;
            //click the invoice number
            Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[1]")).Click();
            Thread.Sleep(2000);
            string url_delete1 = Driver.Url;
            //click on the delete button
            Driver.FindElement(By.XPath("//*[@class='btn btn-danger text-nowrap ng-star-inserted']")).Click();
            Thread.Sleep(5000);
            //check the modal is available.
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@id='deleteModal']")).Displayed);
            Thread.Sleep(1000);
            //check the modal is right displayed
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@class='modal-body']")).Text.Contains(invoice_number_detail_delete));
            Thread.Sleep(1000);
            //a)click on No button
            Driver.FindElements(By.XPath("//*[@class='btn btn-secondary']"))[2].Click();
            Thread.Sleep(1000);
            //Check page remains
            Assert.AreEqual(url_delete1, Driver.Url);
            //check the invoice is not deleted
            Assert.IsTrue(Driver.PageSource.Contains(invoice_number_detail_delete));
            //b)click on delete button and click on Yes button
            //click on the delete button
            Driver.FindElement(By.XPath("//*[@class='btn btn-danger text-nowrap ng-star-inserted']")).Click();
            Thread.Sleep(2000);
            Driver.FindElement(By.XPath("//*[@class='btn btn-danger']")).Click();
            Thread.Sleep(5000);
            //Check page remains
            Assert.AreEqual(url_delete0, Driver.Url);
            //check the invoice has been deleted
            Assert.IsFalse(Driver.PageSource.Contains(invoice_number_detail_delete));
            Thread.Sleep(1000);
        }
        [TestMethod]
        [Description("Finalise and send an Invoice in two ways")]
        public void FinaliseSendInvoice()
        {
            UserLogin();
            //Go to invoice lists page
            Driver.Navigate().GoToUrl(URL.InvoiceListsUrl);
            string url_send0 = Driver.Url;
            Thread.Sleep(6000);
            //Select a drafted invoice
            string invoice_number_detail_send = Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[1]")).Text;
            //Click on the invoice number to the invoice detail page
            Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[1]")).Click();
            Thread.Sleep(2000);
            string url_send1 = Driver.Url;
            //click on the finalise&send invoice button
            Driver.FindElement(By.XPath("//*[@class='btn btn-primary ng-star-inserted']")).Click();
            Thread.Sleep(5000);
            //check the modal is available.
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@class='modal-dialog modal-md']")).Displayed);
            Thread.Sleep(1000);
            //check the modal is right displayed
            Assert.IsTrue(Driver.FindElement(By.XPath("//*[@class='modal-body']")).Text.Contains(invoice_number_detail_send));
            Thread.Sleep(1000);
            //Method1:click on No button
            Driver.FindElements(By.XPath("//*[@class='btn btn-secondary']"))[2].Click();
            Thread.Sleep(1000);
            //Check page remains
            Assert.AreEqual(url_send1, Driver.Url);
            //check the invoice is not sent
            Assert.IsTrue(Driver.PageSource.Contains(invoice_number_detail_send));

            //Method2:click on finalise&send button and click on Yes button
            //click on the finalise&send button
            Driver.FindElement(By.XPath("//*[@class='btn btn-primary ng-star-inserted']")).Click();
            Thread.Sleep(2000);
            Driver.FindElement(By.XPath("//*[@class='btn btn-primary']")).Click();
            Thread.Sleep(20000);
            //Check page goes to invoice tracker page
            Assert.AreEqual(url_send0, Driver.Url);
            //check the invoice has been deleted
            Assert.AreEqual("Issued", Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[6]")).Text);
            Thread.Sleep(1000);
        }

        [TestMethod]
        [Description("Download an sent Invoice through pdf icon and open it")]
        public void DownloadPdfinvoice()
        {
            UserLogin();
            //Go to invoice lists page
            Driver.Navigate().GoToUrl(URL.InvoiceListsUrl);
            Thread.Sleep(6000);
            //Select an Issued invoice, Get this invoice number
            string invoice_number_downloaded = Driver.FindElement(By.XPath("//*[@class='table table-bordered table-striped table-hover table-light']/tbody/tr[1]/td[1]")).Text;
            //Click PDF icon and download it
            Driver.FindElement(By.XPath("//*[@id='pdf_ico']")).Click();
            Thread.Sleep(6000);
            //Get download path
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            DirectoryInfo downloadDir = new DirectoryInfo(pathDownload);
            //get files in this path
            FileInfo[] files = downloadDir.GetFiles("*.pdf");
            //get full path name of the first/latest files
            var filename = files[0].FullName;
            //get filename
            string getFileName = Path.GetFileName(filename);
            //Assert filename if it contains invoice number
            if (File.Exists(filename))
            {
                Assert.IsTrue(filename.Contains(invoice_number_downloaded));
            }
            File.Delete(filename);
        }
    }
}