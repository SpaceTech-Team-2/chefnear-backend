using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChefNear.Domain.Exceptions
{
    public class PaymentGatewayException : Exception
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;

        public PaymentGatewayException()
        {
        }

        public PaymentGatewayException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public PaymentGatewayException(string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
