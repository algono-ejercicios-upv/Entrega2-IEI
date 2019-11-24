using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Alejandro Tauste
    /// </summary>
    public class PCComponentesScraper : IPhoneScraper
    {
        public const string Url = "https://www.pccomponentes.com/";

        public IList<Phone> SearchPhone(string brand, string product)
        {
            List<Phone> phones = new List<Phone>();
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                // TODO: Crear el web scraper para PCComponentes

                driver.Quit();
            }

            return phones;
        }
    }
}
