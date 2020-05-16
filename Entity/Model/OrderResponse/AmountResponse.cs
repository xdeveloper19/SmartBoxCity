using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.OrderResponse
{
    public class AmountResponse: BaseResponseObject
    {
        public string amount { get; set; }
        public string inception_address { get; set; }
        public string destination_address { get; set; }
        public string distance { get; set; }
        public string work_amount { get; set; }
        public string insurance_amount { get; set; }
        public string inception_city { get; set; }
        public string destination_city { get; set; }
    }
}
