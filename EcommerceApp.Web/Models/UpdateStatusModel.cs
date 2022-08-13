using System;

namespace EcommerceApp.Web.Models
{
    public class UpdateStatusModel
    {
        public string OrderId { get; set; }
        public int StatusId { get; set; }
        public DateTime ChangeDate { get; set; }

    }
}
