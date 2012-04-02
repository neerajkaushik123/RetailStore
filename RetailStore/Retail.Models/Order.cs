using System;
using System.Collections.Generic;

namespace Retail.Models
{
    /// <summary>
    /// Order class
    /// </summary>
    public class Order
    {
        #region properties
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public List<Item> Items { get; private set; }
        public DateTime GeneratedOn { get; set; }
        public double GrossAmount { get; set; }
        public double NetPayableAmount { get; set; }
        #endregion

        #region Methods

        public double TotalDiscount { get { return GrossAmount - NetPayableAmount; } }

        public void AddItems(Item item)
        {
            if (Items == null)
                Items = new List<Item>();
            Items.Add(item);
        }

        #endregion
    }

}
