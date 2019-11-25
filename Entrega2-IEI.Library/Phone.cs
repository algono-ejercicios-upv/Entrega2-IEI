namespace Entrega2_IEI.Library
{
    public class Phone
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        public string Name => $"{Brand} {Model} - {PriceFormat(Price)} - {PriceFormat(Discount)}";

        public double Price { get; set; }
        public double Discount { get; set; }

        public Phone(string brand, string model, double price = default, double discount = default)
        {
            Brand = brand;
            Model = model;
            Price = price;
            Discount = discount;
        }

        private static string PriceFormat(double price) => string.Format("{0:C2}", price); 
    }
}
