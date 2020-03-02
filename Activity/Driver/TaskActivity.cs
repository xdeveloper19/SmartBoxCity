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
using SmartBoxCity.Activity.Order;

namespace SmartBoxCity.Activity.Driver
{
    public class TaskActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_tasks, container, false);
            //Button btn_about_order = view.FindViewById<Button>(Resource.Id.btn_about_order);
            Button btn_prime = view.FindViewById<Button>(Resource.Id.btn_prime2);

            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();

            //btn_about_order.Click += async delegate
            //{
            //    OrderActivity content = new OrderActivity();
            //    transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
            //};

            btn_prime.Click += async delegate
            {
                TaskNotFoundActivity content = new TaskNotFoundActivity();
                transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
            };
            return view;
        }
    }
}