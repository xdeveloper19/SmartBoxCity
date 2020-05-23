using Entity.Model.TaskResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Repository
{
    public static class StaticDriver
    {
        public static string id { get; set; }
        public static string number { get; set; }
        public static string lat { get; set; }
        public static string lng { get; set; }
        public static string gps_time { get; set; }
        public static string container_count { get; set; }
        public static string container_state { get; set; }
        public static string busy { get; set; }

        public static void AddInfoDriver(DriverResponse model)
        {
            id = model.id;
            number = model.number;
            lat = model.lat;
            lng = model.lng;
            gps_time = model.gps_time;
            container_count = model.container_count;
            container_state = model.container_state;
            busy = model.busy;
        }

       
    }
}
