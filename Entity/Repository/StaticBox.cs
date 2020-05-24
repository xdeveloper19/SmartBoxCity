using Entity.Model.AlarmResponse;
using Entity.Model.BoxResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Repository
{
    public static class StaticBox
    {
        public static bool isDepot;

        public static string id { get; set; }
        public static List<AlarmResponseData> alarms { get; set; }
        public static string battery { get; set; }
        public static string weight { get; set; }
        public static string temperature { get; set; }
        public static string humidity { get; set; }
        public static string illumination { get; set; }
        public static string gate { get; set; }
        public static string Lock { get; set; }
        public static string fold { get; set; }
        public static string driver_id { get; set; }
        public static string cloud_key { get; set; }
        public static string owner_id { get; set; }
        public static string vpn_ip { get; set; }
        public static string busy { get; set; }
        public static string event_count { get; set; }


        public static void AddInfoSensors(SensorResponse response)
        {
            battery = response.battery;
            weight = response.weight;
            temperature = response.temperature;
            humidity = response.humidity;
            illumination = response.illumination;
            gate = response.gate;
            Lock = response.Lock;
            fold = response.fold;
        }

        public static void AddInfoContainer(PartContainerResponse box, string event_count2)
        {
            busy = box.busy;
            cloud_key = box.cloud_key;
            driver_id = box.driver_id;
            vpn_ip = box.vpn_ip;
            id = box.id;
            owner_id = box.owner_id;
            event_count = event_count2;
        }

        public static void AddInfoAlarms(List<AlarmResponseData> alms)
        {
            alarms = alms; 
        }
    }
}
