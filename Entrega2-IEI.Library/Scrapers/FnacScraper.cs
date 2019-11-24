using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Alejandro Gómez
    /// </summary>
    public class FnacScraper : IPhoneScraper
    {
        public const string Url = "https://www.fnac.es/";

        public IList<Phone> SearchPhone(string brand, string product)
        {
            List<Phone> phones = new List<Phone>();
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                // TODO: Crear el web scraper para fnac
                IWebElement phoneCategory = driver.FindElement(By.ClassName("CategoryNav js-CategoryNav")).FindElement(By.Name("Telefonía y Conectados"));
                phoneCategory.Click();

                driver.Quit();
            }

            return phones;
        }
    }
}
