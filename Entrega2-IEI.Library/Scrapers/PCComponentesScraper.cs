using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Alejandro Tauste
    /// </summary>
    public class PCComponentesScraper : PhoneScraper
    {
        public const string PCComponentesUrl = "https://www.pccomponentes.com/";

        public override string Url => PCComponentesUrl;

        private const string
            SearchInputCssSelector = "/html/body/header/div[3]/div[1]/div/div[2]/div/form/input", SearchSubmitCssSelector = "/html/body/header/div[3]/div[1]/div/div[2]/div/form/input",
            ArticleItemCssSelector = "tarjeta-articulo expandible", ArticleDescriptionCssSelector = "tarjeta-articulo__nombre",
            ArticleOldPriceCssSelector = "tarjeta-articulo__pvp", ArticlePriceCssSelector = "tarjeta-articulo__precio-actual";

        public override IEnumerable<Phone> SearchPhone(IWebDriver driver, string brand, string model)
        {
            List<Phone> phones = new List<Phone>();

            Search(driver, $"Smartphone {brand} {model}");

            IReadOnlyCollection<IWebElement> articleList = GetArticleItemList(driver);

            foreach (IWebElement artitleItem in articleList)
            {
                Phone phone = null;
                try 
                {
                    // Obtener el telefono (si existe)
                    // Si no encontrara algún elemento, saltaría al catch e iría al siguiente
                }
                catch (NoSuchElementException ex)
                {
                    Debug.WriteLine("Skipping article: " + ex.Message);
                }

                if (phone != null) yield return phone;
            }

            driver.Quit();
        }

        private static IReadOnlyCollection<IWebElement> GetArticleItemList(IWebDriver driver) => driver.FindElements(By.CssSelector(ArticleItemCssSelector));

        private static void Search(IWebDriver driver, string v)
        {
            IWebElement searchBar = driver.FindElement(By.XPath(SearchInputCssSelector));
            searchBar.SendKeys(v);
        }
    }
}
