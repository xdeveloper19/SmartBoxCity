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
using Entity.Model.OrderResponse;
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Event;
using WebService;
using WebService.Client;

namespace SmartBoxCity.Activity.Order
{
    public class EventsActivity : Fragment
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
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                return view;
            }        
        }

        private async void GetEvents()
        {
            var o_data = new ServiceResponseObject<EventsResponse>();
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                OrderService.InitializeClient(client);
                o_data = await OrderService.Events(StaticOrder.Order_id);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    var number = 0;

                    foreach (KeyValuePair<string, List<EventResponse>> kvp in o_data.ResponseData.EVENTS)
                    {
                        for (int i = 0; i < o_data.ResponseData.EVENTS[kvp.Key].Count; i++)
                        {
                            Eventlist.Add(new EventModel
                            {
                                id = number++,
                                Id = kvp.Value[i].order_id,
                                Name = kvp.Value[i].message,
                                Time = "(" + kvp.Value[i].created_at.ToLongTimeString() + ")",
                                Date = kvp.Value[i].event_day,
                            });
                        }
                    }

                    UpdateList();
                    lstEvent.ItemClick += ListOrders_ItemClick;
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
        private void ListOrders_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Activity, "Выбран заказ №" + e.Position.ToString(), ToastLength.Long).Show();
        }
    }
}