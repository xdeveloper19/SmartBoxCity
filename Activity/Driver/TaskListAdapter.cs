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
using Entity.Model.TaskViewModel;
using SmartBoxCity.Activity.Order;

namespace SmartBoxCity.Activity.Driver
{
    public class TaskListAdapter: BaseAdapter<TaskBookModel>
    {
        Context context;
        List<TaskBookModel> tasks;
        Android.App.FragmentTransaction manager;
        public TaskListAdapter(Context Context, List<TaskBookModel> List, FragmentManager Manager)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.tasks = List;
        }
        public override TaskBookModel this[int position] => tasks[position];

        public override int Count => tasks.Count;

        public override long GetItemId(int position)
        {
            return position;//Convert.ToInt64(orders[position].Id);
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(context).Inflate(Resource.Layout.task_book_parameter, null);

            view.FindViewById<TextView>(Resource.Id.txtTaskName).Text = "Заказ: " + tasks[position].order_id;
            view.FindViewById<TextView>(Resource.Id.txtComment).Text = tasks[position].title;

            var btn = view.FindViewById<Button>(Resource.Id.btn_info_order);

            btn.Click += async delegate
            {
                try
                {
                    MainOrderStatusActivity content = new MainOrderStatusActivity();
                    manager.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                }
               
            };

            return view;
        }
    }
}