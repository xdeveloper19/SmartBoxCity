using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.BoxResponse;
using Entity.Model.OrderResponse;
using Entity.Model.TaskViewModel;
using Entity.Repository;
using EntityLibrary.Model.OrderResponse;
using Plugin.Settings;
using SmartBoxCity.Activity.Order;
using WebService;
using WebService.Client;
using WebService.Driver;

namespace SmartBoxCity.Activity.Driver
{
    public class ManageBoxActivity: Fragment
    {
        #region переменные
        private TextView Id;
        private TextView Weight;
        private TextView Temperature;
        private TextView Battery;
        private TextView Illumination;
        private TextView Humidity;
        private TextView Gate;
        private TextView Lock;
        private TextView Fold;
        private TextView Events;
        public string id;
        #endregion
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_box_management, container, false);

            Id = view.FindViewById<TextView>(Resource.Id.TaskManagementTextIdValue1);
            Weight = view.FindViewById<TextView>(Resource.Id.OrderManagementTexWeight1);
            Temperature = view.FindViewById<TextView>(Resource.Id.OrderManagementTextTemperature1);
            Battery = view.FindViewById<TextView>(Resource.Id.OrderManagementTexBattery1);
            Illumination = view.FindViewById<TextView>(Resource.Id.OrderManagementTextIllumination1);
            Humidity = view.FindViewById<TextView>(Resource.Id.OrderManagementTextHumidity1);
            Gate = view.FindViewById<TextView>(Resource.Id.OrderManagementTextState1);
            Lock = view.FindViewById<TextView>(Resource.Id.OrderManagementTextCastle1);
            Fold = view.FindViewById<TextView>(Resource.Id.OrderManagementTextRoleta1);
            Events = view.FindViewById<TextView>(Resource.Id.OrderManagementTextEvents1);
            var btn_order = view.FindViewById<Button>(Resource.Id.btn_about_order);

            btn_order.Click += delegate
            {
                try
                {
                    FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                    MainOrderStatusActivity content = new MainOrderStatusActivity();
                    transaction.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                }
            };

            var result = GetTasks();
            if (result.Result == TaskStatus.OK)
                GetOrderParameters();
            else if (result.Result == TaskStatus.OrderNotImplemented)
            {
                StaticUser.NamePadeAbsenceSomething = "OrderNotFoundForDriver";
                Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                NotFoundOrdersActivity content = new NotFoundOrdersActivity();
                transaction1.Replace(Resource.Id.frameDriverlayout, content);
                transaction1.Commit();
            }
            else
            {
                FragmentTransaction ft = this.FragmentManager.BeginTransaction();
                TaskNotFoundActivity act = new TaskNotFoundActivity();
                ft.Replace(Resource.Id.frameDriverlayout, act);
                ft.Commit();
            }

            return view;
        }

        private async void GetOrderParameters()
        {
            var o_data = new ServiceResponseObject<OrderObjectResponse<OrderParameters, SensorResponse, StageResponse>>();
            StaticOrder.Order_id = StaticTask.order_id;
            o_data = await OrderService.GetSensorParameters(StaticTask.order_id);

            if (o_data.Status == HttpStatusCode.OK)
            {
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                Id.Text = (o_data.ResponseData.ORDER.id == null) ? "неизвестно" : o_data.ResponseData.ORDER.id;
                Weight.Text = (o_data.ResponseData.SENSORS_STATUS.weight == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.weight;
                Temperature.Text = (o_data.ResponseData.SENSORS_STATUS.temperature == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.temperature;
                Battery.Text = (o_data.ResponseData.SENSORS_STATUS.battery == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.battery;
                Illumination.Text = (o_data.ResponseData.SENSORS_STATUS.illumination == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.illumination;
                Humidity.Text = (o_data.ResponseData.SENSORS_STATUS.humidity == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.humidity;
                //Gate.Text = (o_data.ResponseData.SENSORS_STATUS.gate == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.gate;
                //Lock.Text = (o_data.ResponseData.SENSORS_STATUS.Lock == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.Lock;
                //Fold.Text = (o_data.ResponseData.SENSORS_STATUS.fold == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.fold;
                Events.Text = (o_data.ResponseData.ORDER.event_count == null) ? "неизвестно" : o_data.ResponseData.ORDER.event_count;

                StaticOrder.AddInfoOrder(o_data.ResponseData.ORDER);
                StaticBox.AddInfoSensors(o_data.ResponseData.SENSORS_STATUS);

                if (o_data.ResponseData.SENSORS_STATUS.Lock == "1")
                {
                    Lock.Text = "Закрыт";
                }
                else if (o_data.ResponseData.SENSORS_STATUS.Lock == "0")
                {
                    Lock.Text = "Открыт";
                }
                else
                {
                    Lock.Text = "Неизвестно";
                }

                if (o_data.ResponseData.SENSORS_STATUS.fold == "1")
                {
                    Fold.Text = "Разложен";
                }
                else if (o_data.ResponseData.SENSORS_STATUS.fold == "0")
                {
                    Fold.Text = "Сложен";
                }
                else
                {
                    Fold.Text = "Неизвестно";
                }

                if (o_data.ResponseData.SENSORS_STATUS.gate == "1")
                {
                    Gate.Text = "Закрыта";
                }
                else if (o_data.ResponseData.SENSORS_STATUS.gate == "0")
                {
                    Gate.Text = "Открыта";
                }
                else
                {
                    Gate.Text = "Неизвестно";
                }
            }
            else
            {
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

            }
        }

        private async Task<TaskStatus> GetTasks()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                TaskService.InitializeClient(client);
                var o_data = await TaskService.GetTasks();

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    StaticDriver.AddInfoDriver(o_data.ResponseData.DRIVER);

                    if (o_data.ResponseData.TASKS.Count == 0)
                    {
                        return TaskStatus.NotFound;
                    }


                    var firstTask = o_data.ResponseData.TASKS.First();
                    StaticTask.AddInfoTask(firstTask);

                    var boxes_id = o_data.ResponseData.CONTAINERS.Select(s => s.id).ToList();
                    StaticTask.AddContainersID(boxes_id);

                    if (StaticTask.order_id == "")
                        return TaskStatus.OrderNotImplemented;

                    return TaskStatus.OK;
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."
                    return TaskStatus.ServerError;
                }


            }

        }

    }
}