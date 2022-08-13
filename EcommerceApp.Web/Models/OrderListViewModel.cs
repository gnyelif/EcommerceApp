using System.Collections.Generic;

namespace EcommerceApp.Web.Models
{
    public class OrderListViewModel
    {

        public List<Order> OrderList { get; set; }
        public List<OrderStatus> OrderStatus { get; set; }
        public string OrderStatusText { get; set; }

    }
}
