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
        private const string CategoryCssSelector = ".CategoryNav-link", SubCategoryCssSelector = ".categoryMenu-link",
            SearchInputCssSelector = ".Header__search-input", SearchSubmitCssSelector = ".Header__search-submit";

        public IList<Phone> SearchPhone(string brand, string product)
        {
            List<Phone> phones = new List<Phone>();
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                
                Search(driver, $"Smartphone {brand} {product}");

                // TODO: Extraer datos de los resultados de busqueda

                driver.Quit();
            }

            return phones;
        }

        private static void Search(IWebDriver driver, string text)
        {
            IWebElement searchBar = driver.FindElement(By.CssSelector(SearchInputCssSelector));
            searchBar.SendKeys(text);

            IWebElement searchButton = driver.FindElement(By.CssSelector(SearchSubmitCssSelector));
            searchButton.Click();
        }

        private static void GoToCategory(IWebDriver driver, string text) => ClickOnItemFromList(driver, CategoryCssSelector, text);

        private static void GoToSubCategory(IWebDriver driver, string text) => ClickOnItemFromList(driver, SubCategoryCssSelector, text);

        private static void ClickOnItemFromList(IWebDriver driver, string listCssSelector, string text)
        {
            IReadOnlyCollection<IWebElement> list = driver.FindElements(By.CssSelector(listCssSelector));
            IWebElement item = list.FirstOrDefault(element => element.Text == text);
            item.Click();
        }
    }
}
