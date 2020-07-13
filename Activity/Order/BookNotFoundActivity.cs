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
    public class BookNotFoundActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_not_found_book, container, false);
            var btn_add_order1 = view.FindViewById<Button>(Resource.Id.btn_add_order1);
            btn_add_order1.Click += delegate
            {
                try
                {
                    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                    AddOrderActivity content = new AddOrderActivity();
                    transaction.Replace(Resource.Id.framelayout, content);
                    transaction.Commit();
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