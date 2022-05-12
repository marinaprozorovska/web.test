using System;
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
            driver.Url = "https://localhost:5001/Calculator";
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
            IWebElement depositAmountFields = driver.FindElements(By.TagName("tr"))[0];
            IWebElement depositAmountField = depositAmountFields.FindElements(By.TagName("td"))[0];

            //assert
            Assert.AreEqual("Deposit Amount: *", depositAmountField.Text);

        }

        [Test]
        public void InterestEarnedOutputBugTest()
        {
            //arrange
            IWebElement interestEarnedOutputs = driver.FindElements(By.TagName("tr"))[6];
            IWebElement interestEarnedOutput = interestEarnedOutputs.FindElements(By.TagName("th"))[0];

            //assert
            Assert.AreEqual("Interest Earned: *	", interestEarnedOutput.Text);

        }

        [Test]
        public void IncomeOutputBugTest()
        {
            //arrange
            IWebElement incomeOutputs = driver.FindElements(By.TagName("tr"))[7];
            IWebElement incomeOutput = incomeOutputs.FindElements(By.TagName("th"))[0];

            //assert
            Assert.AreEqual("Income: *", incomeOutput.Text);
        }


        [Test]
        public void OrderOfAprilMonthBugTest()
        {
            //arrange
            IWebElement monthDropdown = driver.FindElement(By.Id("month"));
            IWebElement months = driver.FindElement(By.Id("month"));

            //act
            monthDropdown.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("month")));
            IWebElement aprilMonth = months.FindElements(By.TagName("option"))[3];

            //assert
            Assert.AreEqual("April", aprilMonth.Text);
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
            IWebElement depositAmountField = driver.FindElement(By.Id("amount"));
            IWebElement rateOfInterestField = driver.FindElement(By.Id("percent"));
            IWebElement investmentTermField = driver.FindElement(By.Id("term"));
            IWebElement startDayField = driver.FindElement(By.Id("day"));
            IWebElement startMonthField = driver.FindElement(By.Id("month"));
            IWebElement startYearField = driver.FindElement(By.Id("year"));
            IWebElement financialYear = driver.FindElement(By.Id("finYear"));
            IWebElement financialYearButton365 = financialYear.FindElements(By.TagName("td"))[1];
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));
            IWebElement interestEarnedField = driver.FindElement(By.Id("interest"));
            IWebElement incomeField = driver.FindElement(By.Id("income"));
            IWebElement endDateField = driver.FindElement(By.Id("endDate"));

            //act
            depositAmountField.SendKeys("100000");
            rateOfInterestField.SendKeys("100");
            investmentTermField.SendKeys("365");
            startDayField.SendKeys("1");
            startMonthField.SendKeys("January");
            startYearField.SendKeys("2022");
            financialYearButton365.Click();
            calculateButton.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("calculateBtn")));

            //assert
            Assert.AreEqual("100,000.00", interestEarnedField.GetAttribute("value"));
            Assert.AreEqual("200,000.00", incomeField.GetAttribute("value"));
            Assert.AreEqual("01/01/2023", endDateField.GetAttribute("value"));
        }


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
        [TestCase("100005", "20", "30")]
        [TestCase("100", "101", "30")]
        [TestCase("100", "99", "367")]
        public void NegativeInputTests(string depositAmount, string rateOfInterest, string investmentTerm)
        {
            //arrange
            IWebElement depositAmountField = driver.FindElement(By.Id("amount"));
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

        [TestCase("1", "1", "1", "10", "March", "2020", "0.00", "1.00", "11/03/2020")]
        [TestCase("100", "10", "15", "12", "July", "2022", "0.41", "100.41", "27/07/2022")]
        [TestCase("50000", "50", "300", "25", "August", "2021", "20,547.95", "70,547.95", "21/06/2022")]
        [TestCase("99999", "99", "360", "30", "September", "2012", "97,642.86", "197,641.86", "25/09/2013")]
        [TestCase("100000", "100", "365", "20", "September", "2029", "100,000.00", "200,000.00", "20/09/2030")]
        public void PositiveInput365daysTests(string depositAmount, string rateOfInterest, string investmentTerm, string startDay, string startMonth, string startYear, string interestEarned, string income, string endDate)
        {
            //arrange
            IWebElement depositAmountField = driver.FindElement(By.Id("amount"));
            IWebElement rateOfInterestField = driver.FindElement(By.Id("percent"));
            IWebElement investmentTermField = driver.FindElement(By.Id("term"));
            IWebElement startDayField = driver.FindElement(By.Id("day"));
            IWebElement startMonthField = driver.FindElement(By.Id("month"));
            IWebElement startYearField = driver.FindElement(By.Id("year"));
            IWebElement financialYear = driver.FindElement(By.Id("finYear"));
            IWebElement financialYearButton365 = financialYear.FindElements(By.TagName("input"))[0];
            IWebElement calculateButton = driver.FindElement(By.Id("calculateBtn"));
            IWebElement interestEarnedField = driver.FindElement(By.Id("interest"));
            IWebElement incomeField = driver.FindElement(By.Id("income"));
            IWebElement endDateField = driver.FindElement(By.Id("endDate"));

            //act
            depositAmountField.SendKeys(depositAmount);
            rateOfInterestField.SendKeys(rateOfInterest);
            investmentTermField.SendKeys(investmentTerm);
            startDayField.SendKeys(startDay);
            startMonthField.SendKeys(startMonth);
            startYearField.SendKeys(startYear);
            financialYearButton365.Click();
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