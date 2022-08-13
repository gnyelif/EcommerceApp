using EcommerceApp.Web.Logger;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;


namespace EcommerceApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration configuration;
        private IMyMongoLogger _mylogger;
        EcommerceDBContext _context = new EcommerceDBContext();
        public HomeController(EcommerceDBContext context, IConfiguration iConfig, IMyMongoLogger mylogger)
        {
            _context = context;
            configuration = iConfig;
            _mylogger = mylogger;
        }

        public IActionResult Index()
        {

            OrderListViewModel model = new OrderListViewModel();

            model.OrderList = _context.Order.ToList();
            model.OrderStatus = _context.OrderStatus.ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddItemList([FromBody] IEnumerable<AddOrderItemModel> model)
        {
            List<AddOrderResponseModel> result = new List<AddOrderResponseModel>();
            List<AddOrderRequestModel> list = new List<AddOrderRequestModel>();

            foreach (var item in model)
            {
                AddOrderRequestModel order = new AddOrderRequestModel();
                order.CustomerOrderId = item.CustomerOrderNo;
                order.SenderAddress = item.SenderAddress;
                order.DestinationAddress = item.DestinationAddress;
                order.Quantity = Convert.ToInt32(item.Quantity);
                order.QuantityUnit = item.QuantityUnit;
                order.Weight = Convert.ToInt32(item.Weight);
                order.WeightUnit = item.WeightUnit;
                order.MaterialCode = item.MaterialCode;
                order.MaterialName = item.MaterialName;
                order.Not = item.Not;

                list.Add(order);
            }
            #region api call

            string Url = configuration.GetSection("EcommerceAPIUrl").Value.ToString() + "/order/create";

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072; //TLS v1.2

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(list,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                    streamWriter.Write(json);

                    _mylogger.Infolog("create-order-request", json);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode.ToString() == "OK")
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        result = new JavaScriptSerializer().Deserialize<List<AddOrderResponseModel>>(streamReader.ReadToEnd());
                        _mylogger.Infolog("create-order-response", new JavaScriptSerializer().Serialize(result));
                    }
                }
                //close
                response.Close();
            }
            catch (Exception ex)
            {

                result.Add(
                        new AddOrderResponseModel
                        {
                            Status = "1",
                            StatusMessage = "Güncelleme başarısız"
                        }
                        );
                _mylogger.Errorlog(ex.Message);
            }

            #endregion

            return Ok(result);
        }

        public JsonResult UpdateStatus(int statusCode, string orderId)
        {
            ResultReturnModel result = new ResultReturnModel();
            UpdateStatusModel input = new UpdateStatusModel();

            input.OrderId = orderId;
            input.StatusId = statusCode;
            input.ChangeDate = DateTime.Now;

            #region api call

            string Url = configuration.GetSection("EcommerceAPIUrl").Value.ToString() + "/order/update";
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072; //TLS v1.2

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(input,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                    streamWriter.Write(json);

                    _mylogger.Infolog("update-order-request", json);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode.ToString() == "OK")
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        result = new JavaScriptSerializer().Deserialize<ResultReturnModel>(streamReader.ReadToEnd());

                        _mylogger.Infolog("update-order-response", new JavaScriptSerializer().Serialize(result));
                    }
                }
                //close
                response.Close();
            }
            catch (Exception ex)
            {
                result.Status = 1;
                result.StatusMessage = "Güncelleme işlemi başarısız";

                _mylogger.Errorlog(ex.Message);
            }
            #endregion

            return Json(new { Status = result.Status, StatusMessage = result.StatusMessage });
        }
    }
}
