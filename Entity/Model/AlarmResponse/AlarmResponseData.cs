using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AlarmResponse
{
    public class AlarmResponseData
    {
        public string depot { get; set; }
        public string acknowledged { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string container_id { get; set; }
        public string raised_at { get; set; }
    }
}
