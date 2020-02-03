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
    public class ListOrdersActivity: Fragment
    {
        private ListView lstOrder;
        private EditText editEnterOrder;
        public static List<OrderBookModel> orderlist;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_order_book, container, false);
            lstOrder = view.FindViewById<ListView>(Resource.Id.orderlistview);
            
            //editEnterOrder.TextChanged += EtSearch_TextChanged;
            orderlist = new List<OrderBookModel>();
            OrderBookModel p1 = new OrderBookModel()
            {
                Id = 1,
                name = "Заказ №1",
                Date = "12/11/2019"
            };
            OrderBookModel p2 = new OrderBookModel()
            {
                Id = 2,
                name = "Заказ №2",
                Date = "12/11/2029"
            };
            OrderBookModel p3 = new OrderBookModel()
            {
                Id = 3,
                name = "Заказ №3",
                Date = "12/11/2039"
            };
            orderlist.Add(p1);
            orderlist.Add(p2);
            orderlist.Add(p3);
            UpdateList();
            lstOrder.ItemClick += ListOrders_ItemClick;
            return view;
        }

        private void ListOrders_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Context, "Выбран заказ №" + e.Position.ToString(), ToastLength.Long).Show();
        }

        //private void EtSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        //{
        //    if (editEnterOrder.Text != "")
        //    {
        //        List<OrderBookModel> lstSearch = new List<OrderBookModel>();
        //        foreach (var item in orderlist)
        //        {
        //            if (item.txtFrom.StartsWith(editEnterOrder.Text))
        //            {
        //                lstSearch.Add(item);
        //            }
        //        }
        //        CustomListAdapter adapter = new CustomListAdapter(Context, lstSearch);
        //        lstOrder.Adapter = adapter;
        //    }
        //    else
        //    {
        //        UpdateList();
        //    }
        //}

        //public bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.mymneu, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}
        public override void OnResume()
        {
            UpdateList();
            base.OnResume();
        }

        public void UpdateList()
        {
            CustomListAdapter adapter = new CustomListAdapter(Context, orderlist);
            lstOrder.Adapter = adapter;
        }
    }
}