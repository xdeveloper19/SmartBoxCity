﻿using System;
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
    public class EventModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

    }
}