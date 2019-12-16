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
        private struct ContadorCaptcha
        {
            public int maximo;
            public int actual;
            public bool Seguir => actual < maximo;
            public void Incrementar() => actual++;
        }

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

            // Necesitamos un contador como objeto para evitar que se pierda el contador entre las lambdas
            ContadorCaptcha contadorCaptcha = new ContadorCaptcha
            {
                // Su maximo es el numero de veces que debe salir el captcha
                // Hacemos un numero aleatorio entre 1 y 5 para probar varios casos
                maximo = new Random().Next(1, 5),

                // El valor actual es el contador real en el test
                actual = 0
            };

            // Cuando intente hacer algo con el driver, debe terminar
            driver.Setup(driver => driver.FindElement(It.IsAny<By>())).Throws<ScrapingCorrectoException>();

            // El titulo devolver� el captcha mientras que el contador no llegue a su m�ximo. Por cada llamada, aumenta el contador en 1
            driver.Setup(driver => driver.Title).Returns(delegate { return contadorCaptcha.Seguir ? "Amazon CAPTCHA" : "Amazon"; }).Callback(() => contadorCaptcha.Incrementar());

            // Guardamos la expresi�n asociada a crear un nuevo driver
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
                // Lanzar esta excepci�n indica que consideramos el scraping como correcto
            }
            #endregion

            #region Assert
            // Comprobamos que se ha llamado a crear un nuevo driver exactamente el n�mero de veces que sale el captcha
            // (y una m�s que ser�a la correcta).
            scraperConfig.Verify(setupChromeDriverExpression, Times.Exactly(contadorCaptcha.maximo + 1), failMessage: "El driver no se ha recargado las veces que tocaba"); 
            #endregion
        }
    }
}