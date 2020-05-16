using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.OrderResponse
{
    public class EventsResponse: BaseResponseObject
    {
        public EventsResponse()
        {
            this.EVENTS = new List<EventResponse>();
        }
        public PartitialOrderResponse ORDER { get; set; }
        public List<EventResponse> EVENTS { get; set; }
    }
}
