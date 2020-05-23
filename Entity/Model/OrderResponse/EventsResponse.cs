using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.OrderResponse
{
    public class EventsResponse: BaseResponseObject
    {
        public EventsResponse()
        {
            this.EVENTS = new Dictionary<string, List<EventResponse>>();
        }
        public PartitialOrderResponse ORDER { get; set; }
        public Dictionary<string, List<EventResponse>> EVENTS { get; set; }
    }
}
