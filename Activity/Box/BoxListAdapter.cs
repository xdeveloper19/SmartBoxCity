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
using Entity.Model.BoxViewModel;
using Entity.Repository;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Service;
using static Android.Widget.ExpandableListView;

namespace SmartBoxCity.Activity.Box
{
    public class BoxListAdapter: BaseAdapter<BoxBookModel>
    {
        Context _context;
        List<BoxBookModel> _boxes;
        Android.App.FragmentTransaction _manager;
        public BoxListAdapter(Context Context, List<BoxBookModel> List, FragmentManager Manager)
        {
            this._manager = Manager.BeginTransaction();
            this._context = Context;
            this._boxes = List;
        }
        public override BoxBookModel this[int position] => _boxes[position];

        public override int Count => _boxes.Count;

        public override long GetItemId(int position)
        {
            return _boxes[position].Id;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.box_book_parameters, null);

            view.FindViewById<TextView>(Resource.Id.txtBoxName).Text = _boxes[position].BoxId;
            view.FindViewById<TextView>(Resource.Id.txt123).Text = _boxes[position].OrderId;
            view.FindViewById<ImageView>(Resource.Id.img_box).SetImageResource(_boxes[position].ImageView);
            view.FindViewById<TextView>(Resource.Id.txt_alarm).Text = _boxes[position].AlarmDescription;

            var btn_info_box = view.FindViewById<Button>(Resource.Id.btn_info_box);
            btn_info_box.Click += delegate
            {
                StaticBox.id = _boxes[position].BoxId;
                BoxActivity content2 = new BoxActivity();
                _manager.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null).Commit();
            };
                //var listView = view.FindViewById<ExpandableListView>(Resource.Id.expandableListView2);
                //SensorData.SampleChildData(view);
                //listView.SetAdapter(new ExpandableSensorDataAdapter(_context, SensorData.listDataHeader, SensorData.listDataChild));
                //listView.SetOnGroupClickListener(new Activity.Order.OnExpandedListenerService());

            
            return view;
        }

        
    }
}