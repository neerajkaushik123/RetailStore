
namespace Retail.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public DiscountType DiscountType { get; set; }
        public double DiscountValue { get; set; }
        public UserType Usertype { get; set; }
    }
}
