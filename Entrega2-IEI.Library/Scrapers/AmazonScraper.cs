using OpenQA.Selenium;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Ignacio Ferrer
    /// </summary>
    public class AmazonScraper : PhoneScraper
    {
        public const string AmazonUrl = "https://www.amazon.es/";

        public override string Url => AmazonUrl;

        private const string PriceClassName = "a-price-whole", PriceWithoutDiscountClassName = "a-text-price";

        public AmazonScraper(string brand, string model, bool showBrowser = true) : base(brand, model, showBrowser)
        {

        }

        protected override bool CheckPreconditions(IWebDriver driver)
            => base.CheckPreconditions(driver) && !driver.Title.ContainsIgnoreCase("captcha");

        /// <summary>
        /// Este método NO comprueba que la web no sea la del CAPTCHA. Si lo fuera, soltaría una <see cref="NoSuchElementException"/>.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="brand"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public override IEnumerable<Phone> SearchPhone(IWebDriver driver)
        {
            IWebElement searchBox = driver.FindElement(By.Id("twotabsearchtextbox"));

            searchBox.SendKeys("smartphone " + Brand + " " + Model);

            driver.FindElement(By.ClassName("nav-input")).Click();

            IList<IWebElement> elementList =
                driver.FindElements(By.XPath("//*[contains(@data-cel-widget, 'search_result')]"));

            Debug.WriteLine("Number of elements: " + elementList.Count);

            foreach (IWebElement element in elementList)
            {
                Phone phone = null;
                try
                {
                    string description = element.FindElement(By.XPath(".//descendant::h2")).Text;
                    // Filter of Patrocinados and product's name
                    if (ScraperUtils.IsArticleValid(element.Text) && description.ContainsIgnoreCase(Model))
                    {
                        IWebElement priceElement = element.FindElement(By.ClassName(PriceClassName));

                        string priceText = priceElement.Text;

                        priceText = priceText.Remove(priceText.Length - 1);

                        double price = ScraperUtils.ParseSpanishCulture(priceText);

                        phone = new Phone(Brand, Model, description, price);

                        Debug.WriteLine("--------------------------");
                        Debug.WriteLine(description);
                        Debug.WriteLine(Phone.PriceFormat(priceText));

                        try
                        {
                            IWebElement priceWithoutDiscountElement = element.FindElement(By.ClassName(PriceWithoutDiscountClassName));
                            
                            string priceWithoutDiscountText = priceWithoutDiscountElement.Text;
                            Debug.WriteLine("discount: " + priceWithoutDiscountText);

                            priceWithoutDiscountText = priceWithoutDiscountText.Remove(priceWithoutDiscountText.Length - 1);

                            double priceWithoutDiscount = ScraperUtils.ParseSpanishCulture(priceWithoutDiscountText);

                            phone.Discount = priceWithoutDiscount - price;
                        }
                        catch (NoSuchElementException ex)
                        {
                            Debug.WriteLine("No discount: " + ex.Message);
                        }

                        Debug.WriteLine("--------------------------");
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
    }
}
