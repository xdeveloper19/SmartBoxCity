using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
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
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.box_book_parameters, null);

            view.FindViewById<TextView>(Resource.Id.txtBoxName).Text = _boxes[position].BoxId;
            var txt_order_id = view.FindViewById<TextView>(Resource.Id.txt123);
            txt_order_id.Text = _boxes[position].OrderId;

            if (_boxes[position].OrderId == "нет заказа")
            {
                txt_order_id.SetTextColor(Color.Gray);
                txt_order_id.Clickable = false;
            }
            else
            {
                txt_order_id.SetTextColor(Color.SkyBlue);
                txt_order_id.Clickable = true;
                txt_order_id.Click += delegate
                {
                    try
                    {
                        StaticOrder.Order_id = _boxes[position].OrderId;
                        MainOrderStatusActivity content2 = new MainOrderStatusActivity();
                        _manager.Replace(Resource.Id.frameDriverlayout, content2);
                        _manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(_context, ex.Message, ToastLength.Long).Show();
                    }
                };
            }

            view.FindViewById<ImageView>(Resource.Id.img_box).SetImageResource(_boxes[position].ImageView);
            view.FindViewById<TextView>(Resource.Id.txt_alarm).Text = _boxes[position].AlarmDescription;

            var btn_info_box = view.FindViewById<Button>(Resource.Id.btn_info_box);
            btn_info_box.Click += delegate
            {
                StaticBox.id = _boxes[position].Id;
                try
                {
                    BoxActivity content2 = new BoxActivity();
                    _manager.Replace(Resource.Id.frameDriverlayout, content2);
                    _manager.Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(_context, ex.Message, ToastLength.Long).Show();
                }
            };
                //var listView = view.FindViewById<ExpandableListView>(Resource.Id.expandableListView2);
                //SensorData.SampleChildData(view);
                //listView.SetAdapter(new ExpandableSensorDataAdapter(_context, SensorData.listDataHeader, SensorData.listDataChild));
                //listView.SetOnGroupClickListener(new Activity.Order.OnExpandedListenerService());

            
            return view;
        }

        
    }
}