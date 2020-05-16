using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class SensorResponse: BaseResponseObject
    {
        public string battery { get; set; }
        public string weight { get; set; }
        public string temperature { get; set; }
        public string humidity { get; set; }
        public string illumination { get; set; }
        public string gate { get; set; }
        public string Lock { get; set; }
        public string fold { get; set; }
    }
}
