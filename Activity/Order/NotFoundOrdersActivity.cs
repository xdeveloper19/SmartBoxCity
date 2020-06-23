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
    public class NotFoundOrdersActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_not_found_order, container, false);
            var btn_add_order = view.FindViewById<Button>(Resource.Id.NotFoundOrderBtnAddOrder);
            btn_add_order.Click += delegate
            {
                try
                {
                    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                    AddOrderActivity content = new AddOrderActivity();
                    transaction.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                }
            };
            return view;
        }
    }
}