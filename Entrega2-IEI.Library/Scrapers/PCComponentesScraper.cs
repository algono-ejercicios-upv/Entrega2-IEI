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

        public IList<Phone> SearchPhone(string brand, string model)
        {
            IList<Phone> phones;
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                phones = SearchPhone(driver, brand, model);
            }

            return phones;
        }

        public IList<Phone> SearchPhone(IWebDriver driver, string brand, string model)
        {
            // Descomentar las líneas de código cuando vayas a escribir el código (y quites la NotImplementedException)

            //List<Phone> phones = new List<Phone>();

            // TODO: Crear el web scraper para PCComponentes
            throw new NotImplementedException();

            //driver.Quit();

            //return phones;
        }
    }
}
