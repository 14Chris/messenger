using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Messenger.Facade.Response
{
    public class ReturnApiObject
    {
        public HttpStatusCode HttpStatus { get; set; }

        public ResponseType ResponseType { get; set; }

        public string Message { get; set; }

        public dynamic Result { get; set; }


        /// <summary>
        /// The default constructor take HttpStatusCode (complete the swith)
        /// </summary>
        public ReturnApiObject(HttpStatusCode httpStatusCode)
        {

            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    this.HttpStatus = HttpStatusCode.OK;
                    this.ResponseType = ResponseType.Success;
                    break;

                case HttpStatusCode.InternalServerError:
                    this.HttpStatus = HttpStatusCode.InternalServerError;
                    this.ResponseType = ResponseType.Error;
                    break;

                default:
                    this.HttpStatus = HttpStatusCode.OK;
                    this.ResponseType = ResponseType.Success;
                    break;
            }
        }

        public ReturnApiObject(HttpStatusCode httpStatus, ResponseType responseType, string message, dynamic result)
        {
            HttpStatus = httpStatus;
            ResponseType = responseType;
            Message = message;
            Result = result;
        }

        public ReturnApiObject(HttpStatusCode httpStatusCode, ResponseType responseType)
        {
            this.ResponseType = responseType;
            this.HttpStatus = httpStatusCode;
        }

    }
}
