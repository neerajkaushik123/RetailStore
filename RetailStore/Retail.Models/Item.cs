
namespace Retail.Models
{

    public class Item
    {
        public int Id { get; set; }
        public ItemCategory Category { get; set; }
        public double Price { get; set; }
        private bool isNetPriceSet = false;
        private double _netPrice = 0;

        public double NetPrice
        {
            get { return isNetPriceSet ? _netPrice : Price; }
            set
            {
                isNetPriceSet = true;
                _netPrice = value;
            }
        }
    }

    public enum ItemCategory
    {
        Grocery,
        Electronics,
        Apparel,
        Misc
    }

    public enum DiscountType
    {
        Percentage,
        Amount
    }

}