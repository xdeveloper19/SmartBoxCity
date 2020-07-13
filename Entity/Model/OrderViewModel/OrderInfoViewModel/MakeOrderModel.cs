using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.OrderViewModel.OrderInfoViewModel
{
    public class MakeOrderModel
    {
        public string volume { get; set; }

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
        public string insurance { get; set; }
        public string for_date { get; set; }
        public string for_time { get; set; }
        public string receiver { get; set; }
        public string cargo_loading { get; set; }
    }
}
