using OpenQA.Selenium;
using System.Collections.Generic;

namespace Entrega2_IEI.Library.Scrapers
{
    public interface IPhoneScraper
    {
        IList<Phone> SearchPhone(string brand, string model);
        IList<Phone> SearchPhone(IWebDriver driver, string brand, string model);
    }
}
