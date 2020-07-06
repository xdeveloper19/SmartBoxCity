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
using Plugin.Settings;

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
            RetainInstance = true;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_main_order_status, container, false);

            ButtonOrderAndItsStages = view.FindViewById<Button>(Resource.Id.ButtonOrderAndItsStages);
            ButtonMap = view.FindViewById<Button>(Resource.Id.ButtonMap);
            ButtonEvents = view.FindViewById<Button>(Resource.Id.ButtonEvents);
            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();

            ButtonOrderAndItsStages.Click += delegate
            {
                try
                {
                    OrderListStagesActivity content1 = new OrderListStagesActivity();
                    if (CrossSettings.Current.GetValueOrDefault("role", "") == "driver")
                    {
                        transaction1.Replace(Resource.Id.frameDriverlayout, content1).AddToBackStack(null);
                        transaction1.Commit();
                    }
                    else
                    {
                        transaction1.Replace(Resource.Id.framelayout, content1).AddToBackStack(null);
                        transaction1.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                }
            };

            ButtonMap.Click += delegate
            {
                try
                {
                    MapActivity content2 = new MapActivity();
                    if (CrossSettings.Current.GetValueOrDefault("role", "") == "driver")
                        transaction1.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null).Commit();
                    else
                        transaction1.Replace(Resource.Id.framelayout, content2).AddToBackStack(null).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                }
            };

            ButtonEvents.Click += delegate
            {
                try
                {
                    EventsActivity content3 = new EventsActivity();
                    if (CrossSettings.Current.GetValueOrDefault("role", "") == "driver")
                        transaction1.Replace(Resource.Id.frameDriverlayout, content3).AddToBackStack(null).Commit();
                    else
                        transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                }
            };

            return view;
        }
    }
}