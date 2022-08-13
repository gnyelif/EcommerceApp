using EcommerceApp.Web.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace EcommerceApp.Web.Logger
{
    public class MyMongoLogger : IMyMongoLogger
    {
        private MongoClientSettings settings;
        private MongoClient client;
        IMongoCollection<Logs> collection;
        public MyMongoLogger()
        {
            settings = MongoClientSettings.FromConnectionString("mongodb+srv://gunayelif:Ecommerce_2022@ecommercecluster.3g0xybb.mongodb.net/EcommerceDB?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            client = new MongoClient(settings);
            var database = client.GetDatabase("EcommerceDB");
            collection = database.GetCollection<Logs>("Logs");
        }
        public void Errorlog(string message)
        {
            var item = new Logs()
            {
                _Id = ObjectId.GenerateNewId(),
                Message = message,
                Level = "Error",
                TimeStamp = DateTime.Now
            };

            collection.InsertOne(item);
        }

        public void Infolog(string category, string message)
        {
            var item = new Logs()
            {
                _Id = ObjectId.GenerateNewId(),
                Message = message,
                Level = "Info",
                TimeStamp = DateTime.Now,
                Category=category
            };

            collection.InsertOne(item);
        }
    }
}
