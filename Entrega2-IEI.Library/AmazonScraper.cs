using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Entrega2_IEI.Library
{
    static class AmazonScraper
    {
        private const string url = "https://www.amazon.es/";
        private static IWebDriver driver;
        public static List<Movil> mobiles = new List<Movil>();

        public static List<Movil> Setup(String brand,String product)
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            driver.Navigate().GoToUrl(url);

            IWebElement searchBox = driver.FindElement(By.Id("twotabsearchtextbox"));

            searchBox.SendKeys("smartphone "+brand + " "+ product);

            driver.FindElement(By.ClassName("nav-input")).Click();

            IList<IWebElement> listaElementos =
                driver.FindElements(By.XPath("//*[contains(@data-cel-widget, 'search_result')]"));

            Debug.WriteLine("Number of elements: " + listaElementos.Count);

            foreach (IWebElement element in listaElementos)
            {
                //Filter of Patrocinados and product's name
                if (!ScraperUtils.listFilter(element.Text) && element.FindElement(By.XPath(".//descendant::h2")).Text.Contains(product))
                {
                    Movil movil = new Movil(marca: brand, modelo: element.FindElement(By.XPath(".//descendant::h2")).Text,
                        precio: Double.Parse(element.FindElement(By.ClassName("a-price-whole")).Text));

                    Debug.WriteLine("--------------------------");
                    Debug.WriteLine(element.FindElement(By.XPath(".//descendant::h2")).Text);
                    Debug.WriteLine(element.FindElement(By.ClassName("a-price-whole")).Text+"€");
                    
                    if (ScraperUtils.isElementPresent(element, By.ClassName("a-text-price")))
                    {
                        Debug.WriteLine("discount: "+element.FindElement(By.ClassName("a-text-price")).Text);
                        String discountText = element.FindElement(By.ClassName("a-text-price")).Text;
                        movil.Descuento = Double.Parse(discountText.Remove(discountText.Length - 1));
                    }
                    Debug.WriteLine("--------------------------");
                    mobiles.Add(movil);

                }
            }

            driver.Quit();

            return mobiles;
        }
    }
}
