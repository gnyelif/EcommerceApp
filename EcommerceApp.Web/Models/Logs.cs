using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace EcommerceApp.Web.Models
{
    public class Logs
    {
        [BsonId]
        public ObjectId _Id { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

