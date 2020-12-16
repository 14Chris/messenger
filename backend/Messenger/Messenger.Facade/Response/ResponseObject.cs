using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Messenger.Facade.Response
{
    public class ResponseObject
    {
        public ResponseType ResponseType { get; set; }

        public string Message { get; set; }

        public dynamic Result { get; set; }


        public ResponseObject(ResponseType responseType, string message, dynamic result)
        {
            this.ResponseType = responseType;
            this.Message = message;
            this.Result = result;
        }

        public ResponseObject(ResponseType responseType)
        {
            this.ResponseType = responseType;
        }

    }
}
