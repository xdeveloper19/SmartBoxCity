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
using Entity.Repository;

namespace SmartBoxCity.Activity.Box
{
    public class MainBoxStatusActivity: Fragment
    {
        private Button btn_boxes_car;

        private Button btn_boxes_depot;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_menu_box, container, false);

            btn_boxes_car = view.FindViewById<Button>(Resource.Id.btn_boxes_car);
            btn_boxes_depot = view.FindViewById<Button>(Resource.Id.btn_boxes_depot);
            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();

            btn_boxes_car.Click += delegate
            {
                try
                {
                    BoxListActivity content1 = new BoxListActivity();
                    StaticBox.isDepot = false;
                    Bundle args = new Bundle();
                    args.PutBoolean("isDepot", false);
                    content1.Arguments = args;
                    transaction1.Replace(Resource.Id.frameDriverlayout, content1).AddToBackStack(null);
                    transaction1.Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                }
               
            };

            btn_boxes_depot.Click += async delegate
            {
                try
                {
                    BoxListActivity content2 = new BoxListActivity();
                    StaticBox.isDepot = true;
                    Bundle args = new Bundle();
                    args.PutBoolean("isDepot", true);
                    content2.Arguments = args;
                    transaction1.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null);
                    transaction1.Commit();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Activity, ex.Message, ToastLength.Long);
                }
            };

            return view;
        }
    }
}