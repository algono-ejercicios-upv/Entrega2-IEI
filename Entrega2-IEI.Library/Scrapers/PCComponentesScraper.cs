using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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
        public string PCComponentesUrl = "https://www.pccomponentes.com/buscar/?query=";

        public override string Url => $"{PCComponentesUrl} {Brand} {Model}";

        private const string ArticleItemXPathSelector = "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div";

        public PCComponentesScraper(string brand, string model, bool showBrowser = true) : base(brand, model, showBrowser)
        {

        }

        public static string GetArticlePriceXPathSelector(int articleCounter)
            => "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div[" + articleCounter + "]/article/div[1]/div[2]/div";
        public static string GetArticleDescriptionXPathSelector(int articleCounter)
            => "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div[" + articleCounter + "]/article/div[1]/header/h3/a";

        public static string GetArticleOldPriceXPathSelector(int articleCounter)
            => "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div[" + articleCounter + "]/article/div[1]/div[2]/div[2]/div[1]";


        public override IEnumerable<Phone> SearchPhone(IWebDriver driver)
        {
            // Esperar más tiempo para protegerse de la web proteccion DDOS
            var waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            int articleCounter = 1;

            IReadOnlyCollection<IWebElement> articleList = waiter.Until(GetArticleItemList);
            Debug.WriteLine(articleList.Count);

            System.Threading.Thread.Sleep(5000);
            foreach (IWebElement articleItem in articleList)
            {
                string ArticlePriceXPathSelector = GetArticlePriceXPathSelector(articleCounter);
                string ArticleDescriptionXPathSelector = GetArticleDescriptionXPathSelector(articleCounter);
                string ArticleOldPriceXPathSelector = GetArticleOldPriceXPathSelector(articleCounter);

                Phone phone = null;
                try
                {
                    // Obtener el telefono (si existe)
                    // Si no encontrara algún elemento, saltaría al catch e iría al siguiente

                    IWebElement descriptionElement = articleItem.FindElement(By.XPath(ArticleDescriptionXPathSelector));
                    string description = descriptionElement.FindElement(By.XPath(ArticleDescriptionXPathSelector)).Text;
                    Debug.WriteLine(description);
                    articleCounter++;

                    if (ScraperUtils.IsArticleValid(description) && description.ContainsIgnoreCase(Model))
                    {
                        IWebElement priceElement = articleItem.FindElement(By.XPath(ArticlePriceXPathSelector));
                        double price = ParsePrice(priceElement.Text, out string priceText, out string _);
                        Debug.WriteLine(price);
                        phone = new Phone(Brand, Model, description, price);

                        try
                        {
                            IWebElement oldPriceElement = articleItem.FindElement(By.XPath(ArticleOldPriceXPathSelector));
                            double oldPrice = ParsePrice(oldPriceElement.Text, out string oldPriceText, out string _);

                            phone.Discount = oldPrice - price;
                        }
                        catch (NoSuchElementException e)
                        {
                            // No hagas nada, simplemente no hay descuento
                            Debug.WriteLine("No discount: " + e.Message);
                        }
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

        private static IReadOnlyCollection<IWebElement> GetArticleItemList(IWebDriver driver) => driver.FindElements(By.XPath(ArticleItemXPathSelector));

        private static double ParsePrice(string text, out string priceText, out string priceCurrency)
        {
            priceText = text.Remove(text.Length - 1);
            priceCurrency = text.Substring(text.Length - 1, 1);
            double price = ScraperUtils.ParseSpanishCulture(priceText);
            return price;
        }
    }
}
