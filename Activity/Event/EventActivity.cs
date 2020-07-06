using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
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
                var result = GetEvents();

                if (result.Result == TaskStatus.Faulted)
                {
                    throw new Exception("Событий нет");
                }
                return view;
            }
            catch (Exception ex)
            {
                var view = inflater.Inflate(Resource.Layout.activity_errors_handling, container, false);
                var TextOfError = view.FindViewById<TextView>(Resource.Id.TextOfError);
                var image = view.FindViewById<ImageView>(Resource.Id.img_error_handing);

                TextOfError.Text += "\n(" + ex.Message + ")";
                image.SetImageResource(Resource.Drawable.PageNotFound);
                return view;
            }        
        }

        private async Task<TaskStatus> GetEvents()
        {
            var o_data = new ServiceResponseObject<EventsResponse>();
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                try
                {
                    OrderService.InitializeClient(client);
                    o_data = await OrderService.Events(StaticOrder.Order_id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                        var number = 0;

                        if (o_data.ResponseData.EVENTS == null || o_data.ResponseData.EVENTS.Count == 0)
                        {
                            return TaskStatus.Faulted;
                        }

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
                                    ContentType = kvp.Value[i].type
                                });
                            }
                        }

                        UpdateList();
                        return TaskStatus.Running;
                    }
                    else
                    {
                        return TaskStatus.Faulted;//"Unexpected character encountered while parsing value: <. Path '', line 0, position 0."

                    }
                }
                catch (Exception ex)
                {
                    return TaskStatus.Faulted;
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