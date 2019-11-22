using System.Collections.Generic;

namespace Entrega2_IEI.Library
{
    public static class BusquedaMoviles
    {
        public static List<Movil> BuscarAmazon(string brand, string model)
            => AmazonScraper.SearchPhone(brand, model);

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
