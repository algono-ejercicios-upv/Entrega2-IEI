using System;
using System.Collections.Generic;
using System.Text;

namespace Entrega2_IEI.Library
{
    public class Movil
    {
        public string Marca { get; }
        public string Modelo { get; }

        public string Nombre => $"{Marca} {Modelo}";

        public double Precio { get; }
        public string Moneda { get; }
        public double Descuento { get; }

        public Movil(string marca, string modelo, double precio = default, string moneda = default, double descuento = default)
        {
            Marca = marca;
            Modelo = modelo;
            Precio = precio;
            Moneda = moneda;
            Descuento = descuento;
        }
    }
}
