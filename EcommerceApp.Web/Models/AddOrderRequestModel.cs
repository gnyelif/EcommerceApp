using System;

namespace EcommerceApp.Web.Models
{
    public class AddOrderRequestModel
    {
        public Guid OrderId { get; set; }
        public string CustomerOrderId { get; set; }
        public int StatusId { get; set; }
        public string SenderAddress { get; set; }
        public string DestinationAddress { get; set; }
        public int? Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public int? Weight { get; set; }
        public string WeightUnit { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Not { get; set; }
    }
}
