using System;
using System.Collections.Generic;

namespace EcommerceApp.Web.Models
{
    public partial class OrderStatusChange
    {
        public int Id { get; set; }
        public string OrderId { get; set; }

        public int? StatusId { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
