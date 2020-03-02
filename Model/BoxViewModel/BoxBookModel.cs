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
    public class BoxBookModel
    {
        public int Id { get; set; }
        public int ImageView { get; set; }
        public string OrderId { get; set; }
        public string BoxId { get; set; }
    }
}