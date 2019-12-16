using Entrega2_IEI.Library;
using Entrega2_IEI.Library.Scrapers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Entrega2_IEI.Test
{
    [TestClass]
    public class AmazonScraperTests
    {
        [TestMethod]
        public void AmazonScraperAvoidsCaptchaPage()
        {
            var scraper = new Mock<AmazonScraper>();
            var scraperConfig = new Mock<ScraperConfig>();

            
        }
    }
}
