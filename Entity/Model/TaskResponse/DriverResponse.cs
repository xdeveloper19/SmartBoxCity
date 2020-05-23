using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.TaskResponse
{
    public class DriverResponse
    {
        public string id { get; set; }
        public string number { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string gps_time { get; set; }
        public string container_count { get; set; }
        public string container_state { get; set; }
        public string busy { get; set; }
    }
}
