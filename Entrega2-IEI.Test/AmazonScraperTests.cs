using Entrega2_IEI.Library;
using Entrega2_IEI.Library.Scrapers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq.Expressions;

namespace Entrega2_IEI.Test
{
    [TestClass]
    public class AmazonScraperTests
    {
        /// <summary>
        /// En <see cref="AmazonScraper"/>, si aparece la pagina del Captcha, debe crear un nuevo <see cref="IWebDriver"/> hasta que deje de aparecer 
        /// </summary>
        [TestMethod]
        public void AmazonScraperAvoidsCaptchaPage()
        {
            string marca = "Xiaomi";
            string modelo = "Mi A3";

            #region Arrange
            // Inicializamos los mocks
            var scraperConfig = new Mock<ScraperConfig>();
            var driver = new Mock<IWebDriver>();

            // Hacemos un numero aleatorio entre 1 y 5 para probar varios casos
            int vecesQueVaASalirElCaptcha = new Random().Next(1, 5);

            int vecesQueHaSalidoElCaptcha = 0;

            // Cuando intente hacer algo con el driver, debe terminar
            driver.Setup(driver => driver.FindElement(It.IsAny<By>())).Throws<ScrapingCorrectoException>();

            Expression<Func<IWebDriver, string>> tituloDriverExpression = driver => driver.Title;
            // El titulo devolverá el captcha mientras que el contador no llegue a su máximo. Por cada llamada, aumenta el contador en 1
            driver.When(() => vecesQueHaSalidoElCaptcha < vecesQueVaASalirElCaptcha)
                .SetupGet(tituloDriverExpression)
                .Returns("Amazon CAPTCHA")
                .Callback(() => vecesQueHaSalidoElCaptcha++);

            // Cuando ya hemos mostrado el captcha las veces indicadas, devuelve un titulo válido
            driver.When(() => vecesQueHaSalidoElCaptcha >= vecesQueVaASalirElCaptcha)
                .SetupGet(tituloDriverExpression)
                .Returns("Amazon");

            // Guardamos la expresión asociada a crear un nuevo driver
            Expression<Func<ScraperConfig, IWebDriver>> setupChromeDriverExpression = scraperConfig => scraperConfig.SetupChromeDriver(It.IsAny<string>());

            // Si va a crear un nuevo driver, devuelve el mock
            scraperConfig.Setup(setupChromeDriverExpression).Returns(driver.Object).Verifiable();

            // Crea el scraper real con los mocks
            AmazonScraper scraper = new AmazonScraper(marca, modelo)
            {
                Config = scraperConfig.Object
            };
            #endregion

            #region Act
            try
            {
                foreach (Phone phone in scraper.SearchPhone())
                {
                    Console.WriteLine(phone);
                }
            }
            catch (ScrapingCorrectoException)
            {
                // Lanzar esta excepción indica que consideramos el scraping como correcto
            }
            #endregion

            #region Assert
            // Comprobamos que se ha llamado a crear un nuevo driver exactamente el número de veces que sale el captcha
            // (y una más que sería la correcta).
            scraperConfig.Verify(setupChromeDriverExpression, Times.Exactly(vecesQueVaASalirElCaptcha + 1), failMessage: "El driver no se ha recargado las veces que tocaba");
            #endregion
        }
    }
}
