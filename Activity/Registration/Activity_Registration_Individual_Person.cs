using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Registration
{
    public class Activity_Registration_Individual_Person : Fragment
    {
        #region Переменные

        private EditText s_email_individual;

        private EditText s_phone_individual;

        private EditText s_login_individual;

        private EditText s_pass_individual;

        private EditText s_pass_confirmation_individual;

        private EditText s_surname_individual;

        private EditText s_name_individual;

        private EditText s_patronymic_individual;

        private EditText s_passport_series_individual;

        private EditText s_passport_number_individual;

        private EditText s_date_birth_individual;

        private EditText s_department_code_individual;

        private Button btn_make_request;

        private CheckBox check_personal_data_processing_individual;

        private CheckBox check_contract_oferta_individual;

        private ProgressBar preloader;

        #endregion
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_registration_individual_person, container, false);
            #region Объявление переменных

            s_email_individual = view.FindViewById<EditText>(Resource.Id.s_email_individual);
            s_phone_individual = view.FindViewById<EditText>(Resource.Id.s_phone_individual);
            s_login_individual = view.FindViewById<EditText>(Resource.Id.s_login_individual);
            s_pass_individual = view.FindViewById<EditText>(Resource.Id.s_pass_individual);
            s_pass_confirmation_individual = view.FindViewById<EditText>(Resource.Id.s_pass_confirmation_individual);
            s_surname_individual = view.FindViewById<EditText>(Resource.Id.s_surname_individual);
            s_name_individual = view.FindViewById<EditText>(Resource.Id.s_name_individual);
            s_patronymic_individual = view.FindViewById<EditText>(Resource.Id.s_patronymic_individual);
            s_passport_series_individual = view.FindViewById<EditText>(Resource.Id.s_passport_series_individual);
            s_passport_number_individual = view.FindViewById<EditText>(Resource.Id.s_passport_number_individual);
            s_date_birth_individual = view.FindViewById<EditText>(Resource.Id.s_date_birth_individual);
            s_department_code_individual = view.FindViewById<EditText>(Resource.Id.s_department_code_individual);
            btn_make_request = view.FindViewById<Button>(Resource.Id.btn_make_request);
            check_personal_data_processing_individual = view.FindViewById<CheckBox>(Resource.Id.check_personal_data_processing_individual);
            check_contract_oferta_individual = view.FindViewById<CheckBox>(Resource.Id.check_contract_oferta_individual);
            preloader = view.FindViewById<ProgressBar>(Resource.Id.preloader);

            #endregion

            check_personal_data_processing_individual.Click += async delegate
            {
                if (check_personal_data_processing_individual.Checked == true)
                {
                    using (var client = ClientHelper.GetClient())
                    {
                        AuthService.InitializeClient(client);
                        var o_data = await AuthService.Privacy();
                        

                        if (o_data.Status == HttpStatusCode.OK)
                        {
                            AgreementResponseData o_user_data = new AgreementResponseData();
                            o_user_data = o_data.ResponseData;

                            Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                            alert.SetTitle("Согласие на обработку персональных данных");
                            alert.SetMessage(o_user_data.Agreement);

                            alert.SetPositiveButton("Принимаю", (senderAlert, args) =>
                            {
                            });
                            alert.SetNegativeButton("Не принимаю", (senderAlert, args) =>
                            {
                                check_personal_data_processing_individual.Checked = false;
                            });
                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }
                        else
                        {
                            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                        }
                    };
                }
            };

            check_contract_oferta_individual.Click += async delegate
            {
                if (check_contract_oferta_individual.Checked == true)
                {
                    using(var client = ClientHelper.GetClient())
                    {
                        AuthService.InitializeClient(client);
                        var o_data = await AuthService.Offer();
                        
                        if (o_data.Status == HttpStatusCode.OK)
                        {
                            AgreementResponseData o_user_data = new AgreementResponseData();
                            o_user_data = o_data.ResponseData;

                            Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                            alert.SetTitle("Согласие с договором офертой");
                            alert.SetMessage(o_user_data.Agreement);

                            alert.SetPositiveButton("Принимаю", (senderAlert, args) =>
                            {

                            });
                            alert.SetNegativeButton("Не принимаю", (senderAlert, args) =>
                            {
                                check_contract_oferta_individual.Checked = false;
                            });
                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }
                        else
                        {
                            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                        }
                    };
                    
                }
            };
            
            btn_make_request.Click += async delegate
            {
                if (s_pass_individual.Text == s_pass_confirmation_individual.Text)
                {
                    if (check_contract_oferta_individual.Checked == true && check_personal_data_processing_individual.Checked == true)
                    {
                        RegisterIndividualModel register = new RegisterIndividualModel
                        {
                            Login = s_login_individual.Text,
                            Password = s_pass_individual.Text,
                            Email = s_email_individual.Text,
                            Phone = s_phone_individual.Text,
                            ClientType = "person",
                            ClientLastName = s_surname_individual.Text,
                            ClientName = s_name_individual.Text,
                            ClientPatronymic = s_patronymic_individual.Text,
                            ClientBirthday = s_date_birth_individual.Text,
                            ClientPassportSerie = s_passport_series_individual.Text,
                            ClientPassportId = s_passport_number_individual.Text,
                            ClientPassportCode = s_department_code_individual.Text
                        };

                        using (var client = ClientHelper.GetClient())
                        {
                            AuthService.InitializeClient(client);
                            var o_data = await AuthService.RegisterIndividual(register);

                            if (o_data.Status == HttpStatusCode.OK)
                            {
                                //o_data.Message = "Успешно авторизован!";
                                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                                RegisterResponseData o_user_data = new RegisterResponseData();
                                o_user_data = o_data.ResponseData;

                                preloader.Visibility = Android.Views.ViewStates.Invisible;
                                CrossSettings.Current.AddOrUpdateValue("isAuth", "true");

                                CrossSettings.Current.AddOrUpdateValue("role", "user");
                                Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                                Intent main = new Intent(Activity, typeof(MainActivity));
                                StartActivity(main);
                            }
                            else
                            {
                                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                            }
                        };
                    }
                    else
                    {
                        Toast.MakeText(Activity, "Необходимо дать согласие на обработку " +
                            "персональных данных и согласиться с договором офертой", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(Activity, "Пароли не совпадают ", ToastLength.Long).Show();
                }
                              
            };
            return view;
        }
    }
}