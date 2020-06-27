using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Repository;

namespace SmartBoxCity.Activity.Order
{
    public class NotFoundOrdersActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_not_found_order, container, false);
            var image = view.FindViewById<ImageView>(Resource.Id.img_not_found);
            var txt_not_found = view.FindViewById<TextView>(Resource.Id.txt_not_found_something);
            var btn_add_order = view.FindViewById<Button>(Resource.Id.NotFoundOrderBtnAddOrder);
            if (StaticUser.NamePadeAbsenceSomething == "AlarmsActivity")
            {
                image.SetImageBitmap(BitmapFactory.DecodeResource(this.Resources , Resource.Drawable.PageNotFound));
                btn_add_order.Visibility = ViewStates.Gone;
                txt_not_found.Text = "На данный момент тревог не обнаружено.";
            }
            else
            {
                btn_add_order.Click += delegate
                {
                    try
                    {
                        FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                        AddOrderActivity content = new AddOrderActivity();
                        transaction.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                    }
                };
            }
            return view;
        }
    }
}