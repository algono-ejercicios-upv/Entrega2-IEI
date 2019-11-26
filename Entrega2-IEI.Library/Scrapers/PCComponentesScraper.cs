using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Alejandro Tauste
    /// </summary>
    public class PCComponentesScraper : IPhoneScraper
    {
        public const string Url = "https://www.pccomponentes.com/";

        public void GoToUrl(IWebDriver driver) => driver.Navigate().GoToUrl(Url);

        public IEnumerable<Phone> SearchPhone(string brand, string model)
        {
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                return SearchPhone(driver, brand, model);
            }
        }

        public IEnumerable<Phone> SearchPhone(IWebDriver driver, string brand, string model)
        {
            // Descomentar las líneas de código cuando vayas a escribir el código (y quites la NotImplementedException)

            // TODO: Crear el web scraper para PCComponentes
            throw new NotImplementedException();

            //driver.Quit();
        }
    }
}
