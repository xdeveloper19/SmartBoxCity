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
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Activity.Registration;

namespace SmartBoxCity.Activity.Home
{
    public class MainPageActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.LayoutLast, container, false);
            var btn_calculate = view.FindViewById<Button>(Resource.Id.btn_cost);

            Button btn_auth1 = view.FindViewById<Button>(Resource.Id.btn_auth2);

            btn_auth1.Click += (s, e) =>
            {
                Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                AuthActivity content3 = new AuthActivity();
                transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
            };

            // Переход к форме регистрации.
            btn_calculate.Click += (s, e) =>
            {
                    //set alert for executing the task
                    try
                {
                    Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                    AddOrderActivity content = new AddOrderActivity();
                    transaction2.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                    
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
                }

            };
            return view;
            // Переход к форме авторизация

        }
    }
}