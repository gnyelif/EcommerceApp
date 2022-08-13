namespace EcommerceApp.Web.Models
{
    public class AddOrderResponseModel
    {
        public string OrderId { get; set; }
        public string CustomerOrderId { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
    }
}
