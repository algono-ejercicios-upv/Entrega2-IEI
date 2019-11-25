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
                string description = element.FindElement(By.XPath(".//descendant::h2")).Text;
                // Filter of Patrocinados and product's name
                if (ScraperUtils.IsArticleValid(element.Text) && description.ContainsIgnoreCase(model))
                {
                    string priceText = element.FindElement(By.ClassName("a-price-whole")).Text;
                    double price = ScraperUtils.ParseSpanishCulture(priceText);

                    Phone phone = new Phone(brand, model, description, price);

                    Debug.WriteLine("--------------------------");
                    Debug.WriteLine(element.FindElement(By.XPath(".//descendant::h2")).Text);
                    Debug.WriteLine(Phone.PriceFormat(element.FindElement(By.ClassName("a-price-whole")).Text));

                    if (ScraperUtils.IsElementPresent(element, By.ClassName("a-text-price")))
                    {
                        Debug.WriteLine("discount: " + element.FindElement(By.ClassName("a-text-price")).Text);
                        string discountText = element.FindElement(By.ClassName("a-text-price")).Text;
                        phone.Discount = double.Parse(discountText.Remove(discountText.Length - 1));
                    }
                    Debug.WriteLine("--------------------------");
                    phones.Add(phone);

                }
            }

            driver.Quit();

            return phones;
        }
    }
}
