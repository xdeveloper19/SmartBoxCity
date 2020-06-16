using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model.OrderResponse;
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Activity.Registration;
using SmartBoxCity.Service;
using WebService;
using WebService.Client;

namespace SmartBoxCity.Activity.Order
{
    class ActivityOrderPreis : Fragment
    {
        private TextView txt_From;
        private TextView txt_To;
        private TextView txt_Distance;
        private TextView txt_Insurance_Price;
        private TextView txt_Shipping_Cost;
        private Button btn_add_order;
        private Button btn_add_order_again;
        private ProgressBar preloader;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_order_price, container, false);
            preloader = view.FindViewById<ProgressBar>(Resource.Id.loader);
            txt_From = view.FindViewById<TextView>(Resource.Id.OrderPriceTextFrom);
            txt_To = view.FindViewById<TextView>(Resource.Id.OrderPriceTextTo);
            txt_Distance = view.FindViewById<TextView>(Resource.Id.OrderPriceTextDistance);
            txt_Insurance_Price = view.FindViewById<TextView>(Resource.Id.OrderPriceTextInsurancePrice);
            txt_Shipping_Cost = view.FindViewById<TextView>(Resource.Id.OrderPriceTextShippingCost);
            btn_add_order = view.FindViewById<Button>(Resource.Id.OrderPriceBtnAddOrder);
            btn_add_order_again = view.FindViewById<Button>(Resource.Id.OrderPriceBtnAddOrderAgain);

            btn_add_order_again.Click += delegate
            {

            };

            btn_add_order.Click += async delegate
            {
                preloader.Visibility = Android.Views.ViewStates.Visible;
                MakeOrderModel model = new MakeOrderModel()
                {
                    destination_address = StaticOrder.Destination_address,
                    for_date = StaticOrder.For_date,
                    for_time = StaticOrder.For_time,
                    height = StaticOrder.Height,
                    inception_address = StaticOrder.Inception_address,
                    cargo_class = StaticOrder.Cargo_class,
                    cargo_loading = StaticOrder.Cargo_loading,
                    cargo_type = StaticOrder.Cargo_type,
                    destination_lat = StaticOrder.Destination_lat,
                    destination_lng = StaticOrder.Destination_lng,
                    inception_lat = StaticOrder.Inception_lat,
                    inception_lng = StaticOrder.Inception_lng,
                    insurance = StaticOrder.Insurance,
                    receiver = StaticOrder.Receiver,
                    length = StaticOrder.Length,
                    qty = StaticOrder.Qty,
                    weight = StaticOrder.Weight,
                    width = StaticOrder.Width
                };

                if (CrossSettings.Current.GetValueOrDefault("isAuth", "") == "true")
                {
                    using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                    {
                        OrderService.InitializeClient(client);
                        var o_data = await OrderService.AddOrder(model);

                        if (o_data.Status == HttpStatusCode.OK)
                        {
                            OrderSuccessResponse o_user_data = new OrderSuccessResponse();
                            o_user_data = o_data.ResponseData;

                            preloader.Visibility = Android.Views.ViewStates.Invisible;
                            StaticOrder.Order_id = o_user_data.order_id;
                            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                        }
                        else
                        {
                            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                        }
                        CrossSettings.Current.AddOrUpdateValue("isOrdered", "true");
                    };
                    Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    UserActivity content = new UserActivity();
                    transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                }
                else
                {
                    Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Внимание!");
                    alert.SetMessage("Для оформления заказа необходимо войти или зарегистрироваться.");
                    alert.SetPositiveButton("Регистрация", (senderAlert, args) =>
                    {
                        alert.Dispose();
                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                        alert1.SetTitle("Внимание!");
                        alert1.SetMessage("Необходимо выбрать вид регистрации.");
                        alert1.SetPositiveButton("Для физ.лица", (senderAlert1, args1) =>
                        {
                            CrossSettings.Current.AddOrUpdateValue("isOrdered", "true");
                            Activity_Registration_Individual_Person content4 = new Activity_Registration_Individual_Person();
                            transaction2.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
                        });
                        alert1.SetNegativeButton("Для юр.лица", (senderAlert1, args1) =>
                        {
                            CrossSettings.Current.AddOrUpdateValue("isOrdered", "true");
                            Activity_Legal_Entity_Registration content3 = new Activity_Legal_Entity_Registration();
                            transaction2.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();
                    });
                    alert.SetNegativeButton("Вход", (senderAlert, args) =>
                    {
                        CrossSettings.Current.AddOrUpdateValue("isOrdered", "true");
                        AuthActivity content3 = new AuthActivity();
                        transaction2.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            };
            txt_Shipping_Cost.Text = StaticOrder.Amount;
            txt_Insurance_Price.Text = StaticOrder.Insurance_amount + " ₽";
            txt_Distance.Text = StaticOrder.Distance + " км";
            txt_To.Text = StaticOrder.Destination_address;
            txt_From.Text = StaticOrder.Inception_address;
            return view;
        }
    }
}