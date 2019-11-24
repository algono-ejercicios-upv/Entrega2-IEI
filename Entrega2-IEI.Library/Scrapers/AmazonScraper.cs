using OpenQA.Selenium;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entrega2_IEI.Library.Scrapers
{
    /// <summary>
    /// Author: Ignacio Ferrer
    /// </summary>
    internal static class AmazonScraper
    {
        public const string Url = "https://www.amazon.es/";

        public static List<Movil> SearchPhone(string brand,string product)
        {
            List<Movil> mobiles = new List<Movil>();
            using (IWebDriver driver = ScraperUtils.SetupChromeDriver(Url))
            {
                IWebElement searchBox = driver.FindElement(By.Id("twotabsearchtextbox"));

                searchBox.SendKeys("smartphone " + brand + " " + product);

                driver.FindElement(By.ClassName("nav-input")).Click();

                IList<IWebElement> listaElementos =
                    driver.FindElements(By.XPath("//*[contains(@data-cel-widget, 'search_result')]"));

                Debug.WriteLine("Number of elements: " + listaElementos.Count);

                foreach (IWebElement element in listaElementos)
                {
                    string modelo = element.FindElement(By.XPath(".//descendant::h2")).Text;
                    //Filter of Patrocinados and product's name
                    if (!ScraperUtils.ListFilter(element.Text) && modelo.ContainsIgnoreCase(product))
                    {
                        string precioTexto = element.FindElement(By.ClassName("a-price-whole")).Text;
                        double precio = ScraperUtils.ParseSpanishCulture(precioTexto);

                        Movil movil = new Movil(marca: brand, modelo: modelo,
                            precio: precio);

                        Debug.WriteLine("--------------------------");
                        Debug.WriteLine(element.FindElement(By.XPath(".//descendant::h2")).Text);
                        Debug.WriteLine(element.FindElement(By.ClassName("a-price-whole")).Text + "€");

                        if (ScraperUtils.IsElementPresent(element, By.ClassName("a-text-price")))
                        {
                            Debug.WriteLine("discount: " + element.FindElement(By.ClassName("a-text-price")).Text);
                            string discountText = element.FindElement(By.ClassName("a-text-price")).Text;
                            movil.Descuento = double.Parse(discountText.Remove(discountText.Length - 1));
                        }
                        Debug.WriteLine("--------------------------");
                        mobiles.Add(movil);

                    }
                }

                driver.Quit();
            }

            return mobiles;
        }
    }
}
