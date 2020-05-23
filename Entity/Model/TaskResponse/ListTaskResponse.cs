using Entity.Model.BoxResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.TaskResponse
{
    public class ListTaskResponse: BaseResponseObject
    {
        public ListTaskResponse()
        {
            this.DRIVER = new DriverResponse();
            this.CONTAINERS = new List<ContainerResponse>();
            this.MAP_WAYPOINTS = new List<GeoLocation<string>>();
            this.TASKS = new List<TaskResponse>();
        }

        public List<ContainerResponse> CONTAINERS { get; set; }
        public DriverResponse DRIVER { get; set; }
        public List<GeoLocation<string>> MAP_WAYPOINTS { get; set; }
        public List<TaskResponse> TASKS { get; set; }
    }

    public class TaskResponse
    {
        public string address { get; set; }
        public string delivery_to { get; set; }
        public string done_at { get; set; }
        public string driver_id { get; set; }
        public string id { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string order_id { get; set; }
        public string priority { get; set; }
        public string required_tags { get; set; }
        public object started_at { get; set; }
        public string time_window { get; set; }
        public string title { get; set; }
        public string type { get; set; }
    }



}
