using System;

namespace EcommerceApp.Web.Logger
{
    public interface IMyMongoLogger
    {
        public void Infolog(string category,string message);
        public void Errorlog(string message);
    }
}
