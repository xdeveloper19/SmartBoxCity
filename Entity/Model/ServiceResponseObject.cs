using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Entity.Model
{
    public class ServiceResponseObject<T>
    where T : BaseResponseObject, new()
    {
        /// <summary>
        /// Статус ответа.
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// Информация о клиенте.
        /// </summary>
        public T ResponseData { get; set; }
    }
}
