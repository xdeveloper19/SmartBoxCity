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
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Order;
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
            try
            {
                var o_data = new ServiceResponseObject<ListAlarmResponse>();
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {

                    AlarmService.InitializeClient(client);
                    o_data = await AlarmService.GetAlarms();
                    if (o_data.Status == HttpStatusCode.OK)
                    {

                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                        alarmlist = new List<AlarmBookModel>();

                        if (o_data.ResponseData.ALARMS_STATUS == null || 
                            o_data.ResponseData.ALARMS_STATUS.Count == 0)
                        {
                            StaticUser.NamePadeAbsenceSomething = "AlarmsActivity";
                            Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                            NotFoundOrdersActivity content = new NotFoundOrdersActivity();
                            transaction.Replace(Resource.Id.frameDriverlayout, content);
                            transaction.Commit();
                        }

                        foreach (var alm in o_data.ResponseData.ALARMS_STATUS)
                        {
                            alarmlist.Add(new AlarmBookModel
                            {
                                Id = alm.id,
                                Acknowledged = alm.acknowledged,
                                Container_id = alm.container_id,
                                Name = alm.name,
                                Raised_At = alm.raised_at,
                                IsDepot = (alm.depot == "1") ? true : false
                            }
                            );
                        }
                        UpdateList();
                        lstAlarm.ItemClick += ListBoxes_ItemClick;
                    }
                    else
                    {
                        StaticUser.NamePadeAbsenceSomething = "AlarmsActivity";
                        Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                        NotFoundOrdersActivity content = new NotFoundOrdersActivity();
                        transaction.Replace(Resource.Id.frameDriverlayout, content);
                        transaction.Commit();
                    }
                }

            }
            catch (System.Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
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