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
using Badoualy.StepperIndicatorLib;

namespace SmartBoxCity.Activity.Order
{
    public class OrderActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //var view = inflater.Inflate(Resource.Layout.activity_order_status, container, false);
            //StepperIndicator stepper = view.FindViewById<StepperIndicator>(Resource.Id.sdsd);

            //return view;
            return base.View;
        }
    }
}