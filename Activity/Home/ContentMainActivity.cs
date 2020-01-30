using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Order;

namespace SmartBoxCity.Activity.Home
{
    public class ContentMainActivity : Fragment
    {
        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        private Button btn_auth_form;

        /// <summary>
        /// Конпка прехода на форму регистрации.
        /// </summary>
        private Button btn_reg_form;

        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        private Button btn_calculate;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.content_main, container, false);

            try
            {
                btn_auth_form = view.FindViewById<Button>(Resource.Id.btn_auth_form);
                btn_reg_form = view.FindViewById<Button>(Resource.Id.btn_reg_form);
                btn_calculate = view.FindViewById<Button>(Resource.Id.btn_calculate);

                // Переход к форме регистрации.
                btn_reg_form.Click += (s, e) =>
                {
                    
                };

                // Переход к форме регистрации.
                btn_calculate.Click += (s, e) =>
                {
                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    AddOrderActivity content = new AddOrderActivity();
                    transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                };

                // Переход к форме авторизация

                btn_auth_form.Click += (s, e) =>
                {
                    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                    AuthActivity content = new AuthActivity();
                    transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                };
            }
            catch(Exception ex)
            {
                Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }
    }
}