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
using Entity.Model.AlarmViewModel;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Box;
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Activity.Driver
{
    public class AlarmsAdapter: BaseAdapter<AlarmBookModel>
    {
        Context _context;
        List<AlarmBookModel> _alarms;
        Android.App.FragmentTransaction _manager;
        public AlarmsAdapter(Context Context, List<AlarmBookModel> List, FragmentManager Manager)
        {
            this._manager = Manager.BeginTransaction();
            this._context = Context;
            this._alarms = List;
        }
        public override AlarmBookModel this[int position] => _alarms[position];

        public override int Count => _alarms.Count;

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.driver_info_alarm, null);

            var txt_container = view.FindViewById<TextView>(Resource.Id.txt_title_alarm);
            txt_container.Text = _alarms[position].Container_id;
            view.FindViewById<TextView>(Resource.Id.txt_alarm_description).Text = _alarms[position].Name;
            view.FindViewById<TextView>(Resource.Id.txt_alarm_date).Text = _alarms[position].Raised_At.ToString();

            var btn_info_alarm = view.FindViewById<Button>(Resource.Id.btn_get_alarm);

            if (_alarms[position].Acknowledged == "0")
            {
                btn_info_alarm.Enabled = true;
                btn_info_alarm.Text = "Признать";
                btn_info_alarm.SetBackgroundResource(Resource.Drawable.button_cancel);
            }
            else
            {
                btn_info_alarm.Enabled = false;
                btn_info_alarm.Text = "Признана";
                btn_info_alarm.SetBackgroundResource(Resource.Drawable.button_primary);
            }


            btn_info_alarm.Click += async delegate
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    AlarmService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await AlarmService.Acknowledge(_alarms[position].Container_id, _alarms[position].Id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        Toast.MakeText(Application.Context, o_data.Message, ToastLength.Long).Show();
                        try
                        {
                            AlarmsActivity content2 = new AlarmsActivity();
                            _manager.Replace(Resource.Id.frameDriverlayout, content2).Commit();
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(_context, ex.Message, ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, o_data.Message, ToastLength.Long).Show();
                    }
                }
            };

            txt_container.Click += delegate
            {
                try
                {
                    BoxActivity content2 = new BoxActivity();
                    StaticBox.id = _alarms[position].Container_id;
                    _manager.Replace(Resource.Id.frameDriverlayout, content2).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(_context, ex.Message, ToastLength.Long).Show();
                }
               
            };

            return view;
        }
    }
}