using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Globalization;
using System.Linq;

namespace Entrega2_IEI.Library
{
    internal static class ScraperUtils
    {
        public static string Currency => CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;

        /// <summary>
        /// Check if an element exists, if not throws an exception.
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsElementPresent(IWebElement webElement, By element)
        {
            try
            {
                webElement.FindElement(element);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private static readonly string[] filter = new[] { "Patrocinado", "Protector", "Silicona", "Funda", "Batería" };

        /// <summary>
        /// Check if it's a recommended product or isn't a smartphone
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsArticleValid(string s) => !filter.Any(x => s.ContainsIgnoreCase(x));

        public static ChromeDriver SetupChromeDriver(string startUrl)
        {
            ChromeDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            driver.Navigate().GoToUrl(startUrl);

            return driver;
        }

        public static bool ContainsIgnoreCase(this string source, string value)
            => CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, value, CompareOptions.IgnoreCase) >= 0;

        public static double ParseSpanishCulture(string source) => double.Parse(source, CultureInfo.CurrentCulture);
    }
}
