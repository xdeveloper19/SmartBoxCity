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
using SmartBoxCity.Model.OrderViewModel;

namespace SmartBoxCity.Activity.Event
{
     public class EventListAdapter : BaseAdapter<EventModel>
    {

        Context context;
        List<EventModel> events;
        Android.App.FragmentTransaction manager;
        public EventListAdapter(Context Context, List<EventModel> List, FragmentManager Manager)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.events = List;
        }
        public override EventModel this[int position] => events[position];

        public override int Count => events.Count;

        public override long GetItemId(int position)
        {
            return events[position].Id;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.EventCardView, null);
                view.FindViewById<TextView>(Resource.Id.EventCardTextName).Text = events[position].Name;
                view.FindViewById<TextView>(Resource.Id.EventCardTextTime).Text = events[position].Time;
                view.FindViewById<TextView>(Resource.Id.EventCardTextDate).Text = events[position].Date;
            }
            return view;
        }

    }
}