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

namespace SmartBoxCity.Model.BoxViewModel
{
    public class SensorResponse : BaseResponseObject
    {
        public string battery { get; set; }
        public string weight { get; set; }
        public string temperature { get; set; }
        public string humidity { get; set; }
        public string illumination { get; set; }
        public string gate { get; set; }
        public string Lock {get;set;}

    }
}