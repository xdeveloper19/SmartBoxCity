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
using Entity.Model;
using Entity.Model.BoxResponse;
using Entity.Model.OrderResponse;
using EntityLibrary.Model.OrderResponse;
using WebService.Client;

namespace SmartBoxCity.Activity.Order
{
    public class ManageOrderActivity: Fragment
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
        public ManageOrderActivity(string id)
        {
            this.id = id;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_order_management, container, false);

            Id = view.FindViewById<TextView>(Resource.Id.OrderManagementTextIdValue);
            Weight = view.FindViewById<TextView>(Resource.Id.OrderManagementTexWeight);
            Temperature = view.FindViewById<TextView>(Resource.Id.OrderManagementTextTemperature);
            Battery = view.FindViewById<TextView>(Resource.Id.OrderManagementTexBattery);
            Illumination = view.FindViewById<TextView>(Resource.Id.OrderManagementTextIllumination);
            Humidity = view.FindViewById<TextView>(Resource.Id.OrderManagementTextHumidity);
            Gate = view.FindViewById<TextView>(Resource.Id.OrderManagementTextState);
            Lock = view.FindViewById<TextView>(Resource.Id.OrderManagementTextCastle);
            Fold = view.FindViewById<TextView>(Resource.Id.OrderManagementTextRoleta);
            Events = view.FindViewById<TextView>(Resource.Id.OrderManagementTextEvents);

            GetOrderParameters();

            return view;
        }

        private async void GetOrderParameters()
        {
            var o_data = new ServiceResponseObject<OrderObjectResponse<OrderParameters, SensorResponse, StageResponse>>();
            o_data = await OrderService.GetSensorParameters(id);

            if (o_data.Status == HttpStatusCode.OK)
            {
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                Id.Text = (o_data.ResponseData.ORDER.id == null) ? "неизвестно" : o_data.ResponseData.ORDER.id;
                Weight.Text = (o_data.ResponseData.SENSORS_STATUS.weight == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.weight;
                Temperature.Text = (o_data.ResponseData.SENSORS_STATUS.temperature == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.temperature;
                Battery.Text = (o_data.ResponseData.SENSORS_STATUS.battery == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.battery;
                Illumination.Text = (o_data.ResponseData.SENSORS_STATUS.illumination == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.illumination;
                Humidity.Text = (o_data.ResponseData.SENSORS_STATUS.humidity == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.humidity;
                Gate.Text = (o_data.ResponseData.SENSORS_STATUS.gate == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.gate;
                Lock.Text = (o_data.ResponseData.SENSORS_STATUS.Lock == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.Lock;
                Fold.Text = (o_data.ResponseData.SENSORS_STATUS.fold == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.fold;
                Events.Text = (o_data.ResponseData.ORDER.event_count == null) ? "неизвестно" : o_data.ResponseData.ORDER.event_count;
                //Weight.Text = o_data.ResponseData.SENSORS_STATUS.Weight;
                //Temperature.Text = o_data.ResponseData.SENSORS_STATUS.Temperature;
                //Battery.Text = o_data.ResponseData.SENSORS_STATUS.Battery;
                //Illumination.Text = o_data.ResponseData.SENSORS_STATUS.Illumination;
                //Humidity.Text = o_data.ResponseData.SENSORS_STATUS.Humidity;
                //Gate.Text = o_data.ResponseData.SENSORS_STATUS.Gate;
                //Lock.Text = o_data.ResponseData.SENSORS_STATUS.Lock;
                //Fold.Text = o_data.ResponseData.SENSORS_STATUS.Fold;
                //Events.Text = o_data.ResponseData.ORDER.event_count;
            }
            else
            {
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

            }
        }
    }
}