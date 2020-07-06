using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class EventsBoxResponse: BaseResponseObject
    {
        public EventsBoxResponse()
        {
            this.EVENTS = new Dictionary<string, List<EventResponse>>();
        }
        public PartContainerResponse CONTAINER { get; set; }
        public Dictionary<string, List<EventResponse>> EVENTS { get; set; }
    }

    public class EventResponse
    {
        public string created_at { get; set; }
        public string message { get; set; }
        public string type { get; set; }
    }
}
