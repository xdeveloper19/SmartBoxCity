using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SmartBoxCity.Model.OrderViewModel
{
    public class ArchiveResponse
    {
        //перепроверь с архивом 
        public string id { get; set; }
        public string inception_address { get; set; }
        public string inception_lat { get; set; }
        public string inception_lng { get; set; }
        public string destination_address { get; set; }
        public string destination_lat { get; set; }
        public string destination_lng { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string qty { get; set; }
        public string cargo_type { get; set; }
        public string cargo_class { get; set; }
        public string distance { get; set; }
        public string insurance { get; set; }
        public object stage2_datetime { get; set; }
        public object stage5_datetime { get; set; }
        public string payment_id { get; set; }
        public string order_stage_id { get; set; }
        public object created_at { get; set; }
        public string payment_amount { get; set; }
        public string payment_status { get; set; }
        public string order_stage_name { get; set; }
        public object last_stage_at { get; set; }
        public string container_id { get; set; }
        //public SensorResponse sensors_status { get; set; }
        public string event_count { get; set; }
    }
}