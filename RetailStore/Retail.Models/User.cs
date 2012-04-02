using System;

namespace Retail.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public virtual UserType UserType { get; private set; }
        public DateTime CreationDate { get; set; }
    }

    public enum UserType
    {
        Employee,
        Customer,
        Affiliate,
        Partner
    }
}