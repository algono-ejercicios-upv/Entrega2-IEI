using OpenQA.Selenium.Chrome;
using System;
using System.Globalization;
using System.Linq;

namespace Entrega2_IEI.Library
{
    public class ScraperConfig
    {
        public bool ShowBrowser { get; set; }

        public string Currency => CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;

        private readonly string[] filter = new[] { "Patrocinado", "Protector", "Silicona", "Funda", "Batería", "Carcasa" };

        /// <summary>
        /// Check if it's a recommended product or isn't a smartphone
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool IsArticleValid(string s) => !filter.Any(x => s.ContainsIgnoreCase(x));

        public ChromeDriver SetupChromeDriver(string startUrl = default)
        {
            ChromeOptions options = new ChromeOptions();
            if (!ShowBrowser) options.AddArgument("headless");

            ChromeDriver driver = new ChromeDriver(options);

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            if (startUrl != default) driver.Navigate().GoToUrl(startUrl);

            return driver;
        }

        public double ParseSpanishCulture(string source) => double.Parse(source, CultureInfo.CurrentCulture);
    }
}
