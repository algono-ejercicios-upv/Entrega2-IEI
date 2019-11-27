using OpenQA.Selenium;
using System.Collections.Generic;

namespace Entrega2_IEI.Library.Scrapers
{
    public abstract class PhoneScraper
    {
        public abstract string Url { get; }

        public string Brand { get; }

        public string Model { get; }

        public bool ShowBrowser { get; set; }

        protected PhoneScraper(string brand, string model, bool showBrowser = true)
        {
            Brand = brand;
            Model = model;
            ShowBrowser = showBrowser;
        }

        public void GoToUrl(IWebDriver driver) => driver.Navigate().GoToUrl(Url);

        public IEnumerable<Phone> SearchPhone()
        {
            bool done = false;
            while (!done)
            {
                using (IWebDriver driver = ScraperUtils.SetupChromeDriver(ShowBrowser, Url))
                {
                    if (CheckPreconditions(driver))
                    {
                        foreach (Phone phone in SearchPhone(driver))
                        {
                            yield return phone;
                        }

                        done = true;
                    }
                }
            }
        }

        protected virtual bool CheckPreconditions(IWebDriver driver) => true;

        /// <summary>
        /// Esta versión NO te lleva a la url de la web, lo tienes que hacer mediante <see cref="GoToUrl(IWebDriver)"/> antes, o no funcionará.
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public abstract IEnumerable<Phone> SearchPhone(IWebDriver driver);
    }
}
