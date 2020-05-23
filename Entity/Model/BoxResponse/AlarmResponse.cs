using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class AlarmResponse
    {
        public string acknowledged { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public object raised_at { get; set; }
    }
}
