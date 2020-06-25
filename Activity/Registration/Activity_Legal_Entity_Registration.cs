using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.AccountViewModel.AuthViewModel;
using Entity.Model.HomeViewModel;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Service;
using WebService;
using WebService.Account;

namespace SmartBoxCity.Activity.Registration
{
    public class Activity_Legal_Entity_Registration : Fragment
    {
        #region Переменные

        private EditText s_login;

        private EditText s_password;

        private EditText s_pass_confirmation;

        private EditText s_email;

        private EditText s_orgPostalAddress;

        private EditText s_clientLastName;

        private EditText s_clientName;

        private EditText s_clientPatronymic;

        private EditText s_orgPhone;

        private EditText s_orgName;

        private EditText s_orgKpp;

        private EditText s_orgInn;

        private EditText s_orgOgrn;

        private EditText s_orgBank;

        private EditText s_orgBankpayment;

        private EditText s_orgBankCorrespondent;

        private EditText s_orgBankBik;

        private EditText s_orgLegalAddress;

        private Button btn_make_request;

        private CheckBox check_personal_data_processing_entity;

        private CheckBox check_contract_oferta_entity;

        private ProgressBar preloader;

        #endregion
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_legal_entity_registration, container, false);

            #region Объявление переменных

            s_login = view.FindViewById<EditText>(Resource.Id.s_login_entity);
            s_password = view.FindViewById<EditText>(Resource.Id.s_pass_entity);
            s_pass_confirmation = view.FindViewById<EditText>(Resource.Id.s_pass_confirmation_entity);
            s_email = view.FindViewById<EditText>(Resource.Id.s_email_entity);
            s_orgPostalAddress = view.FindViewById<EditText>(Resource.Id.s_mailing_address_entity);
            s_clientLastName = view.FindViewById<EditText>(Resource.Id.s_lastname_entity);
            s_clientName = view.FindViewById<EditText>(Resource.Id.s_name_entity);
            s_clientPatronymic = view.FindViewById<EditText>(Resource.Id.s_patronymic_entity);
            s_orgPhone = view.FindViewById<EditText>(Resource.Id.s_phone_individual);
            s_orgName = view.FindViewById<EditText>(Resource.Id.s_nomination_entity);
            s_orgKpp = view.FindViewById<EditText>(Resource.Id.s_KPP_entity);
            s_orgInn = view.FindViewById<EditText>(Resource.Id.s_INN_entity);
            s_orgOgrn = view.FindViewById<EditText>(Resource.Id.s_OGRN_entity);
            s_orgBank = view.FindViewById<EditText>(Resource.Id.s_bank_entity);
            s_orgBankpayment = view.FindViewById<EditText>(Resource.Id.s_payment_account_entity);
            s_orgBankCorrespondent = view.FindViewById<EditText>(Resource.Id.s_correspondent_account_entity);
            s_orgBankBik = view.FindViewById<EditText>(Resource.Id.s_BIK_entity);
            s_orgLegalAddress = view.FindViewById<EditText>(Resource.Id.s_legal_address_entity);
            btn_make_request = view.FindViewById<Button>(Resource.Id.btn_make_request);
            check_personal_data_processing_entity = view.FindViewById<CheckBox>(Resource.Id.check_personal_data_processing_entity);
            check_contract_oferta_entity = view.FindViewById<CheckBox>(Resource.Id.check_contract_oferta_entity);
            preloader = view.FindViewById<ProgressBar>(Resource.Id.preloader);
            s_orgPhone.SetMaxLines(11);
            s_orgInn.SetMaxLines(9);
            s_orgKpp.SetMaxLines(9);
            s_orgOgrn.SetMaxLines(15);
            s_orgBank.SetMaxLines(15);
            s_orgBankpayment.SetMaxLines(20);
            s_orgBankCorrespondent.SetMaxLines(20);
            s_orgBankBik.SetMaxLines(9);

            #endregion

