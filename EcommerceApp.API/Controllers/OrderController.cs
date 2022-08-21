
using EcommerceApp.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        EcommerceDBContext _context = new EcommerceDBContext();
        public OrderController(EcommerceDBContext context, ILogger<OrderController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public ActionResult Get()
        {
            var values = _context.Order.ToList();
            return Ok(values);
            //return new string[] { "value1", "value2" };
        }

        [Route("update")]
        [HttpPost]
        public IActionResult Create([FromBody] OrderStatusUpdateInputModel value)
        {
            OrderStatusUpdateOutputModel result = new OrderStatusUpdateOutputModel();
            var order = _context.Order.Where(a => a.OrderId == Guid.Parse(value.OrderId)).FirstOrDefault();

            try
            {
                if (order != null)
                {
                    #region order update
                    order.StatusId = value.StatusId;
                    _context.Order.Update(order);
                    _context.SaveChanges();
                    #endregion

                    #region add order status change
                    OrderStatusChange newItem = new OrderStatusChange();
                    newItem.StatusId = value.StatusId;
                    newItem.OrderId = value.OrderId;
                    newItem.ChangeDate = value.ChangeDate;
                    _context.OrderStatusChange.Add(newItem);
                    _context.SaveChanges();
                    #endregion
                }
                result.Status = 0;
                result.StatusMessage = "Güncelleme başarılı";
            }
            catch (Exception)
            {
                result.Status = 1;
                result.StatusMessage = "Güncelleme başarısız";

            }
            return Ok(result);
        }

        [Route("create")]
        [HttpPost]
        public IActionResult CreateOrder([FromBody] List<OrderInputModel> value)
        {
            List<OrderOutputModel> output = new List<OrderOutputModel>();

            foreach (var item in value)
            {
                // Aynı müşteri için tekrar eden müşteri sipariş numarası yeniden kabul edilmemelidir.
                if (_context.Order.Where(a => a.CustomerOrderId.ToLower() == item.CustomerOrderId.ToLower()).FirstOrDefault() != null)
                {
                    output.Add(
                        new OrderOutputModel
                        {
                            CustomerOrderId = item.CustomerOrderId,
                            OrderId = Guid.Empty.ToString(),
                            Status = "1",
                            StatusMessage = item.CustomerOrderId + " müşteri sipariş numarasına ait sistemde sipariş bulunmaktadır."
                        }
                        );
                }
                else
                {
                    Order newOrder = new Order();
                    newOrder.OrderId = Guid.NewGuid();
                    newOrder.CustomerOrderId = item.CustomerOrderId;
                    newOrder.SenderAddress = item.SenderAddress;
                    newOrder.DestinationAddress = item.DestinationAddress;
                    newOrder.Quantity = item.Quantity;
                    newOrder.QuantityUnit = item.QuantityUnit;
                    newOrder.Weight = item.Weight;
                    newOrder.WeightUnit = item.WeightUnit;
                    newOrder.Not = item.Not;
                    //Sipariş sisteme ilk kaydedildiğinde durum bilgisi “Sipariş Alındı” olmalıdır. 
                    newOrder.StatusId = 1;
                    newOrder.MaterialCode = item.MaterialCode;

                    _context.Order.Add(newOrder);
                    _context.SaveChanges();

                    #region material
                    //Sipariş verisindeki malzeme kodu eğer malzeme tablosunda yoksa malzeme tabloya eklenmelidir.
                    var material = _context.Material.Where(a => a.MaterialCode.ToLower() == item.MaterialCode.ToLower()).FirstOrDefault();
                    if (material == null)
                    {
                        Material newItem = new Material();
                        newItem.MaterialCode = item.MaterialCode;
                        newItem.MaterialName = item.MaterialName;
                        _context.Material.Add(newItem);
                        _context.SaveChanges();
                    }
                    #endregion

                    output.Add(
                        new OrderOutputModel
                        {
                            CustomerOrderId = item.CustomerOrderId,
                            OrderId = newOrder.OrderId.ToString(),
                            Status = "0",
                            StatusMessage = "Sipariş başarılı kayıt edilmiştir." 
                        }
                        );
                }
            }
            return Ok(output);
        }
    }
}
