
namespace Retail.Models
{
    public class Customer:User
    {
        public override UserType UserType
        {
            get
            {
                return UserType.Customer;
            }
        }

    }
}