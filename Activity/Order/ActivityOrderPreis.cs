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
using Plugin.Settings;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Activity.Registration;

namespace SmartBoxCity.Activity.Order
{
    class ActivityOrderPreis : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_order_price, container, false);
            Button btn_add_order3 = view.FindViewById<Button>(Resource.Id.btn_add_order3);
            btn_add_order3.Click += Btn_add_order3_Click;

            return view;
        }

        private void Btn_add_order3_Click(object sender, EventArgs e)
        {
            Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
            if (CrossSettings.Current.GetValueOrDefault("isAuth", "") == "true")
            {
                ActivityOrderPreis content = new ActivityOrderPreis();
                transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Context);
                alert.SetTitle("Внимание!");
                alert.SetMessage("Для оформления заказа необходимо авторизироваться или зарегистрироваться.");
                alert.SetPositiveButton("Регистрация", (senderAlert, args) =>
                {
                    alert.Dispose();
                    Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Context);
                    alert1.SetTitle("Внимание!");
                    alert1.SetMessage("Необходимо выбрать вид регистрации.");
                    alert1.SetPositiveButton("Для физ.лица", (senderAlert1, args1) =>
                    {
                        Activity_Registration_Individual_Person content4 = new Activity_Registration_Individual_Person();
                        transaction1.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
                    });
                    alert1.SetNegativeButton("Для юр.лица", (senderAlert1, args1) =>
                    {
                        Activity_Legal_Entity_Registration content3 = new Activity_Legal_Entity_Registration();
                        transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                    });
                    Dialog dialog1 = alert1.Create();
                    dialog1.Show();
                });
                alert.SetNegativeButton("Авторизация", (senderAlert, args) =>
                {
                    AuthActivity content3 = new AuthActivity();
                    transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            Toast.MakeText(Context, "Заявка оформлена", ToastLength.Long).Show();
        }
    }
}