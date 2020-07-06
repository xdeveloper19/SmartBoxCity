using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.BoxResponse;
using Entity.Model.OrderResponse;
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using Entity.Repository;
using EntityLibrary.Model.OrderResponse;
using Plugin.Settings;
using SmartBoxCity.Service;
using WebService;
using WebService.Client;

namespace SmartBoxCity.Activity.Order
{
    public class ListOrdersActivity : Fragment
    {
        private ListView lstOrder;
        public static List<OrderBookModel> orderlist;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view;
                if (StaticUser.PresenceOnPage != true)
                {
                    view = inflater.Inflate(Resource.Layout.activity_not_found_book, container, false);
                    var btn_add_order1 = view.FindViewById<Button>(Resource.Id.btn_add_order1);
                    btn_add_order1.Click += delegate
                    {
                        try
                        {
                            Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                            AddOrderActivity content = new AddOrderActivity();
                            transaction.Replace(Resource.Id.framelayout, content).AddToBackStack(null);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                        }
                        
                    };
                    return view;
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.activity_order_book, container, false);
                    lstOrder = view.FindViewById<ListView>(Resource.Id.orderlistview);
                    orderlist = new List<OrderBookModel>();
                    //начинай тестиьть

                    GetOrders();

                    //editEnterOrder.TextChanged += EtSearch_TextChanged;

                    //int i = 0;
                    //while(i<1)
                    //{
                    //    OrderResponse order = new OrderResponse();
                    //    order = o_date.Rusult;
                    //}
                    //AuthResponseData o_user_data = new AuthResponseData();
                    //o_user_data = o_data.ResponseData;

                    return view;
                }
            }
            catch (Exception ex)
            {
                var view = inflater.Inflate(Resource.Layout.activity_errors_handling, container, false);
                var TextOfError = view.FindViewById<TextView>(Resource.Id.TextOfError);
                TextOfError.Text += "\n(Ошибка: " + ex.Message + ")";
                return view;
            }

        }

        //не отображается архив заказов, проверить!
        private async void GetOrders()
        {
            //var o_data1 = new ServiceResponseObject<SensorResponse>();
            //using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            //{
            //    //надо было сначала клиента указать, а потом вызывать метод
            //    //и обязательно с токеном
            //    OrderService.InitializeClient(client);
            //    o_data1 = await OrderService.GetSensorParameters();

            

            //}

            var o_data = new ServiceResponseObject<ListResponse<OrderResponseData, ArchiveResponse>>();
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                //надо было сначала клиента указать, а потом вызывать метод
                //и обязательно с токеном
                OrderService.InitializeClient(client);
                o_data = await OrderService.GetOrders();

                if (o_data.Status == HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    //StaticUser.Email = s_login.Text;
                    //StaticUser.AddInfoAuth(o_user_data);
                    if (o_data.ResponseData.ARCHIVE.Count == 0)
                    {
                        try
                        {
                            Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                            BookNotFoundActivity content = new BookNotFoundActivity();
                            transaction.Replace(Resource.Id.framelayout, content);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                        }
                        return;
                    }

                    //обязательно должен быть прогресс бар при обращении к серверу, типо такого
                    //preloader.Visibility = Android.Views.ViewStates.Invisible;
                    foreach (var order in o_data.ResponseData.ARCHIVE)
                    {
                        orderlist.Add(new OrderBookModel
                        {
                            Id = order.id,
                            Destination = order.destination_address,
                            Inception = order.inception_address,
                            Price = order.payment_amount + " рублей",
                            OrderName = order.id,
                            Date = order.stage2_datetime.ToString()
                        }
                        );
                    }



                    //OrderBookModel p2 = new OrderBookModel()
                    //{
                    //    Id = 2,
                    //    Destination = "Славный переулок, 5, Новошахтинск",
                    //    Inception = "Астаховский переулок, 84, Каменск-Шахтинский",
                    //    Price = "950 руб",
                    //    OrderName = "Заказ OP5887450402",
                    //    Date = "12 марта 11:34"
                    //};
                    //OrderBookModel p3 = new OrderBookModel()
                    //{
                    //    Id = 3,
                    //    Destination = "Комитетская улица, 88, Новочеркасск",
                    //    Inception = "переулок Чапаева, 2, Шахты",
                    //    Price = "800 руб",
                    //    OrderName = "Заказ PR3921079101",
                    //    Date = "19 февраля 09:11"
                    //};
                    //orderlist.Add(p1);
                    //orderlist.Add(p2);
                    //orderlist.Add(p3);
                    UpdateList();
                    lstOrder.ItemClick += ListOrders_ItemClick;

                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."

                }
            }
        }

        private void ListOrders_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Activity, "Выбран заказ №" + e.Position.ToString(), ToastLength.Long).Show();
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
        //        CustomListAdapter adapter = new CustomListAdapter(Activity, lstSearch);
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
            // UpdateList();
            base.OnResume();
        }

        public void UpdateList()
        {
            CustomListAdapter adapter = new CustomListAdapter(Activity, orderlist, this.FragmentManager);
            lstOrder.Adapter = adapter;
        }

    }
}