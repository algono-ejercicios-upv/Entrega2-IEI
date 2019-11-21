using System;
using System.Collections.Generic;
using System.Text;

namespace Entrega2_IEI.Library
{
    public class Movil
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }

        public string Nombre => $"{Marca} {Modelo} - {Precio} EUR - {Descuento} EUR";

        public double Precio { get; set; }
        public string Moneda { get; set; }
        public double Descuento { get; set; }

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
