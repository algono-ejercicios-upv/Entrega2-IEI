using OpenQA.Selenium;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Alejandro Gómez
    /// </summary>
    public class FnacScraper : PhoneScraper
    {
        public const string FnacUrl = "https://www.fnac.es/";

        public override string Url => FnacUrl;

        private const string 
            SearchInputCssSelector = ".Header__search-input", SearchSubmitCssSelector = ".Header__search-submit",
            ArticleItemCssSelector = ".Article-item", ArticleDescriptionCssSelector = ".Article-desc",
            ArticleOldPriceCssSelector = ".oldPrice", ArticlePriceCssSelector = ".userPrice";

        public FnacScraper(string brand, string model, bool showBrowser = true) : base(brand, model, showBrowser)
        {

        }

        public override IEnumerable<Phone> SearchPhone(IWebDriver driver)
        {
            Search(driver, $"Smartphone {Brand} {Model}");

            // TODO: Extraer datos de los resultados de busqueda
            IReadOnlyCollection<IWebElement> articleItemList = GetArticleItemList(driver);

            foreach (IWebElement articleItem in articleItemList)
            {
                Phone phone = null;
                try
                {
                    IWebElement descriptionElement = articleItem.FindElement(By.CssSelector(ArticleDescriptionCssSelector));
                    string description = descriptionElement.FindElement(By.XPath(".//descendant::a")).Text;

                    if (ScraperUtils.IsArticleValid(description) && description.ContainsIgnoreCase(Model))
                    {
                        IWebElement priceElement = articleItem.FindElement(By.CssSelector(ArticlePriceCssSelector));
                        double price = ParsePrice(priceElement.Text, out string priceText, out string _);

                        phone = new Phone(Brand, Model, description, price);

                        try
                        {
                            IWebElement oldPriceElement = articleItem.FindElement(By.CssSelector(ArticleOldPriceCssSelector));
                            double oldPrice = ParsePrice(oldPriceElement.Text, out string oldPriceText, out string _);

                            phone.Discount = oldPrice - price;
                        }
                        catch (NoSuchElementException ex)
                        {
                            // No hagas nada, simplemente no hay descuento
                            Debug.WriteLine("No discount: " + ex.Message);
                        }

                        // Como en FNAC la descripción también contiene la marca y el Modelo, mostramos sólo la descripción
                        phone.Description = description;
                        phone.NameFormat = PhoneNameFormat.Description;
                    }
                }
                catch (NoSuchElementException ex)
                {
                    Debug.WriteLine("Skipping article: " + ex.Message);
                }

                if (phone != null) yield return phone;
            }

            driver.Quit();
        }

        private static double ParsePrice(string priceFullText, out string priceText, out string priceCurrency)
        {
            priceText = priceFullText.Remove(priceFullText.Length - 1);
            priceCurrency = priceFullText.Substring(priceFullText.Length - 1, 1);
            double price = ScraperUtils.ParseSpanishCulture(priceText);
            return price;
        }

        private static void Search(IWebDriver driver, string text)
        {
            IWebElement searchBar = driver.FindElement(By.CssSelector(SearchInputCssSelector));
            searchBar.SendKeys(text);

            IWebElement searchButton = driver.FindElement(By.CssSelector(SearchSubmitCssSelector));
            searchButton.Click();
        }

        private static IReadOnlyCollection<IWebElement> GetArticleItemList(IWebDriver driver) => driver.FindElements(By.CssSelector(ArticleItemCssSelector));
    }
}
