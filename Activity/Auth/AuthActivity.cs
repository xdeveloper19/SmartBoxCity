using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.Settings;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Activity.Registration;
using SmartBoxCity.Model;
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Repository;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Auth
{
    public class AuthActivity: Android.App.Fragment
    {
        /// <summary>
        /// Почта клиента
        /// </summary>
        private EditText s_login;

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        private EditText s_pass;

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        private CheckBox is_remember;

        /// <summary>
        /// Конпка регистрации.
        /// </summary>
        private Button btn_register;

        /// <summary>
        /// Конпка авторизации.
        /// </summary>
        private Button btn_auth;

        /// <summary>
        /// Кнопка прокрутки.
        /// </summary>
        private ProgressBar preloader;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_auth, container, false);

            //btn_register = view.FindViewById<Button>(Resource.Id.btn_register);
            btn_auth = view.FindViewById<Button>(Resource.Id.btn_auth);
            //btn_back_a = view.FindViewById<ImageButton>(Resource.Id.btn_back_a);

            s_login = view.FindViewById<EditText>(Resource.Id.s_login);
            s_pass = view.FindViewById<EditText>(Resource.Id.s_pass);
            is_remember = view.FindViewById<CheckBox>(Resource.Id.is_remember);
            preloader = view.FindViewById<ProgressBar>(Resource.Id.loader);

            is_remember.Checked = true;

            string file_data_remember = "";
            // Проверяю запомнил ли пользователя.
            string check = CrossSettings.Current.GetValueOrDefault("check", "");

            if (check == "1")
            {
                s_login.Text = CrossSettings.Current.GetValueOrDefault("login", "");
                s_pass.Text = CrossSettings.Current.GetValueOrDefault("password", "");
            }

          
            //btn_register.Click += (s, e) =>
            //{
            //    Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
            //    AlertDialog.Builder alert = new AlertDialog.Builder(Context);
            //    alert.SetTitle("Внимание!");
            //    alert.SetMessage("Для оформления заказа необходимо авторизироваться или зарегистрироваться.");
            //    alert.SetPositiveButton("Регистрация", (senderAlert, args) =>
            //    {
            //        alert.Dispose();
            //        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(Context);
            //        alert1.SetTitle("Внимание!");
            //        alert1.SetMessage("Необходимо выбрать вид регистрации.");
            //        alert1.SetPositiveButton("Для физ.лица", (senderAlert1, args1) =>
            //        {
            //            Activity_Registration_Individual_Person content4 = new Activity_Registration_Individual_Person();
            //            transaction1.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
            //        });
            //        alert1.SetNegativeButton("Для юр.лица", (senderAlert1, args1) =>
            //        {
            //            Activity_Legal_Entity_Registration content3 = new Activity_Legal_Entity_Registration();
            //            transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
            //        });
            //        Dialog dialog1 = alert1.Create();
            //        dialog1.Show();
            //    });
            //    alert.SetNegativeButton("Авторизация", (senderAlert, args) =>
            //    {
            //        AuthActivity content3 = new AuthActivity();
            //        transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
            //    });
            //    Dialog dialog = alert.Create();
            //    //dialog.Show();
            //};

            btn_auth.Click += async delegate
            {
                try
                {
                    preloader.Visibility = Android.Views.ViewStates.Visible;
                    // Авторизируюсь клиентом.

                    AuthModel auth = new AuthModel
                    {
                        Login = s_login.Text,
                        Password = s_pass.Text
                    };

                    using (var client = ClientHelper.GetClient(auth.Login, auth.Password))
                    {
                        AuthService.InitializeClient(client);
                        var o_data = await AuthService.Login(auth);
                        
                        if (o_data.Status == HttpStatusCode.OK)
                        {
                            //o_data.Message = "Успешно авторизован!";
                            Toast.MakeText(Context, o_data.Message, ToastLength.Long).Show();
                             AuthResponseData o_user_data = new AuthResponseData();
                             o_user_data = o_data.ResponseData;

                             if (is_remember.Checked == true)
                             {
                                 CrossSettings.Current.AddOrUpdateValue("check", "1");
                                 CrossSettings.Current.AddOrUpdateValue("login", s_login.Text);
                                 CrossSettings.Current.AddOrUpdateValue("password", s_pass.Text);
                             }
                             else
                             {
                                 CrossSettings.Current.AddOrUpdateValue("check", "0");
                             }

                            //StaticUser.Email = s_login.Text;
                            //StaticUser.AddInfoAuth(o_user_data);


                            preloader.Visibility = Android.Views.ViewStates.Invisible;
                            CrossSettings.Current.AddOrUpdateValue("isAuth", "true");
                            CrossSettings.Current.AddOrUpdateValue("token", o_user_data.Token);
                            CrossSettings.Current.AddOrUpdateValue("role", o_user_data.Role);
                            Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            Intent main = new Intent(Context, typeof(MainActivity));
                            StartActivity(main);
                        }
                        else
                        {
                            Toast.MakeText(Context, o_data.Message, ToastLength.Long).Show();
                        }
                    }

                    using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token","")))
                    {
                        AuthService.InitializeClient(client);
                        await AuthService.Login(CrossSettings.Current.GetValueOrDefault("token", ""));
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            return view;
        }

        /// <summary>
        /// Метод очистки полей.
        /// </summary>
        void ClearField()
        {
            s_login.Text = "";
            s_pass.Text = "";
        }
    }
}