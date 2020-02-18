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
    public class OrderBookModel
    {
        public int Id { get; set; }

        public string Inception { get; set; }

        public string Destination { get; set; }

        public string Price { get; set; }

        public string OrderName { get; set; }
        public string name { get; set; }
        public string Date { get; set; }
    }
}