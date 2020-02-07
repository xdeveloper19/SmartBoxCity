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
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Activity.Registration;

namespace SmartBoxCity.Activity.Home
{
    public class ContentMainActivity : Android.App.Fragment
    {     

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
                btn_calculate = view.FindViewById<Button>(Resource.Id.btn_calculate);


                // Переход к форме регистрации.
                btn_calculate.Click += (s, e) =>
                {
                    //set alert for executing the task
                    try
                    {
                        Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Внимание !");
                        alert.SetMessage("Для оформления заказа необходимо авторизироваться или зарегистрироваться.");
                        alert.SetPositiveButton("Регистрация", (senderAlert, args) =>
                        {
                            Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Android.App.Application.Context);
                            alert1.SetTitle("Внимание !");
                            alert1.SetMessage("Необходимо выбрать вид регистрации.");
                            Android.App.FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                            alert1.SetPositiveButton("Для физ.лица", (senderAlert1, args1) =>
                            {
                                Activity_Registration_Individual_Person content4 = new Activity_Registration_Individual_Person();
                                transaction2.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
                            });
                            alert1.SetNegativeButton("Для юр.лица", (senderAlert1, args1) =>
                            {
                                Activity_Legal_Entity_Registration content3 = new Activity_Legal_Entity_Registration();
                                transaction2.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
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
                    catch (Exception ex)
                    {
                        Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
                    }
                };

                // Переход к форме авторизация

            }
            catch(Exception ex)
            {
                Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }
    }
}