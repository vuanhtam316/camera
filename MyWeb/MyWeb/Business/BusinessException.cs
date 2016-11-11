using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Business
{
    public class BusinessException : Exception
    {
        public object Result;
        public BusinessException(string message, object result = null)
            : base(message)
        {
            this.Result = result;
        }
    }
}