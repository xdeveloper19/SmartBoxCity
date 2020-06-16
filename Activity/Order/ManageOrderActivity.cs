using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
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
    public enum Stages: int
    {
        Request = 1,
        WaitLoading,
        Loading,
        Transportation,
        WaitUnloading,
        Unloading,
        Finish,
        Completed
    }
    public class ManageOrderActivity: Fragment
    {
        #region Объявление переменных
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

        private Spinner Date;
        private List<string> Time;
        private CheckBox checkBox;
        private bool check;

        private ProgressBar progressBar;
        private TextView Status;
        private TextView Cost;
        private TextView Payment;
        private Button btn_Pay;
        private Button btn_Photo;
        private Button btn_Video;
        private Button btn_Lock;
        private const string URL = "https://smartboxcity.ru/";
        private string Date_str;
        private string ErrorHandling;
        #endregion
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            GetOrderParameters();
            if (ErrorHandling == "error")
            {
                var view = inflater.Inflate(Resource.Layout.activity_errors_handling, container, false);

                return view;
            }
            else
            {
                var view = inflater.Inflate(Resource.Layout.activity_order_management, container, false);
                #region Иннициализация переменных
                Id = view.FindViewById<TextView>(Resource.Id.OrderManagementTextIdValue);
                Weight = view.FindViewById<TextView>(Resource.Id.OrderManagementTexWeight);
                Temperature = view.FindViewById<TextView>(Resource.Id.OrderManagementTextTemperature);
                Battery = view.FindViewById<TextView>(Resource.Id.OrderManagementTexBattery);
                Illumination = view.FindViewById<TextView>(Resource.Id.OrderManagementTextIllumination);
                Humidity = view.FindViewById<TextView>(Resource.Id.OrderManagementTextHumidity);
                Gate = view.FindViewById<TextView>(Resource.Id.OrderManagementTextGate);
                Lock = view.FindViewById<TextView>(Resource.Id.OrderManagementTextLock);
                Fold = view.FindViewById<TextView>(Resource.Id.OrderManagementTextFold);
                Events = view.FindViewById<TextView>(Resource.Id.OrderManagementTextEvents);
                progressBar = view.FindViewById<ProgressBar>(Resource.Id.OrderManagementProgressBar);
                Status = view.FindViewById<TextView>(Resource.Id.OrderManagementTextStatus);
                Cost = view.FindViewById<TextView>(Resource.Id.OrderManagementTextCost);
                Payment = view.FindViewById<TextView>(Resource.Id.OrderManagementTextPayment);
                btn_Pay = view.FindViewById<Button>(Resource.Id.OrderManagementButtonPay);
                btn_Photo = view.FindViewById<Button>(Resource.Id.OrderManagementButtonPhoto);
                btn_Video = view.FindViewById<Button>(Resource.Id.OrderManagementButtonVideo);
                btn_Lock = view.FindViewById<Button>(Resource.Id.OrderManagementButtonLock);
                #endregion


                btn_Lock.Click += delegate
                {

                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    if (btn_Lock.Text == "Открыть")
                    {
                        alert.SetTitle("Открытие замка");
                        alert.SetMessage("Вы действительно хотите открыть замок контейнера?");
                        alert.SetPositiveButton("Открыть", (senderAlert, args) =>
                        {
                            MakeUnLock(alert);
                        });
                        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        {
                        });
                    }
                    else if (btn_Lock.Text == "Закрыть")
                    {
                        alert.SetTitle("Закрытие замка");
                        if (StaticOrder.Order_Stage_Id == "3")
                        {
                            LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                            View dialogView = layoutInflater.Inflate(Resource.Layout.modal_transmit_order, null);
                            alert.SetView(dialogView);

                            checkBox = dialogView.FindViewById<CheckBox>(Resource.Id.ManageOrderCheckBox);

                            Date = dialogView.FindViewById<Spinner>(Resource.Id.ManageOrderSpinnerTime);
                            CreateTimeArray();
                            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, Time);
                            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                            Date.Adapter = adapter;
                            Date.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerClass_ItemSelected);
                            Date.Visibility = ViewStates.Invisible;

                            checkBox.Text = "Погрузка завершена. Контейнер готов к отправке.";
                            checkBox.Click += delegate
                            {
                                check = checkBox.Checked;
                                Date.Visibility = ViewStates.Visible;
                                Date.Focusable = false;
                                Date.Clickable = false;
                            };

                        }
                        else if (StaticOrder.Order_Stage_Id == "6")
                        {
                            LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                            View dialogView = layoutInflater.Inflate(Resource.Layout.modal_transmit_order, null);
                            alert.SetView(dialogView);

                            checkBox = dialogView.FindViewById<CheckBox>(Resource.Id.ManageOrderCheckBox);
                            Date = dialogView.FindViewById<Spinner>(Resource.Id.ManageOrderSpinnerTime);
                            CreateTimeArray();
                            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, Time);
                            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                            Date.Adapter = adapter;
                            Date.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerClass_ItemSelected);
                            Date.Visibility = ViewStates.Invisible;

                            checkBox.Text = "Разгрузка завершена. Контейнер готов к отправке.";
                            checkBox.Click += delegate
                            {
                                check = checkBox.Checked;
                                Date.Visibility = ViewStates.Visible;
                                Date.Focusable = false;
                                Date.Clickable = false;
                            };
                        }
                        alert.SetMessage("Вы действительно хотите закрыть замок контейнера?");
                        alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                        {
                            MakeLock(alert, check);
                            if (check == true)
                                Transmitt(alert);

                            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            ManageOrderActivity content2 = new ManageOrderActivity();
                            transaction1.Replace(Resource.Id.framelayout, content2).AddToBackStack(null).Commit();
                        });
                    }
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();


                    // }                
                };


                btn_Pay.Click += delegate
                {
                    if (Payment.Text == "неизвестно")
                    {
                        Toast.MakeText(Activity, "В настоящий момент невозможно использовать эту кнопку!\nПричина: Неизвестно состояние об оплате.", ToastLength.Long).Show();
                    }
                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                        alert.SetTitle("Внесение оплаты");
                        alert.SetMessage("Вы действительно хотите оплатить заказ?");
                        alert.SetPositiveButton("Продолжить", (senderAlert, args) =>
                        {
                            MakePayment(alert);

                            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            ManageOrderActivity content2 = new ManageOrderActivity();
                            transaction1.Replace(Resource.Id.framelayout, content2).AddToBackStack(null).Commit();
                        });
                        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        {
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();


                    }
                };

                btn_Photo.Click += delegate
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Сделать фотографию");
                    alert.SetMessage("Вы действительно хотите сделать фотографию с камеры контейнера?");
                    alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                    {
                        GetPhoto(alert);
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                };
                btn_Video.Click += delegate
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Сделать видео");
                    alert.SetMessage("Вы действительно хотите сделать видео с камеры контейнера?");
                    alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                    {
                        GetVideo(alert);
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                };

                return view;
            }
            
        }

        private void CreateTimeArray()
        {
            Time = new List<string>();
            DateTime nowTime = DateTime.Now;
            int[] a = nowTime.ToShortTimeString().Split(':').
              Where(x => !string.IsNullOrWhiteSpace(x)).
              Select(x => int.Parse(x)).ToArray();
            if (a[1] <= 30)
            {
                DateTime ModelTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, a[0], 00, 00);
                for (int i = 2; i < 10; i += 2)
                {
                    string t = ModelTime.ToShortDateString() + " " + ModelTime.AddHours(i - 2).ToShortTimeString() + " - ";
                    t += ModelTime.AddHours(i).ToShortTimeString();
                    Time.Add(t);
                }
            }
            else if (a[1] > 30)
            {
                a[0] += 1;
                DateTime ModelTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, a[0], 30, 00);
                for (int i = 2; i < 10; i += 2)
                {
                    string t = ModelTime.ToShortDateString() + " " + ModelTime.AddHours(i - 2).ToShortTimeString() + " - ";
                    t += ModelTime.AddHours(i).ToShortTimeString();
                    Time.Add(t);
                }
            }
        }

        private void SpinnerClass_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            Date_str = spinner.GetItemAtPosition(e.Position).ToString();
        }

        private async void MakeLock(AlertDialog.Builder alert, bool checkBox)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                ManageOrderService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await ManageOrderService.LockRollete(StaticOrder.Order_id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Закрытие замка");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {
                        if (checkBox == true)
                        {
                            //btn_Lock.Clickable = false;
                            //btn_Lock.Focusable = false;
                            //btn_Lock.LongClickable = false;
                            btn_Lock.Visibility = ViewStates.Gone;
                            Lock.Text = "Закрыт";
                        }
                        else
                        {
                            btn_Lock.Text = "Открыть";
                            Lock.Text = "Закрыт";
                        }
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();
                }


            }
        }

        private async void Transmitt(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                string[] date = Date_str.Split(' ', '-');
                var o_data = await ManageOrderService.TransmitOrder(StaticOrder.Order_id, date[0], date[1]);
                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Закрытие замка");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();

                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }


            }

        }

        private async void MakeUnLock(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                ManageOrderService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await ManageOrderService.UnLockRollete(StaticOrder.Order_id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Открытие замка");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {
                        btn_Lock.Text = "Закрыть";
                        Lock.Text = "Открыт";
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }

        }

        private async void MakePayment(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                ManageOrderService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await ManageOrderService.MakePayment(StaticOrder.Order_id);
                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Внесение оплаты");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }
        }
        private async void GetVideo(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                ManageOrderService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await ManageOrderService.GetVideo(StaticOrder.Order_id);
                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();

                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.modal_video, null);
                    var img_get_video = view.FindViewById<VideoView>(Resource.Id.img_get_video);

                    var src = Android.Net.Uri.Parse(URL + o_data.ResponseData.Message);
                    img_get_video.SetVideoURI(src);
                    img_get_video.Start();

                    //var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.ResponseData.Message);
                    //img_get_video.SetVideoURI(imageBitmap);

                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
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
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }
        }

        private async void GetPhoto(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                ManageOrderService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await ManageOrderService.GetPhoto(StaticOrder.Order_id);
                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();

                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.modal_photo, null);
                    var img_get_photo = view.FindViewById<ImageView>(Resource.Id.img_get_photo);

                    var src = Android.Net.Uri.Parse(URL + o_data.ResponseData.Message);
                    img_get_photo.SetImageURI(src);

                    var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.ResponseData.Message);
                    img_get_photo.SetImageBitmap(imageBitmap);

                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
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
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }
        }


        private async void GetOrderParameters()
        {
            var o_data = new ServiceResponseObject<OrderObjectResponse<OrderParameters, SensorResponse, StageResponse>>();
            o_data = await OrderService.GetSensorParameters(StaticOrder.Order_id);

            if (o_data.Status == HttpStatusCode.OK)
            {
                ErrorHandling = "ok";
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

                StaticBox.AddInfoSensors(o_data.ResponseData.SENSORS_STATUS);
                StaticOrder.AddInfoOrder(o_data.ResponseData.ORDER);

                Id.Text = (o_data.ResponseData.ORDER.id == null) ? "неизвестно" : o_data.ResponseData.ORDER.id;
                Weight.Text = (o_data.ResponseData.SENSORS_STATUS.weight == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.weight;
                Temperature.Text = (o_data.ResponseData.SENSORS_STATUS.temperature == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.temperature;
                Battery.Text = (o_data.ResponseData.SENSORS_STATUS.battery == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.battery;
                Illumination.Text = (o_data.ResponseData.SENSORS_STATUS.illumination == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.illumination;
                Humidity.Text = (o_data.ResponseData.SENSORS_STATUS.humidity == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.humidity;
                Events.Text = (o_data.ResponseData.ORDER.event_count == null) ? "неизвестно" : StaticOrder.Event_Count + " шт.";
                progressBar.Progress = (o_data.ResponseData.ORDER.order_stage_id == null) ? 0 : Convert.ToInt32(o_data.ResponseData.ORDER.order_stage_id);
                if (progressBar.Progress != 0)
                {
                    Status.Text = progressBar.Progress.ToString() + ". " + o_data.ResponseData.STAGES[progressBar.Progress - 1].name;
                }
                else
                {
                    Status.Text = "неизвестно";
                }
                Cost.Text = (o_data.ResponseData.ORDER.payment_amount == null) ? "неизвестно" : StaticOrder.Payment_Amount + " руб.";
                if (o_data.ResponseData.ORDER.payment_status == "1")
                {
                    Payment.Text = "Оплачено";
                    Payment.SetTextColor(Color.ParseColor("#8EF892"));
                }
                else if (o_data.ResponseData.ORDER.payment_status == "0")
                {
                    Payment.Text = "Не оплачено";
                    Payment.SetTextColor(Color.ParseColor("#EC8F9B"));
                }
                else
                {
                    Payment.Text = "неизвестно";
                }

                if (o_data.ResponseData.SENSORS_STATUS.Lock == "1")
                {
                    Lock.Text = "Закрыт";
                    btn_Lock.Text = "Открыть";
                }
                else if (o_data.ResponseData.SENSORS_STATUS.Lock == "0")
                {
                    Lock.Text = "Открыт";
                    btn_Lock.Text = "Закрыть";
                }
                else
                {
                    Lock.Text = "Неизвестно";
                    btn_Lock.Text = "Открыть";
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

                btn_Lock.Enabled = (StaticOrder.Order_Stage_Id == "1" ||
                StaticOrder.Order_Stage_Id == "4" ||
                StaticOrder.Order_Stage_Id == "7" ||
                StaticOrder.Order_Stage_Id == "5" ||
                StaticOrder.Order_Stage_Id == "8") ? false : true;

                btn_Pay.Enabled = (StaticOrder.Order_Stage_Id == "5") ? true : false;
            }
            else
            {
                ErrorHandling = "error";
            }
        }
    }
}