
namespace Retail.Models
{
    public class Affiliate :User
    {
        public override UserType UserType
        {
            get
            {
                return UserType.Affiliate;
            }
        }
    }
}