using OpenQA.Selenium;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Ignacio Ferrer
    /// </summary>
    public class AmazonScraper : IPhoneScraper
    {
        public const string Url = "https://www.amazon.es/";

        private const string PriceClassName = "a-text-price", PriceWithoutDiscountClassName = "a-price-whole";

        public void GoToUrl(IWebDriver driver) => driver.Navigate().GoToUrl(Url);

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
            List<Phone> phones = new List<Phone>();

            IWebElement searchBox = driver.FindElement(By.Id("twotabsearchtextbox"));

            searchBox.SendKeys("smartphone " + brand + " " + model);

            driver.FindElement(By.ClassName("nav-input")).Click();

            IList<IWebElement> elementList =
                driver.FindElements(By.XPath("//*[contains(@data-cel-widget, 'search_result')]"));

            Debug.WriteLine("Number of elements: " + elementList.Count);

            foreach (IWebElement element in elementList)
            {
                try
                {
                    string description = element.FindElement(By.XPath(".//descendant::h2")).Text;
                    // Filter of Patrocinados and product's name
                    if (ScraperUtils.IsArticleValid(element.Text) && description.ContainsIgnoreCase(model))
                    {
                        IWebElement priceElement = element.FindElement(By.ClassName(PriceClassName));
                        string priceText = priceElement.Text;
                        double price = ScraperUtils.ParseSpanishCulture(priceText);

                        Phone phone = new Phone(brand, model, description, price);

                        Debug.WriteLine("--------------------------");
                        Debug.WriteLine(description);
                        Debug.WriteLine(Phone.PriceFormat(priceText));

                        try
                        {
                            IWebElement priceWithoutDiscountElement = element.FindElement(By.ClassName(PriceWithoutDiscountClassName));
                            
                            string priceWithoutDiscountText = priceWithoutDiscountElement.Text;
                            Debug.WriteLine("discount: " + priceWithoutDiscountText);

                            double priceWithoutDiscount = ScraperUtils.ParseSpanishCulture(priceWithoutDiscountText.Remove(priceWithoutDiscountText.Length - 1));

                            phone.Discount = priceWithoutDiscount - price;
                        }
                        catch (NoSuchElementException ex)
                        {
                            Debug.WriteLine("No discount: " + ex.Message);
                        }

                        Debug.WriteLine("--------------------------");
                        phones.Add(phone);
                    }
                }
                catch (NoSuchElementException ex)
                {
                    Debug.WriteLine("Skipping article: " + ex.Message);
                }
            }

            driver.Quit();

            return phones;
        }
    }
}
