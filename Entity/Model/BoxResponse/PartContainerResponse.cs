using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class PartContainerResponse
    {
        public string id { get; set; }
        public string driver_id { get; set; }
        public string cloud_key { get; set; }
        public string owner_id { get; set; }
        public string vpn_ip { get; set; }
        public string busy { get; set; }
    }
}
