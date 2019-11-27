using OpenQA.Selenium;
using System.Collections.Generic;

namespace Entrega2_IEI.Library.Scrapers
{
    public abstract class PhoneScraper
    {
        public abstract string Url { get; }

        public void GoToUrl(IWebDriver driver) => driver.Navigate().GoToUrl(Url);

        public virtual IEnumerable<Phone> SearchPhone(string brand, string model)
        {
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                foreach (Phone phone in SearchPhone(driver, brand, model))
                {
                    yield return phone;
                }
            }
        }

        /// <summary>
        /// Esta versión NO te lleva a la url de la web, lo tienes que hacer mediante <see cref="GoToUrl(IWebDriver)"/> antes, o no funcionará.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="brand"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract IEnumerable<Phone> SearchPhone(IWebDriver driver, string brand, string model);
    }
}
