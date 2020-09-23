using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.BoxResponse;
using Entity.Model.OrderResponse;
using Entity.Repository;
using EntityLibrary.Model.OrderResponse;
using Plugin.Settings;
using SmartBoxCity.Activity.Home;
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
    public class ManageOrderActivity : Fragment
    {
        #region Объявление переменных
        private TextView Id;
        private TextView Weight;
        private TextView Temperature;
        private TextView Battery;
        private TextView Illumination;
        private TextView Humidity;
        private TextView Lock;
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
        private string ErrorData;

        #endregion
        SwipeRefreshLayout sweep;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            GetOrderParameters();
            var view = inflater.Inflate(Resource.Layout.activity_order_management, container, false);
            #region Иннициализация переменных
            Id = view.FindViewById<TextView>(Resource.Id.OrderManagementTextIdValue);
            Weight = view.FindViewById<TextView>(Resource.Id.OrderManagementTexWeight);
            Temperature = view.FindViewById<TextView>(Resource.Id.OrderManagementTextTemperature);
            Battery = view.FindViewById<TextView>(Resource.Id.OrderManagementTexBattery);
            Illumination = view.FindViewById<TextView>(Resource.Id.OrderManagementTextIllumination);
            Humidity = view.FindViewById<TextView>(Resource.Id.OrderManagementTextHumidity);
            //Gate = view.FindViewById<TextView>(Resource.Id.OrderManagementTextGate);
            Lock = view.FindViewById<TextView>(Resource.Id.OrderManagementTextLock);
            //Fold = view.FindViewById<TextView>(Resource.Id.OrderManagementTextFold);
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

            Id.Click += Id_Click;
            Events.Click += Events_Click;

            sweep = view.FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefreshLayout);
            sweep.SetColorSchemeColors(Color.Red, Color.Green, Color.Blue, Color.Yellow);
            sweep.Refresh += RefreshLayout_Refresh;

            btn_Lock.Click += delegate
            {

                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                if (btn_Lock.Text == "Поднять")
                {
                    alert.SetTitle("Подтверждение действия");
                    alert.SetMessage("Вы действительно хотите поднять роллету контейнера?");
                    alert.SetPositiveButton("Поднять", (senderAlert, args) =>
                    {
                        MakeUnLock();
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                }
                else if (btn_Lock.Text == "Опустить")
                {
                    alert.SetTitle("Подтверждение действия");
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
                    alert.SetMessage("Вы действительно хотите опустить роллету контейнера?");
                    alert.SetPositiveButton("Опустить", (senderAlert, args) =>
                    {
                        MakeLock(check);
                        if (check == true)
                            Transmitt();

                        FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                        ManageOrderActivity content2 = new ManageOrderActivity();
                        transaction1.Replace(Resource.Id.framelayout, content2).AddToBackStack(null).Commit();
                    });
                }
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
                    AlertDialogCreation("Внесение оплаты", "Вы действительно хотите оплатить заказ?");
                }
            };
            btn_Photo.Click += delegate
            {
                AlertDialogCreation("Сделать фотографию", "Вы действительно хотите сделать фотографию с камеры контейнера?");
            };
            btn_Video.Click += delegate
            {
                AlertDialogCreation("Сделать видео", "Вы действительно хотите сделать видео с камеры контейнера?");
            };

            return view;
            
        }
        private void RefreshLayout_Refresh(object sender, EventArgs e)
        {
            //Data Refresh Place  
            BackgroundWorker work = new BackgroundWorker();
            work.DoWork += Work_DoWork;
            work.RunWorkerCompleted += Work_RunWorkerCompleted;
            work.RunWorkerAsync();
        }
        private void Work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sweep.Refreshing = false;
            ManageOrderActivity content = new ManageOrderActivity();
            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
            transaction1.Replace(Resource.Id.framelayout, content);
            transaction1.Commit();
        }
        private void Work_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }
        private void Events_Click(object sender, EventArgs e)
        {
            try
            {
                FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                EventsActivity content3 = new EventsActivity();
                transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null);
                transaction1.Commit();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }

        private void Id_Click(object sender, EventArgs e)
        {
            try
            {
                FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                MainOrderStatusActivity content2 = new MainOrderStatusActivity();
                transaction1.Replace(Resource.Id.framelayout, content2).AddToBackStack(null);
                transaction1.Commit();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }

        private void AlertDialogCreation(string titleString, string messageString)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(Activity);
            View view = layoutInflater.Inflate(Resource.Layout.qqqqqqww, null);

            var txtTitle = view.FindViewById<TextView>(Resource.Id.TextTitle);
            var txtInfo = view.FindViewById<TextView>(Resource.Id.TextInfo);
            var btn_Negative = view.FindViewById<Button>(Resource.Id.BtnNegative);
            var btn_Positive = view.FindViewById<Button>(Resource.Id.BtnPositive);
           // Thread thread = new Thread
           //(()
           //     =>{
           //        while (true)
           //        {
           //            if (StaticOrder.MessageResult == "1")
           //            {
           //                btn_Positive.Text = "Открыть";
           //                 return;
           //            }
           //        }
           //});

            txtTitle.Text = titleString;
            txtInfo.Text = messageString;            

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(Activity);            
            
            alert.SetView(view);
            alert.SetCancelable(false);
           
            Dialog dialog = alert.Create();
            btn_Positive.Click += delegate
            {
                if (btn_Positive.Text == "Открыть")
                {
                    if (StaticOrder.MessageResult == "1")
                    {
                        //thread.Abort();
                        //thread.Join();
                        dialog.Dismiss();
                        SetPhoto();
                    }
                    else
                        Toast.MakeText(Activity, "Фото ещё не загруженно.", ToastLength.Long).Show();
                }
                else
                {
                    switch (titleString)
                    {
                        case "Сделать фотографию":
                            btn_Positive.Text = "Открыть";
                            GetPhoto();
                            break;
                        case "Сделать видео":
                            Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                            VideoFromServerActivity content = new VideoFromServerActivity(StaticOrder.Order_id, "");
                            transaction.Replace(Resource.Id.framelayout, content);
                            transaction.Commit();
                            break;
                        case "Внесение оплаты":
                            MakePayment();
                            break;
                    }
                }
            };
            btn_Negative.Click += delegate
            {
                dialog.Dismiss();
            };
            dialog.Show();


            //AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
            //alert.SetTitle(titleString);
            //alert.SetMessage(messageString);
            //alert.SetPositiveButton("Да", (senderAlert, args) =>
            //{
            //    switch(titleString)
            //    {
            //        case "Сделать фотографию":
            //            GetPhoto();
            //            break;
            //        case "Сделать видео":
            //            Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            //            VideoFromServerActivity content = new VideoFromServerActivity(StaticOrder.Order_id, "");
            //            transaction.Replace(Resource.Id.framelayout, content);
            //            transaction.Commit();
            //            break;
            //        case "Внесение оплаты":
            //            MakePayment();
            //            break;
            //    }
            //});
            //alert.SetNegativeButton("Отмена", (senderAlert, args) =>
            //{
            //});
            //Dialog dialog = alert.Create();
            //dialog.Show();
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

        private async void MakeLock(bool checkBox)
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.LockRollete(StaticOrder.Order_id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                        alert1.SetTitle("Закрытие контейнера");
                        alert1.SetMessage(o_data.Message);
                        alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                        {

                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();

                        if (checkBox == true)
                        {
                            //btn_Lock.Clickable = false;
                            //btn_Lock.Focusable = false;
                            //btn_Lock.LongClickable = false;
                            btn_Lock.Visibility = ViewStates.Gone;
                            Lock.Text = "Опущена";
                        }
                        else
                        {
                            btn_Lock.Text = "Поднять";
                            Lock.Text = "Опущена";
                        }

                        //FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                        //ManageOrderActivity content2 = new ManageOrderActivity();
                        //transaction1.Replace(Resource.Id.framelayout, content2);
                        //transaction1.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }            
        }

        private async void Transmitt()
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    string[] date = Date_str.Split(' ', '-');
                    var o_data = await ManageOrderService.TransmitOrder(StaticOrder.Order_id, date[0], date[1]);
                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                        alert1.SetTitle("Закрытие контейнера");
                        alert1.SetMessage(o_data.Message);
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
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }            
        }

        private async void MakeUnLock()
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.UnLockRollete(StaticOrder.Order_id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                        alert1.SetTitle("Открытие контейнера");
                        alert1.SetMessage(o_data.Message);
                        alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                        {

                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();

                        btn_Lock.Text = "Опустить";
                        Lock.Text = "Поднята";

                        FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                        ManageOrderActivity content2 = new ManageOrderActivity();
                        transaction1.Replace(Resource.Id.framelayout, content2);
                        transaction1.Commit();
                    }
                    else
                    {
                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }           
        }

        private async void MakePayment()
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
                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                        alert1.SetTitle("Внесение оплаты");
                        alert1.SetMessage(o_data.Message);
                        alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                        {
                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();

                        FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                        ManageOrderActivity content2 = new ManageOrderActivity();
                        transaction1.Replace(Resource.Id.framelayout, content2);
                        transaction1.Commit();
                    }
                    else
                    {
                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }
        //private async void GetVideo()
        //{
        //    try
        //    {
        //        using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
        //        {
        //            ManageOrderService.InitializeClient(client);
        //            var o_data = new ServiceResponseObject<SuccessResponse>();
        //            o_data = await ManageOrderService.GetVideo(StaticOrder.Order_id);

        //            if (o_data.Status == HttpStatusCode.OK)
        //            {

        //                LayoutInflater layoutInflater = LayoutInflater.From(Activity);
        //                View view = layoutInflater.Inflate(Resource.Layout.modal_video, null);
        //                var img_get_video = view.FindViewById<VideoView>(Resource.Id.img_get_video);

        //                var src = Android.Net.Uri.Parse(URL + o_data.Message);
        //                img_get_video.SetVideoURI(src);
        //                img_get_video.Start();

        //                //var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.ResponseData.Message);
        //                //img_get_video.SetVideoURI(imageBitmap);

        //                Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
        //                alert1.SetTitle("Сделать видео");
        //                alert1.SetView(view);
        //                alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
        //                {
        //                });
        //                Dialog dialog1 = alert1.Create();
        //                dialog1.Show();
        //            }
        //            else
        //            {
        //                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
        //    }            
        //}

        private void SetPhoto()
        {
            LayoutInflater layoutInflater = LayoutInflater.From(Activity);
            View view = layoutInflater.Inflate(Resource.Layout.modal_photo, null);
            var img_get_photo = view.FindViewById<ImageView>(Resource.Id.img_get_photo);

            var src = Android.Net.Uri.Parse(URL + StaticOrder.File_Name);
            img_get_photo.SetImageURI(src);

            var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + StaticOrder.File_Name);
            img_get_photo.SetImageBitmap(imageBitmap);

            Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
            alert1.SetView(view);
            alert1.SetCancelable(false);
            alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
            {
            });
            Dialog dialog1 = alert1.Create();
            dialog1.Show();            

        }
        private async void GetPhoto()
        {
            try
            {
                
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.GetPhoto(StaticOrder.Order_id);
                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        ///дописать
                        StaticOrder.File_Name = o_data.Message;
                        StaticOrder.MessageResult = "0";
                        StartUp.StartTracking(Activity, 2);
                        
                    }
                    else
                    {
                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }         
        }

        private async void GetOrderParameters()
        {
            try
            {
                var o_data = new ServiceResponseObject<OrderObjectResponse<OrderParameters, SensorResponse, StageResponse>>();
                o_data = await OrderService.GetSensorParameters(StaticOrder.Order_id);

                if (o_data.Status == HttpStatusCode.OK)
                {
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

                    if (o_data.ResponseData.SENSORS_STATUS.gate == "1")
                    {
                        Lock.Text = "Опущена";
                        btn_Lock.Text = "Поднять";
                    }
                    else if (o_data.ResponseData.SENSORS_STATUS.gate == "0")
                    {
                        Lock.Text = "Поднята";
                        btn_Lock.Text = "Опустить";
                    }
                    else
                    {
                        Lock.Text = "Неизвестно";
                        btn_Lock.Text = "Неизвестно";
                    }

                    //if (o_data.ResponseData.SENSORS_STATUS.fold == "1")
                    //{
                    //    Fold.Text = "Разложен";
                    //}
                    //else if (o_data.ResponseData.SENSORS_STATUS.fold == "0")
                    //{
                    //    Fold.Text = "Сложен";
                    //}
                    //else
                    //{
                    //    Fold.Text = "Неизвестно";
                    //}

                    //if (o_data.ResponseData.SENSORS_STATUS.gate == "1")
                    //{
                    //    Gate.Text = "Закрыта";
                    //}
                    //else if (o_data.ResponseData.SENSORS_STATUS.gate == "0")
                    //{
                    //    Gate.Text = "Открыта";
                    //}
                    //else
                    //{
                    //    Gate.Text = "Неизвестно";
                    //}

                    btn_Lock.Enabled = (StaticOrder.Order_Stage_Id == "1" ||
                    StaticOrder.Order_Stage_Id == "4" ||
                    StaticOrder.Order_Stage_Id == "7" ||
                    StaticOrder.Order_Stage_Id == "5" ||
                    StaticOrder.Order_Stage_Id == "8") ? false : true;

                    btn_Pay.Enabled = (StaticOrder.Order_Stage_Id == "5") ? true : false;

                    btn_Photo.Enabled = (StaticOrder.Order_Stage_Id == "7" ||
                   StaticOrder.Order_Stage_Id == "8" ||
                    StaticOrder.Order_Stage_Id == "1") ? false : true;

                    btn_Video.Enabled = (StaticOrder.Order_Stage_Id == "7" ||
                        StaticOrder.Order_Stage_Id == "8" ||
                         StaticOrder.Order_Stage_Id == "1") ? false : true;
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }           
        }

    }

}