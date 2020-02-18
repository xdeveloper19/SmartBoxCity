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
    public class ListOrdersActivity : Fragment
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
                Destination = "улица Шеболдаева, 24А, Ростов-на-Дону",
                Inception = "улица Кошевого, 1, Новочеркасск",
                Price = "650 руб",
                OrderName = "Заказ SO4386943088",
                Date = "9 февраля 16:34"
            };
            OrderBookModel p2 = new OrderBookModel()
            {
                Id = 2,
                Destination = "Славный переулок, 5, Новошахтинск",
                Inception = "Астаховский переулок, 84, Каменск-Шахтинский",
                Price = "950 руб",
                OrderName = "Заказ OP5887450402",
                Date = "12 марта 11:34"
            };
            OrderBookModel p3 = new OrderBookModel()
            {
                Id = 3,
                Destination = "Комитетская улица, 88, Новочеркасск",
                Inception = "переулок Чапаева, 2, Шахты",
                Price = "800 руб",
                OrderName = "Заказ PR3921079101",
                Date = "19 февраля 09:11"
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