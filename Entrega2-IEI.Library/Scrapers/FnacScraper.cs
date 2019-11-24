using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

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
                IWebElement categoryList = driver.FindElement(By.CssSelector(".CategoryNav"));
                IReadOnlyCollection<IWebElement> categories = categoryList.FindElements(By.CssSelector(".CategoryNav-item"));
                IWebElement phoneCategory = categories.FirstOrDefault(element => element.GetAttribute("content") == "Telefonía y Conectados");
                phoneCategory.Click();

                driver.Quit();
            }

            return phones;
        }
    }
}
