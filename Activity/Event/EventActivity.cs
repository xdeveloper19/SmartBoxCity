using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmartBoxCity.Activity.Event;
using SmartBoxCity.Model.OrderViewModel;

namespace SmartBoxCity.Activity.Order
{
    public class EventsActivity : Fragment
    {
        private ListView lstEvent;
        public static List<EventModel> Eventlist;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_events, container, false);
            lstEvent = view.FindViewById<ListView>(Resource.Id.eventlistview);
            Eventlist = new List<EventModel>();

            EventModel a = new EventModel()
            {
                Id = 111,
                Name = "Заказ выполнен",
                Time = "07:41:07",
                Date = "2020-04-30"
            };

            EventModel b = new EventModel()
            {
                Id = 222,
                Name = "Завершение использования",
                Time = "07:40:58",
                Date = "2020-04-30"
            };

            EventModel c = new EventModel()
            {
                Id = 333,
                Name = "Выгрузка",
                Time = " 07:40:48",
                Date = "2020-04-30"
            };

            Eventlist.Add(a);
            Eventlist.Add(b);
            Eventlist.Add(c);
            UpdateList();

            return view;
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