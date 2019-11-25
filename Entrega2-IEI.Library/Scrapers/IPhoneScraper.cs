using OpenQA.Selenium;
using System.Collections.Generic;

namespace Entrega2_IEI.Library.Scrapers
{
    public interface IPhoneScraper
    {
        void GoToUrl(IWebDriver driver);

        IList<Phone> SearchPhone(string brand, string model);

        /// <summary>
        /// Esta versión NO te lleva a la url de la web, lo tienes que hacer mediante <see cref="GoToUrl(IWebDriver)"/> antes, o no funcionará.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="brand"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<Phone> SearchPhone(IWebDriver driver, string brand, string model);
    }
}
