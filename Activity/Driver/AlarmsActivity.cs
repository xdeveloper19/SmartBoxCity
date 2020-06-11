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
using Entity.Model.AlarmResponse;
using Entity.Model.AlarmViewModel;
using Entity.Model.BoxViewModel;
using Plugin.Settings;
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Activity.Driver
{
    public class AlarmsActivity: Fragment
    {
        private ListView lstAlarm;
        public static List<AlarmBookModel> alarmlist;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_list_alarms, container, false);
            lstAlarm = view.FindViewById<ListView>(Resource.Id.alarmlistview);
            GetAlarms();
            return view;
        }

        private async void GetAlarms()
        {
            var o_data = new ServiceResponseObject<ListAlarmResponse>();
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                //надо было сначала клиента указать, а потом вызывать метод
                //и обязательно с токеном
                AlarmService.InitializeClient(client);
                o_data = await AlarmService.GetAlarms();

                if (o_data.Status == HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    //StaticUser.Email = s_login.Text;
                    //StaticUser.AddInfoAuth(o_user_data);
                    alarmlist = new List<AlarmBookModel>();
                    //обязательно должен быть прогресс бар при обращении к серверу, типо такого
                    //preloader.Visibility = Android.Views.ViewStates.Invisible;
                               
                    foreach (var alm in o_data.ResponseData.ALARMS_STATUS)
                    {
                        alarmlist.Add(new AlarmBookModel
                        {
                            Id = alm.id,
                            Acknowledged = alm.acknowledged,
                            Container_id = alm.container_id,
                            Name = alm.name,
                            Raised_At = alm.raised_at
                        }
                        );
                    }

                    UpdateList();
                    lstAlarm.ItemClick += ListBoxes_ItemClick;

                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."
                    FragmentTransaction transaction3 = this.FragmentManager.BeginTransaction();
                    NotFoundAlarmsActivity content2 = new NotFoundAlarmsActivity();
                    transaction3.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null).Commit();
                }
            }
        }

        private void ListBoxes_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Activity, "", ToastLength.Long).Show();
        }

        public override void OnResume()
        {
            // UpdateList();
            base.OnResume();
        }

        public void UpdateList()
        {
            AlarmsAdapter adapter = new AlarmsAdapter(Activity, alarmlist, this.FragmentManager);
            lstAlarm.Adapter = adapter;
        }
    }
}