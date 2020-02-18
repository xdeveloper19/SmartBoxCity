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
using SmartBoxCity.Model.OrderViewModel;

namespace SmartBoxCity.Activity.Order
{
    public class CustomListAdapter : BaseAdapter<OrderBookModel>
    {
        Context context;
        List<OrderBookModel> orders;
        public CustomListAdapter(Context Context, List<OrderBookModel> List)
        {
            this.context = Context;
            this.orders = List;
        }
        public override OrderBookModel this[int position] => orders[position];

        public override int Count => orders.Count;

        public override long GetItemId(int position)
        {
            return orders[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.order_book_parameters, null);
                view.FindViewById<TextView>(Resource.Id.txtFrom).Text = orders[position].Inception;
                view.FindViewById<TextView>(Resource.Id.txtWhere).Text = orders[position].Destination;
                view.FindViewById<TextView>(Resource.Id.txtPrice).Text = orders[position].Price;
                view.FindViewById<TextView>(Resource.Id.txtDate).Text = orders[position].Date;
                view.FindViewById<TextView>(Resource.Id.txtOrderName).Text = orders[position].OrderName;
            }
            return view;
        }
    }
}