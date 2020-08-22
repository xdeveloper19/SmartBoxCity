using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.BoxResponse;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Event;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Service;
using WebService;
using WebService.Driver;
using Xamarin.Essentials;
using static Android.Support.Design.Widget.AppBarLayout;

namespace SmartBoxCity.Activity.Box
{
    [Obsolete]
    public class BoxActivity: Fragment
    {
        #region Объявление переменных
        private TextView BoxTextIdValue;
        private TextView BoxTexWeight;
        private TextView BoxTextTemperature;
        private TextView BoxTexBattery;
        private TextView BoxTextIllumination;
        private TextView BoxTextHumidity;
        private TextView BoxTextGate;
        //private TextView BoxTextLock;
        private TextView BoxTextFold;
        private TextView BoxTextEvents;

        public Button BoxButtonStop { get; private set; }

        private Button BoxButtonDetach;
        private Button BoxButtonPhoto;
        private Button BoxButtonVideo;
        private Button btn_fold;
        private Button btn_gate;
        private LinearLayout BoxLinearAlarms;
        private const string URL = "https://smartboxcity.ru/";
        #endregion
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_info_box, container, false);
            #region Иннициализация переменных
            BoxTextIdValue = view.FindViewById<TextView>(Resource.Id.BoxTextIdValue);
            BoxTexWeight = view.FindViewById<TextView>(Resource.Id.BoxTexWeight);
            BoxTextTemperature = view.FindViewById<TextView>(Resource.Id.BoxTextTemperature);
            BoxTexBattery = view.FindViewById<TextView>(Resource.Id.BoxTexBattery);
            BoxTextIllumination = view.FindViewById<TextView>(Resource.Id.BoxTextIllumination);
            BoxTextHumidity = view.FindViewById<TextView>(Resource.Id.BoxTextHumidity);
            BoxTextGate = view.FindViewById<TextView>(Resource.Id.BoxTextGate);
            BoxTextFold = view.FindViewById<TextView>(Resource.Id.BoxTextFold);
            //BoxTextLock = view.FindViewById<TextView>(Resource.Id.BoxTextLock);
            BoxTextEvents = view.FindViewById<TextView>(Resource.Id.BoxTextEvents);


            BoxButtonStop = view.FindViewById<Button>(Resource.Id.BoxButtonStop);
            BoxButtonDetach = view.FindViewById<Button>(Resource.Id.BoxButtonDetach);
            BoxButtonVideo = view.FindViewById<Button>(Resource.Id.BoxButtonVideo);
            BoxButtonPhoto = view.FindViewById<Button>(Resource.Id.BoxButtonPhoto);
            btn_fold = view.FindViewById<Button>(Resource.Id.btn_fold);
            btn_gate = view.FindViewById<Button>(Resource.Id.btn_gate);

            BoxButtonStop.Enabled = (StaticBox.isDepot) ? false : true;
            btn_gate.Enabled = (StaticBox.isDepot) ? false : true;
            btn_fold.Enabled = (StaticBox.isDepot) ? false : true;

            BoxLinearAlarms = view.FindViewById<LinearLayout>(Resource.Id.BoxLinearAlarms);

            BoxButtonDetach.Text = (StaticBox.isDepot) ? "Прикрепить" : "Открепить";
            btn_fold.Enabled = (StaticBox.isDepot) ? false : true;
            #endregion

            BoxTextEvents.Click += BoxTextEvents_Click;
            GetBoxParameters();

