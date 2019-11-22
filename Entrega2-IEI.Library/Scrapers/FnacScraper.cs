using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Alejandro Gómez
    /// </summary>
    internal static class FnacScraper
    {
        public const string Url = "https://www.fnac.es/";

        public static List<Movil> SearchPhone(string brand, string product)
        {
            List<Movil> phones = new List<Movil>();
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                // TODO: Crear el web scraper para fnac

                driver.Quit();
            }

            return phones;
        }
    }
}
