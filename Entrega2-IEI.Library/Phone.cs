namespace Entrega2_IEI.Library
{
    public class Phone
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        public string Name => $"{Brand} {Model} - {Price} {Currency} - {Discount} {Currency}";

        public double Price { get; set; }
        public string Currency { get; set; }
        public double Discount { get; set; }

        public Phone(string brand, string model, double price = default, string currency = default, double discount = default)
        {
            Brand = brand;
            Model = model;
            Price = price;
            Currency = currency;
            Discount = discount;
        }
    }
}
