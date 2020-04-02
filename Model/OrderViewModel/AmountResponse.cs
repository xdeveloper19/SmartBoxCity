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