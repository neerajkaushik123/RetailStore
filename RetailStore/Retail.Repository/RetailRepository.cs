using System;
using System.Collections.Generic;
using Retail.Models;

namespace Retail.Repository
{
    public class RetailRepository : IRetailRepository
    {
        public IList<User> GetUsers()
        {
            //todo:
            //Fetch data from databases

            //I am mocking data here
            var usersList = new List<User>();


            //Creating Employees
            usersList.Add(new Employee() { Id = 1, FirstName = "Neeraj", LastName = "Kaushik", CreationDate = DateTime.Now, Dept = DepartmentType.Admin, Address = "New Delhi" });
            //Creating Customers
            usersList.Add(new Affiliate() { Id = 2, FirstName = "Martin", LastName = "Buhl", CreationDate = DateTime.Now, Address = "France" });
            //Creating Affiliate old >2 yrs
            usersList.Add(new Customer() { Id = 3, FirstName = "Karla", LastName = "Ball", CreationDate = new DateTime(2008, 3, 1), Address = "New York" });
            //Creating Affiliate
            usersList.Add(new Customer() { Id = 4, FirstName = "Niels", LastName = "Kloster", CreationDate = DateTime.Now, Address = "Chicago" });
            //Creating Affiliate wi
            usersList.Add(new Employee() { Id = 5, FirstName = "Niels", LastName = "Holger", CreationDate = new DateTime(2006, 3, 1), Address = "Chicago" });

            return usersList;

        }

        public IList<Discount> GetDiscounts()
        {
            //todo:
            //Fetch data from databases

            //I am mocking data here
            //Discount List
            List<Discount> discountList = new List<Discount>();

            discountList.Add(new Discount() { DiscountType = DiscountType.Percentage, Id = 1, DiscountValue = 30 });
            discountList.Add(new Discount() { DiscountType = DiscountType.Percentage, Id = 2, DiscountValue = 10 });

            return discountList;
        }

        public double GetDiscountOnTotalBill()
        {
            return 5.0;
        }


        public double GetDiscountForOldCustomer()
        {
            return 10.0d;
        }
    }
}
