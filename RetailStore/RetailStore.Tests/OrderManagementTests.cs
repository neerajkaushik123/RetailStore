using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Retail.BusinessLogic;
using Retail.Models;
using Rhino.Mocks;


namespace RetailStore.Tests
{
    /// <summary>
    /// Summary description for OrderManagementTests
    /// </summary>
    [TestClass]
    public class OrderManagementTests
    {
        #region private members

        List<User> _usersList;
        List<Discount> _discountList;
        IRetailRepository _repository;
        MockRepository _mockrepo;

        #endregion

        #region Test Cases
        [TestInitialize]
        public void Setup()
        {

            _mockrepo = new MockRepository();

            #region Creation of static data
            //List of users in system

            _usersList = new List<User>();
            //Creating Employees
            _usersList.Add(new Employee() { Id = 1, FirstName = "Neeraj", LastName = "Kaushik", CreationDate = DateTime.Now, Dept = DepartmentType.Admin, Address = "New Delhi" });
            //Creating Customers
            _usersList.Add(new Affiliate() { Id = 2, FirstName = "Martin", LastName = "Buhl", CreationDate = DateTime.Now, Address = "France" });
            //Creating Affiliate old >2 yrs
            _usersList.Add(new Customer() { Id = 3, FirstName = "Karla", LastName = "Ball", CreationDate = new DateTime(2008, 3, 1), Address = "New York" });
            //Creating Affiliate
            _usersList.Add(new Customer() { Id = 4, FirstName = "Niels", LastName = "Kloster", CreationDate = DateTime.Now, Address = "Chicago" });
            //Creating Affiliate wi
            _usersList.Add(new Employee() { Id = 5, FirstName = "Niels", LastName = "Kloster", CreationDate = new DateTime(2006, 3, 1), Address = "Chicago" });


            //Discount List
            _discountList = new List<Discount>();

            _discountList.Add(new Discount() { Usertype = UserType.Employee, DiscountType = DiscountType.Percentage, Id = 1, DiscountValue = 30 });
            _discountList.Add(new Discount() { Usertype = UserType.Affiliate, DiscountType = DiscountType.Percentage, Id = 2, DiscountValue = 10 });

            #endregion

            _repository = _mockrepo.CreateMock<IRetailRepository>();

            Expect.Call(_repository.GetUsers()).Return(_usersList);
            Expect.Call(_repository.GetDiscounts()).Return(_discountList);
            Expect.Call(_repository.GetDiscountOnTotalBill()).Return(5);//5%
            Expect.Call(_repository.GetDiscountForOldCustomer()).Return(10);//5%

            _mockrepo.ReplayAll();
        }

        [TestMethod]
        public void TestIfEmployeeDiscount()
        {

            var orderMgm = new OrderManagement(_repository);

            Order order = new Order();
            order.UserId = 1;//Employee
            order.Id = OrderManagement.CreateOrderId();
            //Add Items
            order.AddItems(new Item() { Category = ItemCategory.Apparel, Id = 1, Price = 20 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 2, Price = 40 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 3, Price = 10 });

            orderMgm.ProcessOrder(order);

            //Expectation
            //GrossAmount=70
            //Discount=30% of non grocery items discount = 30% of 20=6
            //Net Amount= 70-6=64
            Assert.AreEqual(70, order.GrossAmount);
            Assert.AreEqual(64, order.NetPayableAmount);
            Assert.AreEqual(6, order.TotalDiscount);
        }

        [TestMethod]
        public void TestAffiliateDiscount()
        {
            var orderMgm = new OrderManagement(_repository);

            Order order = new Order();
            order.UserId = 2;//Affiliate
            order.Id = OrderManagement.CreateOrderId();
            //Add Items
            order.AddItems(new Item() { Category = ItemCategory.Apparel, Id = 1, Price = 20 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 2, Price = 40 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 3, Price = 10 });

            orderMgm.ProcessOrder(order);

            //Expectation
            //GrossAmount=70
            //Discount=10% of non grocery items discount = 10% of 20=2
            //Net Amount= 70-2=68
            Assert.AreEqual(70, order.GrossAmount);
            Assert.AreEqual(68, order.NetPayableAmount);
            Assert.AreEqual(2, order.TotalDiscount);
        }

