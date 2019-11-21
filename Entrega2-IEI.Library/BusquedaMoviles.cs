using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entrega2_IEI.Library
{
    public static class BusquedaMoviles
    {
        public static List<Movil> BuscarAmazon(String brand,String model)
        {
            return AmazonScraper.Setup(brand, model);
        }

        public static void BuscarPCComponentes()
        {
            // TODO: Buscar en PC Componentes
        }
        public static void BuscarFnac()
        {
            // TODO: Buscar en Fnac
        }
    }
}
