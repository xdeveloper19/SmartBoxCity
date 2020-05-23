using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.OrderResponse
{
    /// <summary>
    /// Событие заказа.
    /// </summary>
    public class EventResponse
    {
        public DateTime created_at { get; set; }
        public string event_day { get; set; }
        public string id { get; set; }
        public string message { get; set; }
        public string order_id { get; set; }
        public string type { get; set; }
    }
}
