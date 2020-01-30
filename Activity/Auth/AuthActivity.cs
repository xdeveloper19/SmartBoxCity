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
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.Settings;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Model;
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Repository;

namespace SmartBoxCity.Activity.Auth
{
    public class AuthActivity: Fragment
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
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

        /// <summary>
        /// Кнопка прокрутки.
        /// </summary>
        private ProgressBar preloader;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Toast.MakeText(Context, "Ваш статус: «Свободен для заказов»", ToastLength.Long).Show();

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_auth, container, false);

            btn_register = view.FindViewById<Button>(Resource.Id.btn_register);
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

            //        if (file_data_remember.Substring(0, 1) == "1")
            //        {
            //preloader.Visibility = Android.Views.ViewStates.Visible;
            //file_data_remember = file_data_remember.Remove(0, 1);
            //            AuthResponseData o_data = JsonConvert.DeserializeObject<AuthResponseData>(file_data_remember);

            //            StaticUser.AddInfoAuth(o_data);

            //preloader.Visibility = Android.Views.ViewStates.Gone;

            //// Переход на главную страницу.
            //Intent homeActivity = new Intent(this, typeof(Home.HomeActivity));
            //            StartActivity(homeActivity);
            //            //this.Finish();
            //        }
            //        else
            //        {
            // Переход к форме регистрации.
            btn_register.Click += (s, e) =>
            {
                //Intent registerActivity = new Intent(this, typeof(Auth.RegisterActivity));
                //StartActivity(registerActivity);
            };

            btn_auth.Click += async delegate
            {
                try
                {
                    preloader.Visibility = Android.Views.ViewStates.Visible;
                    // Авторизируюсь клиентом.

                    AuthModel auth = new AuthModel
                    {
                        Email = s_login.Text,
                        Password = s_pass.Text,
                    };



                    var myHttpClient = new HttpClient();
                    var _authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", auth.Email, auth.Password))));

                    myHttpClient.DefaultRequestHeaders.Authorization = _authHeader;

                    var uri = new Uri("http://iot.tmc-centert.ru/api/auth/login?email=" + auth.Email + "&password=" + auth.Password);
                    /*
                    //json структура.
                    var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "Email", auth.Email },
                        { "Password", auth.Password }
                    });
                    */
                    // Поучаю ответ об авторизации [успех или нет]
                    HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json"));

                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }

                    ServiceResponseObject<AuthResponseData> o_data = JsonConvert.DeserializeObject<ServiceResponseObject<AuthResponseData>>(s_result);


                    //ClearField();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (o_data.Status == "0")
                        {
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

                            StaticUser.Email = s_login.Text;
                            StaticUser.AddInfoAuth(o_user_data);

                            //пример ContainerSelection

                            //using (FileStream fs = new FileStream(dir_path + "user_data.txt", FileMode.OpenOrCreate))
                            //{
                            //    await System.Text.Json.JsonSerializer.SerializeAsync<AuthResponseData>(fs, o_user_data);
                            //}
                            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                            using (FileStream file = new FileStream(dir_path + "user_data.txt", FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                // преобразуем строку в байты
                                byte[] array = Encoding.Default.GetBytes(JsonConvert.SerializeObject(o_user_data));// 0 связан с запоминанием 
                                                                                                                   // запись массива байтов в файл
                                file.Write(array, 0, array.Length);
                            }

                            //var role = o_data.ResponseData.Role;
                            //Начинаю собирать информацию о клиенте
                            preloader.Visibility = Android.Views.ViewStates.Invisible;
                            // Переход на страницу водителя.
                            if (o_data.ResponseData.Role == "driver")
                            {
                                //Intent Driver = new Intent(this, typeof(Auth.DriverActivity));
                                //StartActivity(Driver);
                                //this.Finish();
                            }
                            else if (o_data.ResponseData.Role == "user")
                            {
                                //Intent UserActivity = new Intent(this, typeof(Auth.ActivityUserBox));
                                //StartActivity(UserActivity);
                                //this.Finish();
                            }
                            CrossSettings.Current.AddOrUpdateValue("isAuth", "true");
                            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            UserActivity content = new UserActivity();
                            transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                        }
                        else
                        {
                            Toast.MakeText(Context, o_data.Message, ToastLength.Long).Show();
                        }
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