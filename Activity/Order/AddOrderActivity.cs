using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Entity.Model.HomeViewModel;
using Entity.Model.OrderResponse;
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using Entity.Repository;
using Google.Places;
using Plugin.Settings;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Activity.Registration;
using SmartBoxCity.Service;
using WebService;
using WebService.Account;
using WebService.Client;

namespace SmartBoxCity.Activity.Order
{
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class AddOrderActivity : Fragment
    {
        #region Переменные

        bool h_result, width_result, l_result, q_result, weight_result;

        double height, width, weight, length;

        int quantity;

        private EditText s_edit_from;

        private EditText s_edit_where;

        private EditText s_shipment_time;

        private EditText s_shipping_date;

        private EditText s_length;

        private EditText s_width;

        private EditText s_height;

        private EditText s_value;

        private EditText s_size;

        private EditText s_weight;

        private EditText s_sum_seats;

        private EditText s_declared_value_goods;

        private EditText s_order_cost;

        private EditText s_contact_person;

        private EditText s_phone;

        private EditText s_email_notifications;

        private EditText s_comment_order;

        private Spinner s_cargo_characteristic;

        private Spinner s_hazard_class;

        private Spinner s_loading_methods;

        private CheckBox chek_cargo_insurance;

        private CheckBox check_date;
        private string myCity;
        private CheckBox check_argue;
        private CheckBox check_receiver;
        private CheckBox check_opherta;

        private string a_cargo_characteristic;

        private string a_hazard_class;

        private string a_loading_methodsc;
        private bool sum_seats_result;
        private double sum_seats;
        #endregion
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RetainInstance = true;

            if (!PlacesApi.IsInitialized)
            {
                string key = GetString(Resource.String.google_key);
                PlacesApi.Initialize(Activity, key);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var view = inflater.Inflate(Resource.Layout.activity_application_processing, container, false);

                List<Place.Field> fields = new List<Place.Field>();

                fields.Add(Place.Field.Id);
                fields.Add(Place.Field.Name);
                fields.Add(Place.Field.LatLng);
                fields.Add(Place.Field.Address);



            
                #region Инициализаия переменных
                Button btn_make_request = view.FindViewById<Button>(Resource.Id.btn_make_request);
                s_edit_from = view.FindViewById<EditText>(Resource.Id.s_edit_from);
                s_edit_where = view.FindViewById<EditText>(Resource.Id.s_edit_where);
                s_shipment_time = view.FindViewById<EditText>(Resource.Id.s_shipment_time);
                s_shipping_date = view.FindViewById<EditText>(Resource.Id.s_shipping_date);
                s_length = view.FindViewById<EditText>(Resource.Id.s_length);
                s_width = view.FindViewById<EditText>(Resource.Id.s_width);
                s_weight = view.FindViewById<EditText>(Resource.Id.s_weight);
                s_height = view.FindViewById<EditText>(Resource.Id.s_height);
                s_size = view.FindViewById<EditText>(Resource.Id.s_size);
                s_sum_seats = view.FindViewById<EditText>(Resource.Id.s_sum_seats);
                s_contact_person = view.FindViewById<EditText>(Resource.Id.s_contact_person);
                s_value = view.FindViewById<EditText>(Resource.Id.s_value);
                s_cargo_characteristic = view.FindViewById<Spinner>(Resource.Id.s_cargo_characteristic);
                s_hazard_class = view.FindViewById<Spinner>(Resource.Id.s_hazard_class);
                s_loading_methods = view.FindViewById<Spinner>(Resource.Id.s_loading_methods);
                chek_cargo_insurance = view.FindViewById<CheckBox>(Resource.Id.chek_cargo_insurance);
                check_date = view.FindViewById<CheckBox>(Resource.Id.chek_date);
                check_argue = view.FindViewById<CheckBox>(Resource.Id.check_argue);
                check_receiver = view.FindViewById<CheckBox>(Resource.Id.chek_receiver);
                #endregion 
                ProgressBar preloader = view.FindViewById<ProgressBar>(Resource.Id.preloader);

                if (Arguments != null)
                {
                    string mParam = Arguments.GetString("isDestination");
                    if (mParam == "true")
                        s_edit_where.Text = StaticOrder.Destination_address;
                    else
                        s_edit_from.Text = StaticOrder.Inception_address;
                }
                s_shipment_time.Focusable = false;
                s_shipment_time.Enabled = false;
                s_shipment_time.Clickable = false;
                s_shipping_date.Focusable = false;
                s_shipping_date.Enabled = false;
                s_shipping_date.Clickable = false;
                s_value.Enabled = false;
                s_contact_person.Enabled = false;
                s_edit_from.Focusable = false;
                s_edit_where.Focusable = false;
                s_size.Focusable = false;

                #region Проверка отмеченных событий
                chek_cargo_insurance.CheckedChange += (o, e) => {
                    if (!chek_cargo_insurance.Checked)
                    {

                        s_value.Enabled = false;
                        s_value.Text = "";
                    }
                    else
                    {

                        s_value.Enabled = true;
                    }
                };

                check_date.CheckedChange += (o, e) =>
                {
                    if (!check_date.Checked)
                    {
                        s_shipping_date.Enabled = false;
                        s_shipment_time.Enabled = false;
                        s_shipment_time.Text = "";
                        s_shipping_date.Text = "";
                    }
                    else
                    {
                        s_shipping_date.Enabled = true;
                        s_shipment_time.Enabled = true;
                    }
                };

                s_shipping_date.Click += S_shipping_date_Click;
                s_shipment_time.Click += delegate
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.modal_user_time, null);
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetView(view);

                    #region Объявление переменных в диалоговом окне
                    var rb_first = view.FindViewById<RadioButton>(Resource.Id.rb_first);
                    var rb_second = view.FindViewById<RadioButton>(Resource.Id.rb_second);
                    var rb_third = view.FindViewById<RadioButton>(Resource.Id.rb_third);
                    var rb_vierth = view.FindViewById<RadioButton>(Resource.Id.rb_vierth);
                    var rb_fifth = view.FindViewById<RadioButton>(Resource.Id.rb_fifth);
                    var rb_sixth = view.FindViewById<RadioButton>(Resource.Id.rb_sixth);
                    #endregion

                    #region Обработка событий кнопок

                    rb_first.Click += delegate
                    {
                        s_shipment_time.Text = rb_first.Text;
                    };

                    rb_second.Click += delegate
                    {
                        s_shipment_time.Text = rb_second.Text;
                    };

                    rb_third.Click += delegate
                    {
                        s_shipment_time.Text = rb_third.Text;
                    };

                    rb_vierth.Click += delegate
                    {
                        s_shipment_time.Text = rb_vierth.Text;
                    };

                    rb_fifth.Click += delegate
                    {
                        s_shipment_time.Text = rb_fifth.Text;
                    };

                    rb_sixth.Click += delegate
                    {
                        s_shipment_time.Text = rb_sixth.Text;
                    };

                    #endregion

                    alert.SetCancelable(false)
                    .SetPositiveButton("Выбрать", delegate
                    {

                    })
                    .SetNegativeButton("Отмена", delegate
                    {
                        alert.Dispose();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                };

                check_argue.Click += async delegate
                {
                    if (check_argue.Checked == true)
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
                                check_argue.Checked = false;
                                alert.SetPositiveButton("Принимаю", (senderAlert, args) =>
                                {
                                    check_argue.Checked = true;
                                });
                                alert.SetNegativeButton("Не принимаю", (senderAlert, args) =>
                                {
                                    check_argue.Checked = false;
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

                check_receiver.CheckedChange += (o, e) =>
                {
                    if (!check_receiver.Checked)
                    {
                        s_contact_person.Enabled = false;
                        s_contact_person.Text = "";
                    }
                    else
                    {
                        s_contact_person.Enabled = true;
                    }

                };

                #endregion

                
                s_cargo_characteristic.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
                var adapter1 = ArrayAdapter.CreateFromResource(Activity, Resource.Array.array_cargo_characteristic, Android.Resource.Layout.SimpleSpinnerItem);
                adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                s_cargo_characteristic.Adapter = adapter1;

                s_hazard_class.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerClass_ItemSelected);
                var adapter2 = ArrayAdapter.CreateFromResource(Activity, Resource.Array.array_hazard_class, Android.Resource.Layout.SimpleSpinnerItem);
                adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                s_hazard_class.Adapter = adapter2;

                s_loading_methods.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerLoad_ItemSelected);
                var adapter3 = ArrayAdapter.CreateFromResource(Activity, Resource.Array.array_loading_methodsc, Android.Resource.Layout.SimpleSpinnerItem);
                adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                s_loading_methods.Adapter = adapter3;

                FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();

                s_size.Click += async delegate
                {
                    if (string.IsNullOrEmpty(s_length.Text) == false &&
                    string.IsNullOrEmpty(s_width.Text) == false &&
                    string.IsNullOrEmpty(s_height.Text) == false)
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        float size_calculation = float.Parse(s_length.Text, NumberStyles.Any, ci)
                            * float.Parse(s_width.Text, NumberStyles.Any, ci)
                            * float.Parse(s_height.Text, NumberStyles.Any, ci);
                        s_size.Text = size_calculation.ToString();
                    }
                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                        alert.SetTitle("Внимание!");
                        alert.SetMessage("Необходимо заполнить данные о длине, ширине и высоте груза !");
                        alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                        {
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }

                };

                s_edit_from.Click += async delegate
                {
                    //GooglePlacesResult fragment = new GooglePlacesResult();
                    //Bundle args = new Bundle();
                    //args.PutString("isDestination", "false");
                    //fragment.Arguments = args;

                    //transaction2.Replace(Resource.Id.framelayout, fragment).AddToBackStack(null).Commit();

                    Intent intent = new Autocomplete.IntentBuilder(AutocompleteActivityMode.Overlay, fields)
                .SetCountry("RUS")
                .Build(Activity);

                    myCity = "false";

                    StartActivityForResult(intent, 0);

                };


                s_edit_where.Click += async delegate
                {
                    Intent intent = new Autocomplete.IntentBuilder(AutocompleteActivityMode.Overlay, fields)
                .SetCountry("RUS")
                .Build(Activity);
                    myCity = "true";
                    StartActivityForResult(intent, 0);
                    //GooglePlacesResult fragment = new GooglePlacesResult();
                    //Bundle args = new Bundle();
                    //args.PutString("isDestination", "true");
                    //fragment.Arguments = args;
                    //transaction2.Replace(Resource.Id.framelayout, fragment).AddToBackStack(null).Commit();
                };

                //событие расчета стоимости заказа
                btn_make_request.Click += async delegate
                {
                    if (check_argue.Checked)
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        if (float.Parse(s_length.Text, NumberStyles.Any, ci) > 1.88 ||
                        float.Parse(s_width.Text, NumberStyles.Any, ci) > 2.59 ||
                        float.Parse(s_height.Text, NumberStyles.Any, ci) > 2.20)
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                            alert.SetTitle("Внимание!");
                            alert.SetMessage("Пожалуйста, проверьте введённые Вами значения длины, ширины и высоты груза!" +
                                "\n\nМакс. длина: 1.88 м\n\nМакс. ширина: 2.59 м\n\nМакс. высота: 2.20 м");
                            alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                            {
                            });
                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }
                        else if (s_size.Text == null)
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                            alert.SetTitle("Внимание!");
                            alert.SetMessage("Необходимо вычислить объём груза ! Для этого введите данные длины, ширины и высоты груза и нажмите на поле «Объём».");
                            alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                            {
                            });
                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }
                        else
                        {
                            preloader.Visibility = Android.Views.ViewStates.Visible;

                            //q_result = Int32.TryParse(s_sum_seats.Text, out quantity);
                            l_result = Double.TryParse(s_length.Text, out length);
                            width_result = Double.TryParse(s_width.Text, out width);
                            weight_result = Double.TryParse(s_weight.Text, out weight);
                            h_result = Double.TryParse(s_height.Text, out height);

                            if (weight < 0 || weight > 5000)
                            {
                                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                                alert.SetTitle("Внимание!");
                                alert.SetMessage("Пожалуйста, проверьте введённые Вами значения веса груза!" +
                                    "\n\nМакс. вес: 5000 кг");
                                alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                                {
                                });
                                Dialog dialog = alert.Create();
                                dialog.Show();

                            }
                            else
                            {
                                s_size.Text = (width * length * height).ToString();
                                MakeOrderModel model = new MakeOrderModel()
                                {
                                    destination_address = s_edit_where.Text,
                                    for_date = s_shipping_date.Text,
                                    for_time = s_shipment_time.Text,
                                    height = s_height.Text,
                                    inception_address = s_edit_from.Text,
                                    cargo_class = a_hazard_class,
                                    cargo_loading = a_loading_methodsc,
                                    cargo_type = a_cargo_characteristic,
                                    destination_lat = StaticOrder.Destination_lat,/*"47.232032",*/
                                    destination_lng = StaticOrder.Destination_lng,/*"39.731523",*/
                                    inception_lat = StaticOrder.Inception_lat,/*"47.243221",*/
                                    inception_lng = StaticOrder.Inception_lng,/*"39.668781",*/
                                    insurance = s_value.Text,
                                    receiver = s_contact_person.Text,
                                    length = s_length.Text,
                                    qty = s_sum_seats.Text,
                                    weight = s_weight.Text,
                                    width = s_width.Text
                                };


                                using (var client = ClientHelper.GetClient())
                                {
                                    OrderService.InitializeClient(client);
                                    var o_data = await OrderService.GetOrderPrice(model);

                                    if (o_data.Status == HttpStatusCode.OK)
                                    {
                                        //o_data.Message = "Успешно авторизован!";
                                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

                                        AmountResponse order_data = new AmountResponse();
                                        order_data = o_data.ResponseData;

                                        StaticOrder.AddInfoOrder(model);
                                        StaticOrder.AddInfoAmount(order_data);

                                        preloader.Visibility = Android.Views.ViewStates.Invisible;

                                        CrossSettings.Current.AddOrUpdateValue("isOrdered", "true");
                                        Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                                        ActivityOrderPreis content = new ActivityOrderPreis();
                                        transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                                    }
                                    else
                                    {
                                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                                    }
                                }
                            }



                        }

                    }
                    else
                    {
                        Toast.MakeText(Activity, "Необходимо дать согласие с договором офертой ", ToastLength.Long).Show();
                    }


                };

                s_weight.TextChanged += OnWieghtChanged;
                s_sum_seats.TextChanged += OnSumSeatsChanged;
                return view;
            }
            catch (Exception ex)
            {
                var view = inflater.Inflate(Resource.Layout.activity_errors_handling, container, false);
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                return view;
            }
           
        }

        private void OnSumSeatsChanged(object sender, TextChangedEventArgs e)
        {
            string messagif = "Максимальное кол-во мест: 5";
            string messagelse = "Введено неверное значение кол-во мест.";
            TryParseAndInputValidation(ref s_sum_seats, sum_seats_result, sum_seats, 5, messagif, messagelse);
        }       

        private void OnWieghtChanged(object sender, TextChangedEventArgs e)
        {
            string messagif = "Максимальный вес груза: 5000";
            string messagelse = "Введено неверное значение веса груза.";
            TryParseAndInputValidation(ref s_weight, weight_result, weight, 5000, messagif, messagelse);
        }
        private void TryParseAndInputValidation(ref EditText text, bool parse_result,
           double valueDouble, double maxValue, string messagif, string messagelse)
        {
            parse_result = Double.TryParse(text.Text, out valueDouble);
            if (parse_result)
            {
                if (valueDouble > maxValue)
                {
                    text.Text = maxValue.ToString();
                    Toast.MakeText(Activity, messagif, ToastLength.Long).Show();
                }
            }
            else 
            {
               switch(text.Text)
               {
                    case "0":
                        text.Text = "";
                        break;
                    case "00":
                        text.Text = "";
                        break;
                    case "000":
                        text.Text = "";
                        break;
                    case ".":
                        text.Text = "";
                        break;
               }
               Toast.MakeText(Activity, messagelse, ToastLength.Long).Show();
            }
            if(!(String.IsNullOrEmpty(text.Text)))
            {
                if(text.Text[0] == '.' || text.Text[0] == '0')
                {
                    text.Text = "";
                }
            }          
            else
            {
                Toast.MakeText(Activity, messagelse, ToastLength.Long).Show();
            }
        }

        //EventHandler< TextChangedEventArgs> OnTextChanged(object sender, EventArgs e)
        //{
        //    String val = s_weight.Text; //Get Current Text

        //    //if (val.Length > restrictCount)//If it is more than your character restriction
        //    //{
        //    //    val = val.Remove(val.Length - 1);// Remove Last character 
        //    //    entry.Text = val; //Set the Old value
        //    //}
        //}
        private void S_shipping_date_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                s_shipping_date.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        #region Спинеры
        private void SpinnerLoad_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            a_loading_methodsc = spinner.GetItemAtPosition(e.Position).ToString();
        }

        private void SpinnerClass_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            a_hazard_class = spinner.GetItemAtPosition(e.Position).ToString();
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            a_cargo_characteristic = spinner.GetItemAtPosition(e.Position).ToString();
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Toast.MakeText(Context, rb.Text, ToastLength.Short).Show();
        }
        #endregion
        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                var place = Autocomplete.GetPlaceFromIntent(data);

                if (myCity == "true")
                {
                    StaticOrder.Destination_address = place.Address;
                    StaticOrder.Destination_lat = place.LatLng.Latitude.ToString().Replace(",", ".");
                    StaticOrder.Destination_lng = place.LatLng.Longitude.ToString().Replace(",", ".");
                    s_edit_where.Text = place.Address;
                }
                else
                {
                    StaticOrder.Inception_address = place.Address;
                    StaticOrder.Inception_lat = place.LatLng.Latitude.ToString().Replace(",", ".");
                    StaticOrder.Inception_lng = place.LatLng.Longitude.ToString().Replace(",", ".");
                    s_edit_from.Text = place.Address;
                }



            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }

        }


        //public override void OnViewStateRestored(Bundle savedInstanceState)
        //{
        //    base.OnViewStateRestored(savedInstanceState);
        //    if(savedInstanceState != null)
        //    {
        //        s_edit_where.Text = savedInstanceState.GetString("s_edit_where");
        //        s_shipping_date.Text = savedInstanceState.GetString("s_shipping_date");
        //        s_shipment_time.Text = savedInstanceState.GetString("s_shipment_time");
        //        s_height.Text = savedInstanceState.GetString("s_height");
        //        a_hazard_class = savedInstanceState.GetString("a_hazard_class");
        //        a_loading_methodsc = savedInstanceState.GetString("a_loading_methodsc");
        //        a_cargo_characteristic = savedInstanceState.GetString("a_cargo_characteristic");
        //        StaticOrder.Destination_lat = savedInstanceState.GetString("Destination_lat");
        //        StaticOrder.Destination_lng = savedInstanceState.GetString("Destination_lng");
        //        StaticOrder.Inception_lat = savedInstanceState.GetString("Inception_lat");
        //        StaticOrder.Inception_lng = savedInstanceState.GetString("Inception_lng");
        //        s_value.Text = savedInstanceState.GetString("s_value");
        //        s_contact_person.Text = savedInstanceState.GetString("s_contact_person");
        //        s_length.Text = savedInstanceState.GetString("s_length");
        //        s_weight.Text = savedInstanceState.GetString("s_weight");
        //        s_sum_seats.Text = savedInstanceState.GetString("s_sum_seats");
        //        s_width.Text = savedInstanceState.GetString("s_width");
        //        s_size.Text = savedInstanceState.GetString("s_size");
        //    }           
        //}
        //public override void OnSaveInstanceState(Bundle outState)
        //{
        //    base.OnSaveInstanceState(outState);
        //    outState.PutString("s_edit_where", s_edit_where.Text);
        //    outState.PutString("s_shipping_date", s_shipping_date.Text);
        //    outState.PutString("s_shipment_time", s_shipment_time.Text);
        //    outState.PutString("s_height", s_height.Text);
        //    outState.PutString("a_hazard_class", a_hazard_class);
        //    outState.PutString("a_loading_methodsc", a_loading_methodsc);
        //    outState.PutString("a_cargo_characteristic", a_cargo_characteristic);
        //    outState.PutString("Destination_lat", StaticOrder.Destination_lat);
        //    outState.PutString("Destination_lng", StaticOrder.Destination_lng);
        //    outState.PutString("Inception_lat", StaticOrder.Inception_lat);
        //    outState.PutString("Inception_lng", StaticOrder.Inception_lng);
        //    outState.PutString("s_value", s_value.Text);
        //    outState.PutString("s_contact_person", s_contact_person.Text);
        //    outState.PutString("s_length", s_length.Text);
        //    outState.PutString("s_sum_seats", s_sum_seats.Text);
        //    outState.PutString("s_weight", s_weight.Text);
        //    outState.PutString("s_width", s_width.Text);
        //    outState.PutString("s_size", s_size.Text);
        //}

        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    var place = Autocomplete.GetPlaceFromIntent(data);

        //}

    }
}