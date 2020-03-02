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
    public class MainOrderStatusActivity: Fragment
    {
        private Button ButtonOrderAndItsStages;

        private Button ButtonMap;

        private Button ButtonEvents;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_main_order_status, container, false);

            ButtonOrderAndItsStages = view.FindViewById<Button>(Resource.Id.ButtonOrderAndItsStages);
            ButtonMap = view.FindViewById<Button>(Resource.Id.ButtonMap);
            ButtonEvents = view.FindViewById<Button>(Resource.Id.ButtonEvents);
            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();

            ButtonOrderAndItsStages.Click += async delegate
            {
                OrderListStagesActivity content1 = new OrderListStagesActivity();
                transaction1.Replace(Resource.Id.framelayout, content1).AddToBackStack(null).Commit();
            };

            ButtonMap.Click += async delegate
            {
                MapActivity content2 = new MapActivity();
                transaction1.Replace(Resource.Id.framelayout, content2).AddToBackStack(null).Commit();
            };

            ButtonEvents.Click += async delegate
            {
                EventsActivity content3 = new EventsActivity();
                transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
            };

            return view;
        }
    }
}