        [TestMethod]
        public void TestNormalCustomerDiscount()
        {
            var orderMgm = new OrderManagement(_repository);

            Order order = new Order();
            order.UserId = 4;//Normal Customer
            order.Id = OrderManagement.CreateOrderId();
            //Add Items
            order.AddItems(new Item() { Category = ItemCategory.Apparel, Id = 1, Price = 200 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 2, Price = 400 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 3, Price = 150 });

            orderMgm.ProcessOrder(order);

            //Expectation
            Assert.AreEqual(750, order.GrossAmount);
            Assert.AreEqual(715, order.NetPayableAmount);
            Assert.AreEqual(35, order.TotalDiscount);
        }

        [TestMethod]
        public void TestOldCustomerDiscount()
        {
            var orderMgm = new OrderManagement(_repository);

            Order order = new Order();
            order.UserId = 3;//Old Customer
            order.Id = OrderManagement.CreateOrderId();
            //Add Items
            order.AddItems(new Item() { Category = ItemCategory.Apparel, Id = 1, Price = 200 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 2, Price = 400 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 3, Price = 150 });

            orderMgm.ProcessOrder(order);

            //Expectation
            //discount on apparel 10% = 20
            //discount amount 7*5=35 
            //totaldiscount=45
            Assert.AreEqual(750, order.GrossAmount);
            Assert.AreEqual(695, order.NetPayableAmount);
            Assert.AreEqual(55, order.TotalDiscount);
        }

        [TestMethod]
        public void TestPercentageDiscountAppliedOnGrocery()
        {
            var orderMgm = new OrderManagement(_repository);
            Order order = new Order();
            order.UserId = 1;//employee
            order.Id = OrderManagement.CreateOrderId();
            //Add Items
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 1, Price = 200 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 2, Price = 400 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 3, Price = 250 });

            orderMgm.ProcessOrder(order);

            //All items are grocery
            //Only this will be applicable: "For every $100 on the bill, there would be a $ 5 discount"

            Assert.AreEqual(850, order.GrossAmount);
            Assert.AreEqual(810, order.NetPayableAmount);
            Assert.AreEqual(40, order.TotalDiscount);
        }
        /// <summary>
        //test:A user can get only one of the percentage based discounts on a bill
        /// Here old employee will get 30% discount, he will not be entitled for extra 5% discount to be old customer
        /// </summary>
        [TestMethod]
        public void TestOnlyPercentageDiscountOnce()
        {
            var orderMgm = new OrderManagement(_repository);
            Order order = new Order();
            order.UserId = 5;//Old Employee
            order.Id = OrderManagement.CreateOrderId();
            //Add Items
            order.AddItems(new Item() { Category = ItemCategory.Apparel, Id = 1, Price = 200 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 2, Price = 400 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 3, Price = 250 });

            orderMgm.ProcessOrder(order);

            //All items are grocery
            //Only this will be applicable: "For every $100 on the bill, there would be a $ 5 discount"

            Assert.AreEqual(850, order.GrossAmount);
            Assert.AreEqual(755, order.NetPayableAmount);
            Assert.AreEqual(95, order.TotalDiscount);
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestIfUserIsAssociatedWithOrder()
        {
            var orderMgm = new OrderManagement(_repository);
            Order order = new Order();

            order.Id = OrderManagement.CreateOrderId();
            //Add Items
            order.AddItems(new Item() { Category = ItemCategory.Apparel, Id = 1, Price = 200 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 2, Price = 400 });
            //Add grocery Item
            order.AddItems(new Item() { Category = ItemCategory.Grocery, Id = 3, Price = 150 });

            orderMgm.ProcessOrder(order);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestIfUserIdIsNotAvailable()
        {
            var orderMgm = new OrderManagement(_repository);
            Order order = new Order();
            order.UserId = 10;
            orderMgm.ProcessOrder(order);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullOrder()
        {
            var orderMgm = new OrderManagement(_repository);
            Order order = null;
            orderMgm.ProcessOrder(order);
        }

        #endregion
    }
}
