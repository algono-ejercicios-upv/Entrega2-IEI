﻿using OpenQA.Selenium;
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

        public override string Url => PCComponentesUrl;

        private const string
            SearchInputXPathSelector = "/html/body/header/div[3]/div[1]/div/div[2]/div/form/input",
            ArticleItemXPathSelector = "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div",
            SearchSmartphoneXPathSelector = "/html/body/header/div[3]/div[2]/aside/div[3]/div[2]/div/ul/li[1]";






        public override IEnumerable<Phone> SearchPhone(IWebDriver driver, string brand, string model)
        {
            int articleCounter = 1;
            driver.Url = "https://www.pccomponentes.com/buscar/?query=" + " " + brand + " " + model;


            //Search(driver, $"Smartphone {brand} {model} -funda");

            IReadOnlyCollection<IWebElement> articleList = GetArticleItemList(driver);
            Debug.WriteLine(articleList.Count);

            System.Threading.Thread.Sleep(5000);
            foreach (IWebElement articleItem in articleList)
            {
                string ArticlePriceXPathSelector = "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div["+articleCounter+"]/article/div[1]/div[2]/div";
                string ArticleDescriptionXPathSelector = "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div["+articleCounter+"]/article/div[1]/header/h3/a";
                string ArticleOldPriceXPathSelector = "/html/body/div[1]/div[2]/div/div/div[2]/div[2]/div[3]/div[1]/div[" + articleCounter + "]/article/div[1]/div[2]/div[2]/div[1]";


                Phone phone = null;
                try 
                {
                    // Obtener el telefono (si existe)
                    // Si no encontrara algún elemento, saltaría al catch e iría al siguiente


                    IWebElement descriptionElement = articleItem.FindElement(By.XPath(ArticleDescriptionXPathSelector));
                    string description = descriptionElement.FindElement(By.XPath(ArticleDescriptionXPathSelector)).Text;
                    Debug.WriteLine(description);
                    articleCounter++;

                    if (ScraperUtils.IsArticleValid(description) && description.ContainsIgnoreCase(model)) {
                        IWebElement priceElement = articleItem.FindElement(By.XPath(ArticlePriceXPathSelector));
                        double price = ParsePrice(priceElement.Text, out string priceText, out string _);
                        Debug.WriteLine(price);
                        phone = new Phone(brand, model, description, price);

                        

                        try {

                            IWebElement oldPriceElement = articleItem.FindElement(By.XPath(ArticleOldPriceXPathSelector));
                            double oldPrice = ParsePrice(oldPriceElement.Text, out string oldPriceText, out string _);
                        
                            phone.Discount = oldPrice - price;
                        }
                        catch (NoSuchElementException e) {
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

        //private static IReadOnlyCollection<IWebElement> GetButtonItemList(IWebDriver driver) => driver.FindElements(By.XPath(SearchSmartphoneXPathSelector));

        private static void Search(IWebDriver driver, string v)
        {


            IWebElement searchBar = driver.FindElement(By.XPath(SearchInputXPathSelector));
            searchBar.SendKeys(v);

            /**
            IReadOnlyCollection<IWebElement> buttonList = GetButtonItemList(driver);
            int buttonListCounter = 1;

            string smartphoneType = "/html/body/header/div[3]/div[2]/aside/div[3]/div[2]/div/ul/li[" + buttonListCounter + "]";

            foreach (IWebElement buttonItem in buttonList) {
                try
                {
                    string smartphoneButtonFilter = "/html/body/header/div[3]/div[2]/aside/div[3]/div[2]/div/ul/li[" + buttonListCounter + "]/div/div";

                    string buttonName = buttonItem.FindElement(By.XPath(smartphoneButtonFilter)).Text;

                    if (buttonName == "Smartphone/Móviles")
                    {
                        smartphoneType = "/html/body/header/div[3]/div[2]/aside/div[3]/div[2]/div/ul/li[" + buttonListCounter + "]";
                        break;
                    }
                }
                catch (NoSuchElementException ex)
                {
                    Debug.WriteLine("Skipping article: " + ex.Message);
                }
            }

           // IWebElement smartphoneTypeButton = driver.FindElement(By.XPath(smartphoneType));
            //smartphoneTypeButton.Click();
           */

        }

        private static double ParsePrice(string text, out string priceText, out string priceCurrency)
        {
            priceText = text.Remove(text.Length - 1);
            priceCurrency = text.Substring(text.Length - 1, 1);
            double price = ScraperUtils.ParseSpanishCulture(priceText);
            return price;
        }
    }
}
