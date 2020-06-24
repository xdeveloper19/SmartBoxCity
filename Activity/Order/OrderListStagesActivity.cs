using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.BoxResponse;
using Entity.Model.OrderResponse;
using Entity.Repository;
using EntityLibrary.Model.OrderResponse;
using WebService.Client;

namespace SmartBoxCity.Activity.Order
{
    public class OrderListStagesActivity: Fragment 
    {
        #region Объявление переменных
        private TextView Id;
        private TextView PaymentStatus;
        private TextView Cost;
        private TextView StageRequest;
        private TextView StageRequestValue;
        private TextView StageWaitTransport;
        private TextView StageWaitTransportValue;
        private TextView StageLading;
        private TextView StageLadingValue;
        private TextView StageShipping;
        private TextView StageShippingValue;
        private TextView StageWaitUnload;
        private TextView StageWaitUnloadValue;
        private TextView StageUnload;
        private TextView StageUnloadValue;
        private TextView StageCompletion;
        private TextView StageCompletionValue;
        private TextView StageEndCompletion;
        private TextView StageEndCompletionValue;
        #endregion
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var view = inflater.Inflate(Resource.Layout.activity_order_stages, container, false);
                #region Инициализация переменных
                Id = view.FindViewById<TextView>(Resource.Id.OrderStageTxtId);
                PaymentStatus = view.FindViewById<TextView>(Resource.Id.OrderStageTxtPaymentStatus);
                Cost = view.FindViewById<TextView>(Resource.Id.OrderStageTxtCost);
                StageRequest = view.FindViewById<TextView>(Resource.Id.StageRequest);
                StageRequestValue = view.FindViewById<TextView>(Resource.Id.StageRequestValue);
                StageWaitTransport = view.FindViewById<TextView>(Resource.Id.StageWaitTransport);
                StageWaitTransportValue = view.FindViewById<TextView>(Resource.Id.StageWaitTransportValue);
                StageLading = view.FindViewById<TextView>(Resource.Id.StageLading);
                StageLadingValue = view.FindViewById<TextView>(Resource.Id.StageLadingValue);
                StageShipping = view.FindViewById<TextView>(Resource.Id.StageShipping);
                StageShippingValue = view.FindViewById<TextView>(Resource.Id.StageShippingValue);
                StageWaitUnload = view.FindViewById<TextView>(Resource.Id.StageWaitUnload);
                StageWaitUnloadValue = view.FindViewById<TextView>(Resource.Id.StageWaitUnloadValue);
                StageUnload = view.FindViewById<TextView>(Resource.Id.StageUnload);
                StageUnloadValue = view.FindViewById<TextView>(Resource.Id.StageUnloadValue);
                StageCompletion = view.FindViewById<TextView>(Resource.Id.StageCompletion);
                StageCompletionValue = view.FindViewById<TextView>(Resource.Id.StageCompletionValue);
                StageEndCompletion = view.FindViewById<TextView>(Resource.Id.StageEndCompletion);
                StageEndCompletionValue = view.FindViewById<TextView>(Resource.Id.StageEndCompletionValue);
                #endregion

                GetParameters();

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

        private async void GetParameters()
        {
            //проверить статус оплаты
            var o_data = new ServiceResponseObject<OrderObjectResponse<OrderParameters, SensorResponse, StageResponse>>();
            o_data = await OrderService.GetSensorParameters(StaticOrder.Order_id);

            if (o_data.Status == HttpStatusCode.OK)
            {
                var order = o_data.ResponseData.ORDER;
                var stages = o_data.ResponseData.STAGES;
                Id.Text = order.id;

                if (order.payment_status == "1")
                {
                    PaymentStatus.Text = "Оплачено";
                    PaymentStatus.SetTextColor(Color.ParseColor("#8EF892"));
                }
                else if (order.payment_status == "0")
                {
                    PaymentStatus.Text = "Не оплачено";
                    PaymentStatus.SetTextColor(Color.ParseColor("#EC8F9B"));
                }
                Cost.Text = (order.payment_amount == null) ? "неизвестно" : order.payment_amount + " руб.";
                if (order.payment_id != "неизвестно" || order.payment_id != null)
                {
                    var id = Convert.ToInt32(order.order_stage_id);
                    switch (stages[id - 1].name)
                    {
                        case "заявка":
                            StageRequest.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageRequest.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                        case "ожидание погрузки":
                            BackgroundRequest();
                            StageWaitTransport.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageWaitTransportValue.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                        case "погрузка":
                            BackgroundRequest();
                            BackgroundWaitTransport();
                            StageLading.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageLadingValue.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                        case "перевозка":
                            BackgroundRequest();
                            BackgroundWaitTransport();
                            BackgroundLading();
                            StageShipping.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageShippingValue.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                        case "ожидание выгрузки":
                            BackgroundRequest();
                            BackgroundWaitTransport();
                            BackgroundLading();
                            BackgroundShipping();
                            StageWaitUnload.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageWaitUnloadValue.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                        case "выгрузка":
                            BackgroundRequest();
                            BackgroundWaitTransport();
                            BackgroundLading();
                            BackgroundShipping();
                            BackgrounWaitdUnload();
                            StageUnload.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageUnloadValue.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                        case "завершение":
                            BackgroundRequest();
                            BackgroundWaitTransport();
                            BackgroundLading();
                            BackgroundShipping();
                            BackgrounWaitdUnload();
                            BackgroundUnload();
                            StageCompletion.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageCompletionValue.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                        case "выполнен":
                            BackgroundRequest();
                            BackgroundWaitTransport();
                            BackgroundLading();
                            BackgroundShipping();
                            BackgrounWaitdUnload();
                            BackgroundUnload();
                            BackgroundCompletion();
                            StageEndCompletion.SetBackgroundResource(Resource.Drawable.stagestylecurrent);
                            StageEndCompletionValue.SetTextColor(Color.ParseColor("#9AAAFC"));
                            break;
                    }
                }
            }
            else
            {
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
            }
        }

        private void BackgroundCompletion()
        {
            StageCompletion.SetBackgroundResource(Resource.Drawable.stagestylepast);
            StageCompletionValue.SetTextColor(Color.ParseColor("#9FF38C"));
        }

        private void BackgroundUnload()
        {
            StageUnload.SetBackgroundResource(Resource.Drawable.stagestylepast);
            StageUnloadValue.SetTextColor(Color.ParseColor("#9FF38C"));
        }

        private void BackgrounWaitdUnload()
        {
            StageWaitUnload.SetBackgroundResource(Resource.Drawable.stagestylepast);
            StageWaitUnloadValue.SetTextColor(Color.ParseColor("#9FF38C"));
        }

        private void BackgroundShipping()
        {
            StageShipping.SetBackgroundResource(Resource.Drawable.stagestylepast);
            StageShippingValue.SetTextColor(Color.ParseColor("#9FF38C"));
        }

        private void BackgroundLading()
        {
            StageLading.SetBackgroundResource(Resource.Drawable.stagestylepast);
            StageLadingValue.SetTextColor(Color.ParseColor("#9FF38C"));
        }

        private void BackgroundWaitTransport()
        {
            StageWaitTransport.SetBackgroundResource(Resource.Drawable.stagestylepast);
            StageWaitTransportValue.SetTextColor(Color.ParseColor("#9FF38C"));
        }

        private void BackgroundRequest()
        {
            StageRequest.SetBackgroundResource(Resource.Drawable.stagestylepast);
            StageRequestValue.SetTextColor(Color.ParseColor("#9FF38C"));
        }
    }
}