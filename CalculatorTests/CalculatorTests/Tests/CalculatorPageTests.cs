using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace CalculatorTests.Tests
{
    [TestFixture]
    public class CalculatorPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions { AcceptInsecureCertificates = true };
            driver = new ChromeDriver(options);
            SetSettingsToDefault();
            driver.Url = "https://localhost:5001/Calculator";
        }


        public void SetSettingsToDefault()
        {
            driver.Url = "https://localhost:5001/Settings";

            IWebElement dateFormatField = driver.FindElement(By.Id("dateFormat"));
            IWebElement numberFormatField = driver.FindElement(By.Id("numberFormat"));
            IWebElement defaultCurrencyField = driver.FindElement(By.Id("currency"));
            IWebElement saveButton = driver.FindElement(By.Id("save"));
            SelectElement dateFormatDropdown = new SelectElement(dateFormatField);
            SelectElement numberFormatDropdown = new SelectElement(numberFormatField);
            SelectElement defaultCurrencyDropdown = new SelectElement(defaultCurrencyField);

            dateFormatDropdown.SelectByText("dd/MM/yyyy");
            numberFormatDropdown.SelectByText("123,456,789.00");
            defaultCurrencyDropdown.SelectByText("$ - US dollar");
            saveButton.Click();
            System.Threading.Thread.Sleep(2000);
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void DepositAmountFieldBugTest()
        {
            //arrange
            IWebElement depositAmountField = driver.FindElement(By.XPath("//td [text() = 'Deposit Amount: *']"));

            //assert
            Assert.AreEqual("Deposit Amount: *", depositAmountField.Text);

        }

        [Test]
        public void InterestEarnedOutputBugTest()
        {
            //arrange
            IWebElement interestEarnedOutput = driver.FindElement(By.XPath("//th [text() = 'Interest Earned: *']"));

            //assert
            Assert.AreEqual("Interest Earned: *", interestEarnedOutput.Text);

        }

        [Test]
        public void IncomeOutputBugTest()
        {
            //arrange
            IWebElement incomeOutput = driver.FindElement(By.XPath("//th [text() = 'Income: *']"));

            //assert
            Assert.AreEqual("Income: *", incomeOutput.Text);
        }


        [Test]
        public void OrderOfAprilMonthBugTest()
        {
            //arrange
            IWebElement monthDropdown = driver.FindElement(By.Id("month"));

            //act
            monthDropdown.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("month")));
            IWebElement aprilMonth = monthDropdown.FindElements(By.TagName("option"))[3];

            //assert
            Assert.AreEqual("April", aprilMonth.Text);
        }

        [Test]
        public void OrderOfMonthTest()
        {
            List<string> expectedMonths = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<string> actualMonths = new List<string>();
            IWebElement startMonthField = driver.FindElement(By.Id("month"));
            SelectElement startMonthDropdown = new SelectElement(startMonthField);
            foreach (IWebElement element in startMonthDropdown.Options)
            {
                actualMonths.Add(element.Text);
            }
            Assert.AreEqual(expectedMonths, actualMonths);
           //* Assert.AreEqual(12, startMonthDropdown.Options.Count);
        }

        [Test]
        public void JuneMonthDisplayBugTest()
        {
            //arrange
            IWebElement monthDropdown = driver.FindElement(By.Id("month"));
            IWebElement juneMonth = monthDropdown.FindElements(By.TagName("option"))[5];

            //act
            monthDropdown.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("month")));

            //assert
            Assert.AreEqual("June", juneMonth.Text);
        }

        [Test]
        public void FinancialYearButtonMandatoryBugTest()
        {
            //arrange
            IWebElement depositAmountField = driver.FindElement(By.Id("amount"));
            IWebElement rateOfInterestField = driver.FindElement(By.Id("percent"));
            IWebElement investmentTermField = driver.FindElement(By.Id("term"));
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));

            //act
            depositAmountField.SendKeys("100");
            rateOfInterestField.SendKeys("100");
            investmentTermField.SendKeys("100");

            //assert
            Assert.IsFalse(calculateButton.Enabled);
        }

        [Test]
        public void EndDateBugTest()
        { //arrange
            IWebElement depositAmountField = driver.FindElement(By.XPath("//tr[contains( string(), 'Deposit Amount')]//input"));
            IWebElement rateOfInterestField = driver.FindElement(By.XPath("//tr[contains( string(), 'Rate of interest')]//input"));
            IWebElement investmentTermField = driver.FindElement(By.XPath("//tr[contains( string(), 'Investment Term')]//input"));
            IWebElement startDayField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[1]"));
            IWebElement startMonthField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[2]"));
            IWebElement startYearField = driver.FindElement(By.XPath("//tr[contains( string(), 'Start Date:')]//select[3]"));
            IWebElement financialYearButton365 = driver.FindElement(By.XPath("//tr[@id = 'finYear']/td[text() = '365 days']/*"));
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));
            IWebElement interestEarnedField = driver.FindElement(By.XPath("//tr[contains( string(), 'Interest Earned: *')]//input"));
            IWebElement incomeField = driver.FindElement(By.XPath("//tr[contains( string(), 'Income: *')]//input"));
            IWebElement endDateField = driver.FindElement(By.XPath("//tr[contains( string(), 'End Date: *')]//input"));
            SelectElement startDayDropdown = new SelectElement(startDayField);
            SelectElement startMonthDropdown = new SelectElement(startMonthField);
            SelectElement startYearDropdown = new SelectElement(startYearField);

            //act
            depositAmountField.SendKeys("100000");
            rateOfInterestField.SendKeys("100");
            investmentTermField.SendKeys("365");
            startDayDropdown.SelectByText("1");
            startMonthDropdown.SelectByText("January");
            startYearDropdown.SelectByText("2022");
            financialYearButton365.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
           .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));
            calculateButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
           .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));

            //assert
            Assert.AreEqual("100,000.00", interestEarnedField.GetAttribute("value"));
            Assert.AreEqual("200,000.00", incomeField.GetAttribute("value"));
            Assert.AreEqual("01/01/2023", endDateField.GetAttribute("value"));
        }

        /* [Test]
          public void OrderOfDays()
          {
              List<string> expectedMonths = new List<string>();
              List<string> actualMonths = new List<string>();
              IWebElement startMonthField = driver.FindElement(By.Id("day"));
              SelectElement startMonthDropdown = new SelectElement(startMonthField);

              for (int i = 1; i < 30; i++)
              {
                  expectedMonths.Add(i.ToString());
              }

              foreach (IWebElement element in startMonthDropdown.Options)
              {
                  actualMonths.Add(element.Text);
              }
              Assert.AreEqual(expectedMonths, actualMonths);
          }

          string readOnly = startMonthField.GetAttribute("readOnly");

          Assert.AreEqual("true", readOnly);

         */

        [TestCase("aaaa", "aaaa", "aaaa")]
        [TestCase("!@£$%^&*", "!@£$%^&*", "!@£$%^&*")]
        [TestCase("", "", "")]
        [TestCase("0", "0", "0")]
        [TestCase("", "20", "30")]
        [TestCase("10", "", "30")]
        [TestCase("10", "20", "")]
        [TestCase("0", "20", "30")]
        [TestCase("10", "0", "30")]
        [TestCase("10", "20", "0")]
        [TestCase("100001", "20", "30")]
        [TestCase("100", "101", "30")]
        [TestCase("100", "99", "366")]
        public void NegativeInputTests(string depositAmount, string rateOfInterest, string investmentTerm)
        {
            //arrange
            IWebElement depositAmountField = driver.FindElement(By.XPath("//tr[contains( string(), 'Deposit Amount' )]//input"));
            IWebElement rateOfInterestField = driver.FindElement(By.Id("percent"));
            IWebElement investmentTermField = driver.FindElement(By.Id("term"));
            IWebElement financialYear = driver.FindElement(By.Id("finYear"));
            IWebElement financialYearButton365 = financialYear.FindElements(By.TagName("td"))[1];
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));

            //act
            depositAmountField.SendKeys(depositAmount);
            rateOfInterestField.SendKeys(rateOfInterest);
            investmentTermField.SendKeys(investmentTerm);
            financialYearButton365.Click();

            //assert
            Assert.IsFalse(calculateButton.Enabled);
        }

        [TestCase("1", "1", "1", "10", "March", "2020", "365", "0.00", "1.00", "11/03/2020")]
        [TestCase("100", "10", "15", "12", "July", "2022", "365", "0.41", "100.41", "27/07/2022")]
        [TestCase("50000", "50", "300", "25", "August", "2021", "365", "20,547.95", "70,547.95", "21/06/2022")]
        [TestCase("99999", "99", "360", "30", "September", "2012", "360", "98,999.01", "198,998.01", "25/09/2013")]
        [TestCase("100000", "100", "365", "20", "September", "2029", "360", "101,388.89", "201,388.89", "20/09/2030")]
        public void PositiveInputTests(string depositAmount, string rateOfInterest, string investmentTerm, string startDay, string startMonth, string startYear, string finYear, string interestEarned, string income, string endDate)
        {
            //arrange
            IWebElement depositAmountField = driver.FindElement(By.Id("amount"));
            IWebElement rateOfInterestField = driver.FindElement(By.Id("percent"));
            IWebElement investmentTermField = driver.FindElement(By.Id("term"));
            IWebElement startDayField = driver.FindElement(By.Id("day"));
            IWebElement startMonthField = driver.FindElement(By.Id("month"));
            IWebElement startYearField = driver.FindElement(By.Id("year"));
            IWebElement financialYear = driver.FindElement(By.Id("finYear"));
            IWebElement financialYearButton365 = financialYear.FindElement(By.XPath($"//tr[@id = 'finYear']/td[text() = '{finYear} days']/*")); ;
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));
            IWebElement interestEarnedField = driver.FindElement(By.Id("interest"));
            IWebElement incomeField = driver.FindElement(By.Id("income"));
            IWebElement endDateField = driver.FindElement(By.Id("endDate"));
            SelectElement startDayDropdown = new SelectElement(startDayField);
            SelectElement startMonthDropdown = new SelectElement(startMonthField);
            SelectElement startYearDropdown = new SelectElement(startYearField);

            //act
            depositAmountField.SendKeys(depositAmount);
            rateOfInterestField.SendKeys(rateOfInterest);
            investmentTermField.SendKeys(investmentTerm);
            startDayDropdown.SelectByText(startDay);
            startMonthDropdown.SelectByText(startMonth);
            startYearDropdown.SelectByText(startYear);
            financialYearButton365.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));
            calculateButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));

            //assert
            Assert.AreEqual(interestEarned, interestEarnedField.GetAttribute("value"));
            Assert.AreEqual(income, incomeField.GetAttribute("value"));
            Assert.AreEqual(endDate, endDateField.GetAttribute("value"));
        }
    }
}