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
    public class ListOrdersActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_order_book, container, false);

            //try
            //{
                
            //}
            //catch (Exception ex)
            //{
            //    Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            //}
            return view;
        }
    }
}