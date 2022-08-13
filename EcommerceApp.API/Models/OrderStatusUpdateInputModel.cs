using System;

namespace EcommerceApp.API.Models
{
    public class OrderStatusUpdateInputModel
    {
        public string OrderId { get; set; }
        public int StatusId { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
