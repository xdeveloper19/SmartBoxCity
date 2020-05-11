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

namespace SmartBoxCity.Activity.Order
{
    public class MapActivity: Fragment
    {
        private TextView EditFrom;
        private TextView EditTo;
        private TextView EditWeight;
        private TextView EditLenhWidHeig;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_map, container, false);

            EditFrom = view.FindViewById<TextView>(Resource.Id.MapEditFrom);
            EditTo = view.FindViewById<TextView>(Resource.Id.MapEditTo);
            EditWeight = view.FindViewById<TextView>(Resource.Id.MapEditWeight);
            EditLenhWidHeig = view.FindViewById<TextView>(Resource.Id.MapEditLenhWidHeig);

            return view;
        }
    }
}