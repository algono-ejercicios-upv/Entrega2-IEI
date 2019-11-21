using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entrega2_IEI.Library
{
    static class ScraperUtils
    {
        //Check if an element exists, if not throws an exception.
        public static bool isElementPresent(IWebElement webElement, By element)
        {
            try
            {
                webElement.FindElement(element);
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        //Check if it's a recommended product or isn't a smartphone
        public static bool listFilter(string s)
        {
            if (new[] { "Patrocinado", "Más opciones de compra" , "Funda" , "Batería" }.Any(x => s.Contains(x)))
            {
                return true;
            }
            return false;
        }
    }
}
