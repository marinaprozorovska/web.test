using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CalculatorTests.Tests
{
    [TestFixture]
    public class SettingsPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions { AcceptInsecureCertificates = true };
            driver = new ChromeDriver(options);
            driver.Url = "https://localhost:5001/Calculator";


        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

       [TestCase("dd/MM/yyyy", "123,456,789.00", "$ - US dollar", "$", "100,000.00", "200,000.00", "19/05/2023")]
       [TestCase("dd-MM-yyyy", "123.456.789,00", "€ - euro", "€", "100.000,00", "200.000,00", "19-05-2023")]
       [TestCase("MM/dd/yyyy", "123 456 789.00", "£ - Great Britain Pound", "£", "100 000.00", "200 000.00", "05/19/2023")]
       [TestCase("MM dd yyyy", "123 456 789,00", "₴ - Ukrainian hryvnia", "₴", "100 000,00", "200 000,00", "05 19 2023")]


        public void SettingsTests (string dateFormat, string numberFormat, string defaultCurrency, string amountCurrency, string interestEarned, string income, string endDate)
        {
            IWebElement settingsButton = driver.FindElement(By.XPath("//div[@class = 'settings link btn btn-link']"));
          
            
            settingsButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
             .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("save")));

            IWebElement dateFormatField = driver.FindElement(By.Id("dateFormat"));
            IWebElement numberFormatField = driver.FindElement(By.Id("numberFormat"));
            IWebElement defaultCurrencyField = driver.FindElement(By.Id("currency"));
            IWebElement saveButton = driver.FindElement(By.Id("save"));
            SelectElement dateFormatDropdown = new SelectElement(dateFormatField);
            SelectElement numberFormatDropdown = new SelectElement(numberFormatField);
            SelectElement defaultCurrencyDropdown = new SelectElement(defaultCurrencyField);


            dateFormatDropdown.SelectByText(dateFormat);
            numberFormatDropdown.SelectByText(numberFormat);
            defaultCurrencyDropdown.SelectByText(defaultCurrency);
            saveButton.Click();
            System.Threading.Thread.Sleep(2000);
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();

            IWebElement depositAmountField = driver.FindElement(By.XPath("//tr[contains( string(), 'Deposit Amount')]//input"));
            IWebElement rateOfInterestField = driver.FindElement(By.XPath("//tr[contains( string(), 'Rate of interest')]//input"));
            IWebElement investmentTermField = driver.FindElement(By.XPath("//tr[contains( string(), 'Investment Term')]//input"));
            IWebElement startDayField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[1]"));
            IWebElement startMonthField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[2]"));
            IWebElement startYearField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[3]"));
            IWebElement financialYearButton365 = driver.FindElement(By.XPath("//tr[@id = 'finYear']/td[text() = '365 days']/*"));
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));
            IWebElement amountCurrencySign = driver.FindElement(By.Id("currency"));
            IWebElement interestEarnedField = driver.FindElement(By.XPath("//tr[contains( string(), 'Interest Earned: *')]//input"));
            IWebElement incomeField = driver.FindElement(By.XPath("//tr[contains( string(), 'Income: *')]//input"));
            IWebElement endDateField = driver.FindElement(By.XPath("//tr[contains( string(), 'End Date: *')]//input"));
            SelectElement startDayDropdown = new SelectElement(startDayField);
            SelectElement startMonthDropdown = new SelectElement(startMonthField);
            SelectElement startYearDropdown = new SelectElement(startYearField);

            depositAmountField.SendKeys("100000");
            rateOfInterestField.SendKeys("100");
            investmentTermField.SendKeys("365");
            startDayDropdown.SelectByText("19");
            startMonthDropdown.SelectByText("May");
            startYearDropdown.SelectByText("2022");
            financialYearButton365.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));
            calculateButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));

            Assert.AreEqual(amountCurrency, amountCurrencySign.GetAttribute("textContent"));
            Assert.AreEqual(interestEarned, interestEarnedField.GetAttribute("value"));
            Assert.AreEqual(income, incomeField.GetAttribute("value"));
            Assert.AreEqual(endDate, endDateField.GetAttribute("value"));
        }

        [TestCase("dd/MM/yyyy", "123,456,789.00", "$ - US dollar", "$", "100,000.00", "200,000.00", "19/05/2023")]
        [TestCase("dd-MM-yyyy", "123.456.789,00", "$ - US dollar", "$", "100,000.00", "200,000.00", "19/05/2023")]
        [TestCase("MM/dd/yyyy", "123 456 789.00", "$ - US dollar", "$", "100,000.00", "200,000.00", "19/05/2023")]
        [TestCase("MM dd yyyy", "123 456 789,00", "$ - US dollar", "$", "100,000.00", "200,000.00", "19/05/2023")]


        public void CancelButtonTests(string dateFormat, string numberFormat, string defaultCurrency, string amountCurrency, string interestEarned, string income, string endDate)
        {
            IWebElement settingsButton = driver.FindElement(By.XPath("//div[@class = 'settings link btn btn-link']"));


            settingsButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("save")));

            IWebElement dateFormatField = driver.FindElement(By.Id("dateFormat"));
            IWebElement numberFormatField = driver.FindElement(By.Id("numberFormat"));
            IWebElement defaultCurrencyField = driver.FindElement(By.Id("currency"));
            IWebElement cancelButton = driver.FindElement(By.Id("cancel"));
            SelectElement dateFormatDropdown = new SelectElement(dateFormatField);
            SelectElement numberFormatDropdown = new SelectElement(numberFormatField);
            SelectElement defaultCurrencyDropdown = new SelectElement(defaultCurrencyField);

            dateFormatDropdown.SelectByText(dateFormat);
            numberFormatDropdown.SelectByText(numberFormat);
            defaultCurrencyDropdown.SelectByText(defaultCurrency);
            cancelButton.Click();
            
            IWebElement depositAmountField = driver.FindElement(By.XPath("//tr[contains( string(), 'Deposit Amount')]//input"));
            IWebElement rateOfInterestField = driver.FindElement(By.XPath("//tr[contains( string(), 'Rate of interest')]//input"));
            IWebElement investmentTermField = driver.FindElement(By.XPath("//tr[contains( string(), 'Investment Term')]//input"));
            IWebElement startDayField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[1]"));
            IWebElement startMonthField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[2]"));
            IWebElement startYearField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[3]"));
            IWebElement financialYearButton365 = driver.FindElement(By.XPath("//tr[@id = 'finYear']/td[text() = '365 days']/*"));
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));
            IWebElement amountCurrencySign = driver.FindElement(By.Id("currency"));
            IWebElement interestEarnedField = driver.FindElement(By.XPath("//tr[contains( string(), 'Interest Earned: *')]//input"));
            IWebElement incomeField = driver.FindElement(By.XPath("//tr[contains( string(), 'Income: *')]//input"));
            IWebElement endDateField = driver.FindElement(By.XPath("//tr[contains( string(), 'End Date: *')]//input"));
            SelectElement startDayDropdown = new SelectElement(startDayField);
            SelectElement startMonthDropdown = new SelectElement(startMonthField);
            SelectElement startYearDropdown = new SelectElement(startYearField);

            depositAmountField.SendKeys("100000");
            rateOfInterestField.SendKeys("100");
            investmentTermField.SendKeys("365");
            startDayDropdown.SelectByText("19");
            startMonthDropdown.SelectByText("May");
            startYearDropdown.SelectByText("2022");
            financialYearButton365.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));
            calculateButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));

            Assert.AreEqual(amountCurrency, amountCurrencySign.GetAttribute("textContent"));
            Assert.AreEqual(interestEarned, interestEarnedField.GetAttribute("value"));
            Assert.AreEqual(income, incomeField.GetAttribute("value"));
            Assert.AreEqual(endDate, endDateField.GetAttribute("value"));
        }

        [Test]
        public void LogoutTest()
        {
            IWebElement settingsButton = driver.FindElement(By.XPath("//div[@class = 'settings link btn btn-link']"));
            settingsButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("save")));

            IWebElement logoutButton = driver.FindElement(By.XPath("//div[@class = 'login link btn btn-link']"));
            logoutButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2000))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//tr/td[@id='buttons']/button[@class='btn btn-sm btn-success']")));

            IWebElement loginButton = driver.FindElement(By.Id("loginBtn"));


            Assert.IsTrue(loginButton.Displayed);
        }
    }
}