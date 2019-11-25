namespace Entrega2_IEI.Library
{
    public class Phone
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        internal PhoneNameFormat NameFormat { get; set; }
        public string Name => $"{GetFormattedName()} - {PriceFormat(Price)}{GetFormattedDiscount()}";

        public double Price { get; set; }
        public double Discount { get; set; }

        public string Description { get; set; }

        internal Phone(string brand, string model, double price = default, double discount = default, string description = default)
        {
            Brand = brand;
            Model = model;

            Price = price;
            Discount = discount;

            Description = description;
            NameFormat = description == null ? PhoneNameFormat.BrandModel : PhoneNameFormat.Description;
        }

        private string GetFormattedName()
        {
            switch (NameFormat)
            {
                case PhoneNameFormat.Description: return Description;
                default: return $"{Brand} {Model}";
            }
        }

        private string GetFormattedDiscount() => Discount == 0.0 ? "" : $" - { PriceFormat(Discount)}";

        public override string ToString() => Name;

        public static string PriceFormat(object price) => string.Format("{0:C2}", price);
    }
}
