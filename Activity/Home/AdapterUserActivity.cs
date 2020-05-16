using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Repository;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Home
{
    public class AdapterUserActivity : BaseAdapter<OrderAdapter>
    {
        Context context;
        List<OrderAdapter> orders;
        Android.App.FragmentTransaction manager;
        
        public AdapterUserActivity(Context Context, List<OrderAdapter> List, FragmentManager Manager)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.orders = List;
        }
        public override OrderAdapter this[int position] => orders[position];

        public override int Count => orders.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return orders[position];
        }




        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            
                view = LayoutInflater.From(context).Inflate(Resource.Layout.activity_user_CardView, null);
                view.FindViewById<TextView>(Resource.Id.container_name).Text = orders[position].id;
                view.FindViewById<TextView>(Resource.Id.status_view).Text = orders[position].order_stage_id 
                    + ". " + orders[position].order_stage_name;
                view.FindViewById<TextView>(Resource.Id.s_cost).Text = orders[position].payment_amount;
                if(view.FindViewById<TextView>(Resource.Id.s_payment).Text == "1")
                {
                    view.FindViewById<TextView>(Resource.Id.s_payment).Text = "Оплачено";
                }
                else
                {
                    view.FindViewById<TextView>(Resource.Id.s_payment).Text = "Не оплачено";
                    view.FindViewById<TextView>(Resource.Id.s_payment).SetTextColor(Color.ParseColor("#EC8F9B"));
                }

                var btn_pay = view.FindViewById<Button>(Resource.Id.btn_pay);
                var progress = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
                var btn_pass_delivery_service = view.FindViewById<Button>(Resource.Id.btn_pass_delivery_service);
                var btn_make_photo = view.FindViewById<Button>(Resource.Id.btn_make_photo);
                var btn_make_video = view.FindViewById<Button>(Resource.Id.btn_make_video);
                var btn_order_management = view.FindViewById<Button>(Resource.Id.btn_order_management);
                btn_pass_delivery_service.Click += Btn_Click_Delivery_Service;
                btn_make_photo.Click += Btn_Click_Make_Poto;
                btn_make_video.Click += Btn_Click_Make_Video;
                btn_order_management.Click += delegate
                {
                    try
                    {
                        ManageOrderActivity content = new ManageOrderActivity(view.FindViewById<TextView>(Resource.Id.container_name).Text);

                        //для сохранения данных и использования их в других активити используй
                        //статический класс как ниже, необязательно использовать для таких целей
                        //конструктор
                        StaticOrder.Order_id = orders[position].id;
                        manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                    }
                   
                };

                int order_stage;
                var result = int.TryParse(orders[position].order_stage_id, out order_stage);

                if (result == true)
                    progress.Progress = order_stage;
                else
                    progress.Progress = 0;
                //btn.Click += async delegate
                //{
                //    OrderActivity content = new OrderActivity();
                //    manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                //};
            return view;
        }

       

        private void Btn_Click_Make_Video(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Btn_Click_Make_Poto(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Btn_Click_Delivery_Service(object sender, EventArgs e)
        {
            try
            {
                MainOrderStatusActivity content = new MainOrderStatusActivity();
                manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
            }
        }
    }
}