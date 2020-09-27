using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity;
using WebService;
using WebService.Client;
using WebService.Driver;

namespace SmartBoxCity.Service
{
    [BroadcastReceiver]
    public class MyTasksBroadcastReceiver : BroadcastReceiver
    {
        public static string ACTION_PROCESS_TASKS = "SmartBoxCity.UPDATE_TASKS";
        public const string ACTION_PROCESS_CLIENT_PHOTO = "SmartBoxCity.CLIENT_PHOTO";

        // Идентификатор уведомления
        private static int NOTIFY_ID = 101;
        static readonly string CHANNEL_ID = "location_notification";

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent != null)
            {
                string action = intent.Action;
                if (action.Equals(ACTION_PROCESS_TASKS))
                {
                    try
                    {
                        CreateNotificationChannel(context);
                        GetTasks(context);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(context, "Приложение закрыто!", ToastLength.Long);
                    }
                }
                else if (action.Equals(ACTION_PROCESS_CLIENT_PHOTO))
                {
                    try
                    {
                        //CreateNotificationChannel(context);
                        UpdateFileData(context, StaticOrder.File_Name);
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(context, "Приложение закрыто!", ToastLength.Long);
                    }
                }
            }
        }

        private void PushNotifications(Context context, string title, string text)
        {
            // Create PendingIntent
            Intent map_intent = new Intent(context, typeof(MainActivity2));

            PendingIntent resultPendingIntent = PendingIntent.GetActivity(context, 0, map_intent,
            PendingIntentFlags.UpdateCurrent);

            //Create Notification
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
            .SetContentTitle(title)
            .SetContentText(text)
            .SetAutoCancel(true)
            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone))
            .SetVibrate(new long[] { 1000, 1000 })
            .SetContentIntent(resultPendingIntent);

            //Show Notification
            Notification notification = builder.Build();
            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(context);
            var str = notificationManager.AreNotificationsEnabled();
            //NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(NOTIFY_ID, notification);
        }


        void CreateNotificationChannel(Context context)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var name = context.Resources.GetString(Resource.String.channel_name);
            var description = context.Resources.GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };

            NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.CreateNotificationChannel(channel);
        }

        private async void GetTasks(Context context)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                TaskService.InitializeClient(client);
                var o_data = await TaskService.GetTasks();

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    StaticDriver.AddInfoDriver(o_data.ResponseData.DRIVER);

                    if (o_data.ResponseData.TASKS.Count == 0)
                    {
                        StaticTask.IsStoppedGeo = true;
                        //PushNotifications(context, "Задач не обнаружено", "Просмотреть");
                        return;
                    }
                    PushNotifications(context, "Новая задача на перевозку груза", "Просмотреть задачу");
                    StaticTask.IsStoppedGeo = false;
                    StaticTask.IsStoppedGettingTasks = true;
                    StartUp.StopTracking(context, 1);
                    return;
                }
                else
                {
                    StaticTask.IsStoppedGeo = true;
                    return;
                }
            }
        }

        private async void UpdateFileData(Context context, string file_name)
        {
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{CrossSettings.Current.GetValueOrDefault("token", "")}:")));
            HttpClient client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue },
                BaseAddress = new Uri("https://smartboxcity.ru/media.php?media=")
            };

            if(CrossSettings.Current.GetValueOrDefault("role", "") == "driver")
            {
                BoxService.InitializeClient(client);
                var o_data = await BoxService.CheckFile(file_name);
                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    StaticOrder.MessageResult = o_data.Message;
                    //дописать
                    if (o_data.Message == "1")
                    {
                        StartUp.StopTracking(context, 2);
                    }

                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                ManageOrderService.InitializeClient(client);
                var o_data = await ManageOrderService.CheckFile(file_name);
                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    StaticOrder.MessageResult = o_data.Message;
                    //дописать
                    if (o_data.Message == "1")
                    {
                        StartUp.StopTracking(context, 2);
                    }

                    return;
                }
                else
                {
                    return;
                }
            }            

            
            
        }
    }
}