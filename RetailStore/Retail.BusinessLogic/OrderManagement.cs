using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Retail.Models;


namespace Retail.BusinessLogic
{
    /// <summary>
    /// This class contains business logic to process orders
    /// </summary>
    public class OrderManagement
    {
        #region Static

        static int _orderId = 0;
        /// <summary>
        /// Create Order Id
        /// </summary>
        /// <returns></returns>
        public static int CreateOrderId()
        {
            return Interlocked.Increment(ref _orderId);
        }
        
        #endregion

        #region private members

        private Dictionary<int, User> _userList = new Dictionary<int, User>();

        private Dictionary<UserType, Discount> _userTypeDiscount = new Dictionary<UserType, Discount>();

        IRetailRepository _repo;


        /// <summary>
        /// populate Static data
        /// </summary>
        private void PopulateStaticData()
        {
            //Fetch from Database
            //convert list to dictionary for fast retrieval of data

            var listuser = _repo.GetUsers();

            foreach (User usr in listuser)
                _userList.Add(usr.Id, usr);

            var discounts = _repo.GetDiscounts();

            foreach (Discount disc in discounts)
                _userTypeDiscount.Add(disc.Usertype, disc);

        }

        /// <summary>
        /// Validate inputs
        /// </summary>
        /// <param name="order"></param>
        private void Validate(Order order)
        {
            if (order==null)
                throw new ArgumentNullException("Order is blank.");

            //order should have user associated
            if (order.UserId == 0)
                throw new ValidationException("Order should have user associated.");

            //User is not present in system
            if (!_userList.ContainsKey(order.UserId))
                throw new ValidationException(string.Format("UserId {0} is not available in system", order.UserId));
        }


        #endregion

        #region ctor
        public OrderManagement(IRetailRepository repo)
        {
            _repo = repo;
            PopulateStaticData();
        }
        #endregion

        #region public member
        
        /// <summary>
        /// Process Order Calculation
        /// </summary>
        /// <param name="order"></param>
        public void ProcessOrder(Order order)
        {
            //processing of Order
            //Validations

            Validate(order);

            User user = _userList[order.UserId];

            double discount = _userTypeDiscount.ContainsKey(user.UserType) ? _userTypeDiscount[user.UserType].DiscountValue : 0;

            //if user is customer and more than 2 yrs old 
            //2 yrs can be configured
            if (user.UserType == UserType.Customer && (DateTime.Now - user.CreationDate).Days > 365 * 2)
                discount = _repo.GetDiscountForOldCustomer();

            if (discount > 0)
            {
                //Calculate Bill
                foreach (Item itm in order.Items)
                {
                    //% discount not applied on grocery
                    if (itm.Category != ItemCategory.Grocery)
                        itm.NetPrice = itm.Price - (itm.Price * discount * .01);
                }
            }

            //Calculate discount on final amount breakup
            double sumOfNetPrice = order.Items.Sum(itm => itm.NetPrice);
            double sumOfGrossPrice = order.Items.Sum(itm => itm.Price);

            int breakup = (int)Math.Abs(sumOfNetPrice / 100);

            double discountapplied = breakup * _repo.GetDiscountOnTotalBill();//

            order.NetPayableAmount = sumOfNetPrice - discountapplied;
            order.GrossAmount = sumOfGrossPrice;
        }

        #endregion

    }
}
