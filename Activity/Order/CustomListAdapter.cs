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
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using Entity.Repository;

namespace SmartBoxCity.Activity.Order
{
    public class CustomListAdapter : BaseAdapter<OrderBookModel>
    {
        Context context;
        List<OrderBookModel> orders;
        Android.App.FragmentTransaction manager;
        public CustomListAdapter(Context Context, List<OrderBookModel> List, FragmentManager Manager)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.orders = List;
        }
        public override OrderBookModel this[int position] => orders[position];

        public override int Count => orders.Count;

        public override long GetItemId(int position)
        {
            return position;//Convert.ToInt64(orders[position].Id);
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(context).Inflate(Resource.Layout.order_book_parameters, null);

            view.FindViewById<TextView>(Resource.Id.txtFrom).Text = orders[position].Inception;
            view.FindViewById<TextView>(Resource.Id.txtWhere).Text = orders[position].Destination;
            view.FindViewById<TextView>(Resource.Id.txtPrice).Text = orders[position].Price;
            var txt_date = view.FindViewById<TextView>(Resource.Id.txtDate);
            view.FindViewById<TextView>(Resource.Id.txtOrderName).Text = orders[position].OrderName;
            var btn = view.FindViewById<Button>(Resource.Id.btn_alarms);

            string date = orders[position].Date;
            string[] words = date.Split(' ');
            txt_date.Text = "";

            foreach (var word in words)
            {
                txt_date.Text += word + "\n";
            }

            btn.Click += async delegate
            {
                MainOrderStatusActivity content = new MainOrderStatusActivity();
                StaticOrder.Order_id = orders[position].OrderName;
                manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
            };

            return view;
        }
    }
}