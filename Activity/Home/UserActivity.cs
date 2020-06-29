using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Service;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using Entity.Repository;
using Entity.Model.BoxResponse;
using Entity.Model;
using WebService.Client;
using Entity.Model.OrderResponse;
using WebService;
using EntityLibrary.Model.OrderResponse;

namespace SmartBoxCity.Activity.Home
{
    public class UserActivity : Fragment
    {
        private ListView lstOrder;

        public static List<OrderAdapter> orderlist;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;

            if (StaticUser.NeedToCreateOrder == true)
            {
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

                AddOrder(model);
            }

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var view = inflater.Inflate(Resource.Layout.activity_user, container, false);/// ошибка при нажати на кнопку "назад" на лефоне(Binary XML file line #1: Binary XML file line #1: Error inflating class fragment' )
                lstOrder = view.FindViewById<ListView>(Resource.Id.CurrentOrderListView);
                GetOrders();                
                string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);               

                return view;
            }
            catch (Exception ex)
            {
                var view = inflater.Inflate(Resource.Layout.activity_errors_handling, container, false);
                var TextOfError = view.FindViewById<TextView>(Resource.Id.TextOfError);
                TextOfError.Text += "\n(Ошибка: " + ex.Message + ")";
                return view;                
            }
            
        }
               

        private async void GetOrders()
        {
            var o_data = new ServiceResponseObject<ListResponse<OrderResponseData, ArchiveResponse>>();
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                OrderService.InitializeClient(client);
                o_data = await OrderService.GetOrders();

                if (o_data.Status == HttpStatusCode.OK && o_data.ResponseData.ORDERS.Count != 0)
                {
                    var number = 0;

                    orderlist = new List<OrderAdapter>();
                    foreach (var order in o_data.ResponseData.ORDERS)
                    {
                        number++;
                        orderlist.Add(new OrderAdapter
                        {
                            id = order.id,
                            Id = number,
                            inception_address = order.inception_address,
                            inception_lat = order.inception_lat,
                            cargo_class = order.cargo_class,
                            distance = order.distance,
                            insurance = order.insurance,
                            stage2_datetime = order.stage2_datetime,
                            stage5_datetime = order.stage5_datetime,
                            payment_id = order.payment_id,
                            order_stage_id = order.order_stage_id,
                            created_at = order.created_at,
                            payment_amount = order.payment_amount,
                            payment_status = order.payment_status,
                            order_stage_name = order.order_stage_name,
                            last_stage_at = order.last_stage_at,
                            container_id = order.container_id,
                            sensors_status = order.sensors_status,
                            event_count = order.event_count,
                        }
                        );
                    }
                    UpdateList();
                }
                else
                {
                    Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                    NotFoundOrdersActivity content = new NotFoundOrdersActivity();
                    transaction.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                }
            }
        }


        private void UpdateList()
        {
            AdapterUserActivity adapter = new AdapterUserActivity(Activity, orderlist, this.FragmentManager);
            lstOrder.Adapter = adapter;
        }

        public override void OnDestroyView()
        {
            StaticUser.IsUserOrMapActivity = false;
            base.OnDestroyView();
        }

        public override void OnStart()
        {
            StaticUser.IsUserOrMapActivity = true;
            base.OnStart();
        }
        public async void AddOrder(MakeOrderModel model)
        {
            var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", ""));
            OrderService.InitializeClient(client);
            var o_data = await OrderService.AddOrder(model);

            if (o_data.Status == HttpStatusCode.OK)
            {
                OrderSuccessResponse o_user_data = new OrderSuccessResponse();
                o_user_data = o_data.ResponseData;
                StaticOrder.Order_id = o_user_data.order_id;
                string Messag = "Заявка на оформление заказа успешно отправлена !";
                AlertDialogCall(Messag);
            }
            else
            {
                string ErrorMessag = "Не получилось оформить заказ.\nПричина: " + o_data.Message +
                    "\nДля повторного оформления заказа зайдите в раздел 'Заказать'.";
                AlertDialogCall(ErrorMessag);
            }
            StaticUser.NeedToCreateOrder = false;
            StaticUser.OrderInStageOfBid = false;

        }
        private void AlertDialogCall(string Messag)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
            alert.SetTitle("Внимание!");
            alert.SetMessage(Messag);
            alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
            {
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
        //private async void GetSensorParameters()
        //{
        //    var o_data = new ServiceResponseObject<SensorResponse>();
        //    using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
        //    {
        //        //надо было сначала клиента указать, а потом вызывать метод
        //        //и обязательно с токеном
        //        OrderService.InitializeClient(client);
        //        o_data = await OrderService.GetSensorParameters();

        //        if (o_data.Status == HttpStatusCode.OK)
        //        {
        //            //o_data.Message = "Успешно авторизован!";
        //            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

        //        }
        //        else
        //        {
        //            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

        //        }
        //    }
        //}

        //private void Btn_Show_State_Sensors(object sender, EventArgs e)
        //{
        //    //GetSensorParameters();
        //    //    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
        //    //    View view = layoutInflater.Inflate(Resource.Layout.activity_create_task, null);
        //    //    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
        //    //    alert.SetView(view);
        //    //    #region Объявление переменных в диалоговом окне
        //    //    var EditName = view.FindViewById<EditText>(Resource.Id.EditCreateTaskName);
        //    //    var EditTime = view.FindViewById<EditText>(Resource.Id.EditCreateTaskTime);
        //    //    var EditNote = view.FindViewById<EditText>(Resource.Id.EditCreateTaskNote);
        //    //    var RatingImportance = view.FindViewById<RatingBar>(Resource.Id.RatingCreateTaskImportance);
        //    //    var CheckBoxkReminder = view.FindViewById<CheckBox>(Resource.Id.CheckBoxCreateTaskReminder);
        //    //    #endregion

        //    //    alert.SetCancelable(false)
        //    //    .SetPositiveButton("Создать", delegate
        //    //    {
        //    //    })
        //    //    .SetNegativeButton("Отмена", delegate
        //    //    {
        //    //        alert.Dispose();
        //    //    });
        //    //    Dialog dialog = alert.Create();
        //    //    dialog.Show();
        //}

    }
}