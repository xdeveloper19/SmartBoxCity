﻿using System;
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
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using Entity.Repository;
using Plugin.Settings;
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Activity.Event
{
    public class BoxEventsActivity: Fragment
    {
        private ListView lstEvent;
        public static List<EventModel> Eventlist;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var view = inflater.Inflate(Resource.Layout.activity_events, container, false);

                lstEvent = view.FindViewById<ListView>(Resource.Id.eventlistview);
                Eventlist = new List<EventModel>();
                GetEvents();

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

        private async void GetEvents()
        {
            var o_data = new ServiceResponseObject<EventsBoxResponse>();
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                BoxService.InitializeClient(client);
                o_data = await BoxService.Events(StaticBox.id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    var number = 0;

                    if (o_data.ResponseData.EVENTS == null || o_data.ResponseData.EVENTS.Count == 0)
                    {
                        throw new Exception("Событий нет");
                    }

                    foreach (KeyValuePair<string, List < Entity.Model.BoxResponse.EventResponse>> kvp in o_data.ResponseData.EVENTS)
                    {
                        for (int i = 0; i < o_data.ResponseData.EVENTS[kvp.Key].Count; i++)
                        {
                            string[] times = kvp.Value[i].created_at.Split(new char[] { ' ' });
                            Eventlist.Add(new EventModel
                            {
                                id = number++,
                                Id = StaticBox.id,
                                Name = kvp.Value[i].message,
                                Time = "(" + times[1] + ")",
                                Date = times[0],
                                ContentType = kvp.Value[i].type
                            });
                        }
                    }

                    UpdateList();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: <. Path '', line 0, position 0."

                }
            }
        }

        public override void OnResume()
        {
            base.OnResume();
        }
        public void UpdateList()
        {
            EventListAdapter adapter = new EventListAdapter(Activity, Eventlist, this.FragmentManager);
            lstEvent.Adapter = adapter;
        }
    }
}