            check_personal_data_processing_entity.Click += async delegate
            {
                if (check_personal_data_processing_entity.Checked == true)
                {
                    using (var client = ClientHelper.GetClient())
                    {
                        WebService.Home.HomeService.InitializeClient(client);
                        var o_data = await WebService.Home.HomeService.Privacy();


                        if (o_data.Status == HttpStatusCode.OK)
                        {
                            AgreementResponse o_user_data = new AgreementResponse();
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
                                check_personal_data_processing_entity.Checked = false;
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

            check_contract_oferta_entity.Click += async delegate
            {
                if (check_contract_oferta_entity.Checked == true)
                {
                    using (var client = ClientHelper.GetClient())
                    {
                        WebService.Home.HomeService.InitializeClient(client);
                        var o_data = await WebService.Home.HomeService.Offer();

                        if (o_data.Status == HttpStatusCode.OK)
                        {
                            AgreementResponse o_user_data = new AgreementResponse();
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
                                check_contract_oferta_entity.Checked = false;
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
                if (CheckingOnNullOrEmptyOfStrings())
                {
                    Toast.MakeText(Activity, "Пожалуйста, заполните все поля. ", ToastLength.Long).Show();
                }
                else
                {

                    if (s_password.Text == s_pass_confirmation.Text)
                    {
                        if (check_contract_oferta_entity.Checked == true && check_personal_data_processing_entity.Checked == true)
                        {

                            RegisterLegalModel register = new RegisterLegalModel
                            {
                                Login = s_login.Text,
                                Password = s_password.Text,
                                Email = s_email.Text,
                                OrgPhone = s_orgPhone.Text,
                                ClientType = "organization",
                                ClientLastName = s_clientLastName.Text,
                                ClientName = s_clientName.Text,
                                ClientPatronymic = s_clientPatronymic.Text,
                                OrgPostalAddress = s_orgPostalAddress.Text,
                                OrgName = s_orgName.Text,
                                OrgKpp = s_orgKpp.Text,
                                OrgInn = s_orgInn.Text,
                                OrgOgrn = s_orgOgrn.Text,
                                OrgBank = s_orgBank.Text,
                                OrgBankpayment = s_orgBankpayment.Text,
                                OrgBankCorrespondent = s_orgBankCorrespondent.Text,
                                OrgBankBik = s_orgBankBik.Text,
                                OrgLegalAddress = s_orgLegalAddress.Text
                            };

                            using (var client = ClientHelper.GetClient())
                            {
                                AuthService.InitializeClient(client);
                                var o_data = await AuthService.RegisterLegal(register);

                                if (o_data.Status == HttpStatusCode.OK)
                                {
                                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                                    SuccessResponse o_user_data = new SuccessResponse();
                                    o_user_data = o_data.ResponseData;

                                    preloader.Visibility = Android.Views.ViewStates.Invisible;

                                    StaticUser.PresenceOnPage = true;
                                    CrossSettings.Current.AddOrUpdateValue("role", "client");
                                    CrossSettings.Current.AddOrUpdateValue("login", s_login.Text);
                                    CrossSettings.Current.AddOrUpdateValue("password", s_password.Text);
                                    CrossSettings.Current.AddOrUpdateValue("check", "0");
                                    StaticUser.NeedToCreateOrder = true;
                                    StaticUser.PresenceOnPage = true;

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
                }
            };

            return view;
        }

        private bool CheckingOnNullOrEmptyOfStrings()
        {
            if (String.IsNullOrEmpty(s_login.Text) || String.IsNullOrEmpty(s_password.Text)
              || String.IsNullOrEmpty(s_pass_confirmation.Text) || String.IsNullOrEmpty(s_email.Text)
              || String.IsNullOrEmpty(s_orgPostalAddress.Text) || String.IsNullOrEmpty(s_clientLastName.Text)
              || String.IsNullOrEmpty(s_clientName.Text) || String.IsNullOrEmpty(s_clientPatronymic.Text)
              || String.IsNullOrEmpty(s_orgPhone.Text) || String.IsNullOrEmpty(s_orgName.Text)
              || String.IsNullOrEmpty(s_orgKpp.Text) || String.IsNullOrEmpty(s_orgInn.Text)
              || String.IsNullOrEmpty(s_orgOgrn.Text) || String.IsNullOrEmpty(s_orgBank.Text)
              || String.IsNullOrEmpty(s_orgBankpayment.Text) || String.IsNullOrEmpty(s_orgBankCorrespondent.Text)
              || String.IsNullOrEmpty(s_orgBankBik.Text) || String.IsNullOrEmpty(s_orgLegalAddress.Text))
            {
                return true;
            }
            else
                return false;
        }
    }
}