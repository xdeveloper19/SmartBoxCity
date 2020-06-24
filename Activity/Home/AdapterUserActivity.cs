using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Service;
using WebService;
using WebService.Client;

namespace SmartBoxCity.Activity.Home
{
    public class AdapterUserActivity : BaseAdapter<OrderAdapter>
    {
        Context context;
        List<OrderAdapter> orders;
        Android.App.FragmentTransaction manager;
        private TextView Cost;
        private TextView NameContainer;
        private TextView Statusview;
        private TextView Payment;
        private const string URL = "https://smartboxcity.ru/";


        public AdapterUserActivity(Context Context, List<OrderAdapter> List, FragmentManager Manager)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.orders = List;
        }
        public override OrderAdapter this[int position] => orders[position];

        public override int Count => orders.Count;

        public override long GetItemId(int position)
        {
            return orders[position].Id;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return orders[position];
        }




        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(context).Inflate(Resource.Layout.activity_user_CardView, null);

            NameContainer = view.FindViewById<TextView>(Resource.Id.container_name);
            Statusview = view.FindViewById<TextView>(Resource.Id.status_view);
            Cost = view.FindViewById<TextView>(Resource.Id.s_cost);
            Payment = view.FindViewById<TextView>(Resource.Id.s_payment);
            Payment.Text = orders[position].payment_status;
            Cost.Text = orders[position].payment_amount;
            NameContainer.Text = orders[position].id;
            Statusview.Text = orders[position].order_stage_id + ". " + orders[position].order_stage_name;
            if (Payment.Text == "1")
            {
                Payment.Text = "Оплачено";
                Payment.SetTextColor(Color.ParseColor("#8EF892"));
            }
            else
            {
                Payment.Text = "Не оплачено";
                Payment.SetTextColor(Color.ParseColor("#EC8F9B"));
            }

            var btn_pay = view.FindViewById<Button>(Resource.Id.btn_pay);
            var progress = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            var btn_pass_delivery_service = view.FindViewById<Button>(Resource.Id.btn_pass_delivery_service);
            var btn_make_photo = view.FindViewById<Button>(Resource.Id.btn_make_photo);
            var btn_make_video = view.FindViewById<Button>(Resource.Id.btn_make_video);
            var btn_order_management = view.FindViewById<Button>(Resource.Id.btn_order_management);

            btn_pass_delivery_service.Click += delegate
            {
                try
                {
                    MainOrderStatusActivity content = new MainOrderStatusActivity();
                    StaticOrder.Order_id = orders[position].id;
                    StaticOrder.Payment_Amount = orders[position].payment_amount;
                    StaticOrder.Payment_Status = orders[position].payment_status;
                    StaticOrder.Event_Count = orders[position].event_count;
                    manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                }
            };

            btn_pay.Enabled = (orders[position].order_stage_id == "5") ? true : false;

            btn_pay.Click += delegate
            {
                if (Payment.Text == "неизвестно")
                {
                    Toast.MakeText(context, "В настоящий момент невозможно использовать эту кнопку!\nПричина: Неизвестно состояние об оплате.", ToastLength.Long).Show();
                }
                else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(context);
                    alert.SetTitle("Внесение оплаты");
                    alert.SetMessage("Вы действительно хотите оплатить заказ?");
                    alert.SetPositiveButton("Продолжить", (senderAlert, args) =>
                    {
                        MakePayment(alert);
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            };

            btn_make_photo.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(context);
                alert.SetTitle("Сделать фотографию");
                alert.SetMessage("Вы действительно хотите сделать фотографию с камеры контейнера?");
                alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                {
                    GetPhoto(orders[position].id, alert);
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };
            btn_make_video.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(context);
                alert.SetTitle("Сделать видео");
                alert.SetMessage("Вы действительно хотите сделать видео с камеры контейнера?");
                alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                {
                    GetVideo(orders[position].id, alert);
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };
            btn_order_management.Click += delegate
            {
                try
                {
                    ManageOrderActivity content = new ManageOrderActivity();
                    StaticOrder.Order_id = orders[position].id;
                    StaticOrder.Payment_Amount = orders[position].payment_amount;
                    StaticOrder.Payment_Status = orders[position].payment_status;
                    StaticOrder.Event_Count = orders[position].event_count;
                    manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                }

            };

            int order_stage;
            var result = int.TryParse(orders[position].order_stage_id, out order_stage);

            if (result == true)
                progress.Progress = order_stage;
            else
                progress.Progress = 0;
            //btn.Click += async delegate
            //{
            //    OrderActivity content = new OrderActivity();
            //    manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
            //};

            return view;
        }
        private async void GetVideo(string id, AlertDialog.Builder alert)
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.GetVideo(id);
                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        alert.Dispose();

                        LayoutInflater layoutInflater = LayoutInflater.From(context);
                        View view = layoutInflater.Inflate(Resource.Layout.modal_video, null);
                        var img_get_video = view.FindViewById<VideoView>(Resource.Id.img_get_video);

                        var src = Android.Net.Uri.Parse(URL + o_data.Message);
                        img_get_video.SetVideoURI(src);
                        img_get_video.Start();

                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(context);
                        alert1.SetTitle("Сделать видео");
                        alert1.SetView(view);
                        alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                        {
                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();
                    }
                    else
                    {
                        Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }            
        }

        private async void GetPhoto(string id, AlertDialog.Builder alert)
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.GetPhoto(id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        alert.Dispose();

                        LayoutInflater layoutInflater = LayoutInflater.From(context);
                        View view = layoutInflater.Inflate(Resource.Layout.modal_photo, null);
                        var img_get_photo = view.FindViewById<ImageView>(Resource.Id.img_get_photo);

                        var src = Android.Net.Uri.Parse(URL + o_data.Message);
                        img_get_photo.SetImageURI(src);

                        var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.Message);
                        img_get_photo.SetImageBitmap(imageBitmap);

                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(context);
                        alert1.SetView(view);
                        ////
                        alert1.SetCancelable(false);
                        alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                        {
                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();
                    }
                    else
                    {
                        Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }         
        }

        private async void MakePayment(AlertDialog.Builder alert)
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.MakePayment(StaticOrder.Order_id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        alert.Dispose();
                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(context);
                        alert1.SetTitle("Внесение оплаты");
                        alert1.SetMessage(o_data.ResponseData.Message);
                        alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                        {
                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();


                        UserActivity content = new UserActivity();
                        manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                    }
                    else
                    {
                        Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }          
        }       
    }
}