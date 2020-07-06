using Entity.Model;
using Entity.Model.TaskResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Repository
{
    public static class StaticTask
    {
        public static string comment;
        public static List<string> containers_id { get; set; }
        public static List<GeoLocation<double>> way_points { get; set; }
        public static string address { get; set; }
        public static string box_id { get; set; }
        public static string delivery_to { get; set; }
        public static string done_at { get; set; }
        public static string driver_id { get; set; }
        public static string id { get; set; }
        public static string lat { get; set; }
        public static string lng { get; set; }
        public static string order_id { get; set; }
        public static string priority { get; set; }
        public static string required_tags { get; set; }
        public static object started_at { get; set; }
        public static string time_window { get; set; }
        public static string title { get; set; }
        public static string type { get; set; }
        public static bool IsStoppedGeo { get; set; }
        public static bool IsStoppedGettingTasks { get; set; }

        public static void AddInfoTask(TaskResponse task)
        {
            address = task.address;
            done_at = task.done_at;
            driver_id = task.driver_id;
            id = task.id;
            lat = task.lat;
            lng = task.lng;
            order_id = task.order_id;
            priority = task.priority;
            required_tags = task.required_tags;
            started_at = task.started_at;
            time_window = task.time_window;
            title = task.title;
            type = task.type;
        }

        public static void AddContainersID(List<string> vs)
        {
            if (type == "delivery")
            {
                containers_id = vs;
            }
            return;
        }

        public static void AddWayPoints(List<GeoLocation<string>> wp)
        {
            way_points = new List<GeoLocation<double>>(wp.Capacity);

            for (int i = 0; i < wp.Count; i++)
            {
                way_points.Add(new GeoLocation<double>
                {
                    lat = Convert.ToDouble(wp[i].lat.Replace(".", ",")),
                    lng = Convert.ToDouble(wp[i].lng.Replace(".", ","))
                });
            }
        }
    }
}
