using Entrega2_IEI.Library.Scrapers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Entrega2_IEI.Library
{
    public class Phone
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        internal PhoneNameFormat NameFormat { get; set; }
        public string Name => $"{GetFormattedName()} - {PriceFormat(Price)} - {PriceFormat(Discount)}";

        public double Price { get; set; }
        public double Discount { get; set; }

        public string Description { get; set; }

        internal Phone(string brand, string model, double price = default, double discount = default, string description = default)
        {
            Brand = brand;
            Model = model;

            Price = price;
            Discount = discount;

            Description = description;
            NameFormat = description == null ? PhoneNameFormat.BrandModel : PhoneNameFormat.Description;
        }

        private string GetFormattedName()
        {
            switch (NameFormat)
            {
                case PhoneNameFormat.Description: return Description;
                default: return $"{Brand} {Model}";
            }
        }

        public override string ToString() => Name;

        private static string PriceFormat(double price) => string.Format("{0:C2}", price);
        
        public static void SearchPhoneInMultipleScrapers(string brand, string model, Action<IPhoneScraper, IList<Phone>> handler, ICollection<IPhoneScraper> scrapers)
        {
            if (handler != null && scrapers.Count > 0)
            {
                using (IWebDriver driver = ScraperUtils.SetupChromeDriver())
                {
                    foreach (IPhoneScraper scraper in scrapers)
                    {
                        scraper.GoToUrl(driver);
                        IList<Phone> phones = scraper.SearchPhone(driver, brand, model);
                        handler.Invoke(scraper, phones);
                    }
                }
            }
        }
    }
}