            if (StaticBox.alarms != null)
            {
                foreach (var alarm in StaticBox.alarms)
                {
                    // Add textview 1
                    var textview = new TextView(Activity)
                    {
                        LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent),
                        Text = alarm.name + "\n" + alarm.raised_at.ToString(),
                        TextSize = 11,
                        Gravity = GravityFlags.Center
                    };
                    textview.SetBackgroundResource(Resource.Drawable.button_cancel);
                    textview.SetTextColor(Color.White);
                    BoxLinearAlarms.AddView(textview);
                }
            }

            btn_gate.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                if (btn_gate.Text == "Поднять")
                {
                    alert.SetTitle("Поднять роллету");
                    alert.SetMessage("Вы действительно хотите поднять роллету?");
                    alert.SetPositiveButton("Да", (senderAlert, args) =>
                    {
                        MakeUnlockRollete(alert);

                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                }
                else 
                {
                    alert.SetTitle("Опустить роллету");
                    alert.SetMessage("Вы действительно хотите опустить роллету?");
                    alert.SetPositiveButton("Да", (senderAlert, args) =>
                    {
                        MakeLockRollete(alert);
                    });
                }
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            btn_fold.Click += delegate
            {

                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                if (btn_fold.Text == "Сложить")
                {
                    alert.SetTitle("Сложить контейнер");
                    alert.SetMessage("Вы действительно хотите сложить контейнер?");
                    alert.SetPositiveButton("Сложить", (senderAlert, args) =>
                    {
                        MakeFold(alert);
                       
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                }
                else if (btn_fold.Text == "Разложить")
                {
                    alert.SetTitle("Разложить контейнер");
                    alert.SetMessage("Вы действительно хотите разложить контейнер?");
                    alert.SetPositiveButton("Разложить", (senderAlert, args) =>
                    {
                        MakeUnfold(alert);
                    });
                }
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };


            BoxButtonDetach.Click += delegate
            {
                if (StaticBox.isDepot)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Прикрепить контейнер");
                    alert.SetMessage("Вы действительно прикрепить контейнер?");
                    alert.SetPositiveButton("Продолжить", (senderAlert, args) =>
                    {
                        Attach(alert);
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Вы действительно хотите открепить контейнер?");
                    //alert.SetMessage("Вы действительно открепить контейнер?");
                    List<string> Item = new List<string>();
                    Item.Add("Создать задачу забрать контейнер для другого водителя.");

                    bool[] toDownload = { false };
                    alert.SetMultiChoiceItems(Item.ToArray(), toDownload, (sender, e) =>
                    {
                        int index = e.Which;

                        toDownload[index] = e.IsChecked;
                    });
                    alert.SetPositiveButton("Продолжить", (senderAlert, args) =>
                    {
                        Dettach(alert, toDownload[0]);
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            };

            BoxButtonStop.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                alert.SetTitle("Подтверждение");
                alert.SetMessage("Вы действительно хотите остановить выполнение команд?");
                alert.SetPositiveButton("Да", (senderAlert, args) =>
                {
                    StopCommands();
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            BoxButtonPhoto.Click += delegate
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
            BoxButtonVideo.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                alert.SetTitle("Сделать видео");
                alert.SetMessage("Вы действительно хотите сделать видео с камеры контейнера?");
                alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                {
                    Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                    GetBoxVideo content = new GetBoxVideo(StaticBox.id, "");
                    transaction.Replace(Resource.Id.frameDriverlayout, content);
                    transaction.Commit();
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            return view;
        }

        private async void MakeUnfold(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.UnfoldContainer(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Разложить контейнер");
                    alert1.SetMessage(o_data.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {

                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();

                    btn_fold.Text = "Сложить";
                    BoxTextFold.Text = "Разложен";

                    try
                    {
                        FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                        BoxActivity content2 = new BoxActivity();
                        transaction1.Replace(Resource.Id.frameDriverlayout, content2);
                        transaction1.Commit();
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(Activity, "Ошибка обновления страницы", ToastLength.Long);
                    }
                }


            }
        }

        private async void StopCommands()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.StopCommands(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    Toast.MakeText(Activity, "Успешно!", ToastLength.Long).Show();
                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    MainBoxStatusActivity content2 = new MainBoxStatusActivity();
                    transaction1.Replace(Resource.Id.frameDriverlayout, content2);
                    transaction1.Commit();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }
        }

        private async void MakeUnlockRollete(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.UnlockRollete(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Поднять роллету");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {

                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();

                    btn_gate.Text = "Опустить";
                    BoxTextGate.Text = "Открыта";

                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    BoxActivity content2 = new BoxActivity();
                    transaction1.Replace(Resource.Id.frameDriverlayout, content2);
                    transaction1.Commit();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }
        }

        private void BoxTextEvents_Click(object sender, EventArgs e)
        {
            try
            {
                FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                BoxEventsActivity content2 = new BoxEventsActivity();
                transaction1.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null);
                transaction1.Commit();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }

        private async void Dettach(AlertDialog.Builder alert, bool isChecked)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.Dettach(StaticBox.id, isChecked);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Открепить контейнер");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {

                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();

                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    MainBoxStatusActivity content2 = new MainBoxStatusActivity();
                    transaction1.Replace(Resource.Id.frameDriverlayout, content2);
                    transaction1.Commit();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }
        }

        [Obsolete]
        private async void Attach(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.Attach(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Прикрепить контейнер");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();

                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    MainBoxStatusActivity content2 = new MainBoxStatusActivity();
                    transaction1.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null).Commit();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }

        }

        private async void MakeFold(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.FoldContainer(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Сложить контейнер");
                    alert1.SetMessage(o_data.ResponseData.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {
                       
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();

                    btn_fold.Text = "Разложить";
                    BoxTextFold.Text = "Сложен";

                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    BoxActivity content2 = new BoxActivity();
                    transaction1.Replace(Resource.Id.frameDriverlayout, content2);
                    transaction1.Commit();
                }


            }
        }



        private async void MakeLockRollete(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.LockRollete(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
                    alert1.SetTitle("Опустить роллету");
                    alert1.SetMessage(o_data.Message);
                    alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                    {
                        
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();

                    btn_gate.Text = "Поднять";
                    BoxTextGate.Text = "Закрыта";

                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    BoxActivity content2 = new BoxActivity();
                    transaction1.Replace(Resource.Id.frameDriverlayout, content2);
                    transaction1.Commit();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }

        }


        //private async void GetVideo(AlertDialog.Builder alert)
        //{
        //    using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
        //    {
        //        BoxService.InitializeClient(client);
        //        var o_data = new ServiceResponseObject<SuccessResponse>();
        //        o_data = await BoxService.GetVideo(StaticBox.id);

        //        if (o_data.Status == HttpStatusCode.OK)
        //        {
        //            alert.Dispose();

        //            LayoutInflater layoutInflater = LayoutInflater.From(Activity);
        //            View view = layoutInflater.Inflate(Resource.Layout.modal_video, null);
        //            var img_get_video = view.FindViewById<VideoView>(Resource.Id.img_get_video);

        //            var src = Android.Net.Uri.Parse(URL + o_data.ResponseData.Message);
        //            img_get_video.SetVideoURI(src);
        //            img_get_video.Start();

        //            //var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.ResponseData.Message);
        //            //img_get_video.SetVideoURI(imageBitmap);

        //            Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Activity);
        //            alert1.SetTitle("Сделать видео");
        //            alert1.SetView(view);
        //            alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
        //            {
        //            });
        //            Dialog dialog1 = alert1.Create();
        //            dialog1.Show();
        //        }
        //        else
        //        {
        //            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
        //        }
        //    }
        //}

        private async void GetPhoto(AlertDialog.Builder alert)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await BoxService.GetPhoto(StaticBox.id);

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


        private async void GetBoxParameters()
        {

            var o_data = new ServiceResponseObject<InfoBoxResponse>();

            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                o_data = await BoxService.GetInfoBox(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

                    StaticBox.AddInfoSensors(o_data.ResponseData.SENSORS_STATUS);
                    StaticBox.AddInfoContainer(o_data.ResponseData.CONTAINER, o_data.ResponseData.EVENT_COUNT);
                    StaticBox.AddInfoAlarms(o_data.ResponseData.ALARMS_STATUS);

                    BoxTextIdValue.Text = (o_data.ResponseData.CONTAINER.id == null) ? "неизвестно" : o_data.ResponseData.CONTAINER.id;
                    BoxTexWeight.Text = (o_data.ResponseData.SENSORS_STATUS.weight == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.weight;
                    BoxTextTemperature.Text = (o_data.ResponseData.SENSORS_STATUS.temperature == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.temperature;
                    BoxTexBattery.Text = (o_data.ResponseData.SENSORS_STATUS.battery == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.battery;
                    BoxTextIllumination.Text = (o_data.ResponseData.SENSORS_STATUS.illumination == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.illumination;
                    BoxTextHumidity.Text = (o_data.ResponseData.SENSORS_STATUS.humidity == null) ? "неизвестно" : o_data.ResponseData.SENSORS_STATUS.humidity;
                    BoxTextEvents.Text = (o_data.ResponseData.EVENT_COUNT == null) ? "неизвестно" : StaticBox.event_count + " шт.";

                    


                    if (o_data.ResponseData.SENSORS_STATUS.Lock == "1")
                    {
                        //BoxTextLock.Text = "Закрыт";

                        BoxTextGate.Text = "Закрыта";
                        btn_gate.Text = "Поднять";
                    }
                    else if (o_data.ResponseData.SENSORS_STATUS.Lock == "0")
                    {
                        //BoxTextLock.Text = "Открыт";

                        BoxTextGate.Text = "Открыта";
                        btn_gate.Text = "Опустить";
                    }
                    else
                    {
                        //BoxTextLock.Text = "Неизвестно";

                        BoxTextGate.Text = "Неизвестно";
                        btn_gate.Text = "Неизвестно";
                        btn_gate.Enabled = false;
                    }

                    if (o_data.ResponseData.SENSORS_STATUS.fold == "1")
                    {
                        BoxTextFold.Text = "Сложен";
                        btn_fold.Text = "Разложить";
                    }
                    else if (o_data.ResponseData.SENSORS_STATUS.fold == "0")
                    {
                        BoxTextFold.Text = "Разложен";
                        btn_fold.Text = "Сложить";
                    }
                    else
                    {
                        BoxTextFold.Text = "Неизвестно";
                        btn_fold.Text = "Неизвестно";
                    }
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            }

        }
    }
}