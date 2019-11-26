using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Alejandro Tauste
    /// </summary>
    public class PCComponentesScraper : PhoneScraper
    {
        public const string PCComponentesUrl = "https://www.pccomponentes.com/";

        public override string Url => PCComponentesUrl;

        public override IEnumerable<Phone> SearchPhone(IWebDriver driver, string brand, string model)
        {
            // Descomentar las líneas de código cuando vayas a escribir el código (y quites la NotImplementedException)

            // TODO: Crear el web scraper para PCComponentes
            throw new NotImplementedException();

            //driver.Quit();
        }
    }
}
