using System.Collections.Generic;

namespace Retail.Models
{
    public interface IRetailRepository
    {
        IList<Discount> GetDiscounts();
        IList<User> GetUsers();
        double GetDiscountOnTotalBill();
        double GetDiscountForOldCustomer();
    }
}
