namespace EcommerceApp.Web.Models
{
    public class AddOrderItemModel
    {
        public string Id { get; set; }
        public string CustomerOrderNo { get; set; }
        public string SenderAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string Weight { get; set; }
        public string WeightUnit { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Not { get; set; }
    }
}
