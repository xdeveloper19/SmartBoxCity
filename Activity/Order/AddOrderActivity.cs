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
using Google.Places;
using Plugin.Settings;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Activity.Registration;
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Model.OrderViewModel;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Order
{
    public class AddOrderActivity: Fragment
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
        #endregion
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!PlacesApi.IsInitialized)
            {
                string key = GetString(Resource.String.google_key);
                PlacesApi.Initialize(Context, key);
            }

            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_application_processing, container, false);

            List<Place.Field> fields = new List<Place.Field>();

            fields.Add(Place.Field.Id);
            fields.Add(Place.Field.Name);
            fields.Add(Place.Field.LatLng);
            fields.Add(Place.Field.Address);


            
            try
            {
                #region Поиск элементов
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

                s_value.Enabled = false;
                s_shipping_date.Enabled = false;
                s_shipment_time.Enabled = false;
                s_contact_person.Enabled = false;
                s_edit_from.Focusable = false;
                s_edit_where.Focusable = false;

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
                s_shipment_time.Click += S_shipment_time_Click;

                check_argue.Click += async delegate
                {
                    if (check_argue.Checked == true)
                    {
                        using (var client = ClientHelper.GetClient())
                        {
                            AuthService.InitializeClient(client);
                            var o_data = await AuthService.Offer();

                            if (o_data.Status == HttpStatusCode.OK)
                            {
                                AgreementResponseData o_user_data = new AgreementResponseData();
                                o_user_data = o_data.ResponseData;

                                Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                                AlertDialog.Builder alert = new AlertDialog.Builder(Context);
                                alert.SetTitle("Согласие с договором офертой");
                                alert.SetMessage(o_user_data.Agreement);

                                alert.SetPositiveButton("Принимаю", (senderAlert, args) =>
                                {

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
                                Toast.MakeText(Context, o_data.Message, ToastLength.Long).Show();
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
                var adapter1 = ArrayAdapter.CreateFromResource(Context, Resource.Array.array_cargo_characteristic, Android.Resource.Layout.SimpleSpinnerItem);
                adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                s_cargo_characteristic.Adapter = adapter1;

                s_hazard_class.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerClass_ItemSelected);
                var adapter2 = ArrayAdapter.CreateFromResource(Context, Resource.Array.array_hazard_class, Android.Resource.Layout.SimpleSpinnerItem);
                adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                s_hazard_class.Adapter = adapter2;

                s_loading_methods.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SpinnerLoad_ItemSelected);
                var adapter3 = ArrayAdapter.CreateFromResource(Context, Resource.Array.array_loading_methodsc, Android.Resource.Layout.SimpleSpinnerItem);
                adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                s_loading_methods.Adapter = adapter3;

                FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                
                s_edit_from.Click += async delegate
                {
                    //GooglePlacesResult fragment = new GooglePlacesResult();
                    //Bundle args = new Bundle();
                    //args.PutString("isDestination", "false");
                    //fragment.Arguments = args;

                    //transaction2.Replace(Resource.Id.framelayout, fragment).AddToBackStack(null).Commit();

                    Intent intent = new Autocomplete.IntentBuilder(AutocompleteActivityMode.Overlay, fields)
                .SetCountry("RUS")
                .Build(Context);

                    myCity = "false";

                    StartActivityForResult(intent, 0);

                };

                
                s_edit_where.Click += async delegate
                {
                    Intent intent = new Autocomplete.IntentBuilder(AutocompleteActivityMode.Overlay, fields)
                .SetCountry("RUS")
                .Build(Context);
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
                    preloader.Visibility = Android.Views.ViewStates.Visible;
                    //q_result = Int32.TryParse(s_sum_seats.Text, out quantity);
                    //l_result = Double.TryParse(s_length.Text, out length);
                    //width_result = Double.TryParse(s_width.Text, out width);
                    //weight_result = Double.TryParse(s_weight.Text, out weight);
                    //h_result = Double.TryParse(s_height.Text, out height);

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
                            Toast.MakeText(Context, o_data.Message, ToastLength.Long).Show();

                            AmountResponse order_data = new AmountResponse();
                            order_data = o_data.ResponseData;

                            StaticOrder.AddInfoOrder(model);
                            StaticOrder.AddInfoAmount(order_data);

                            preloader.Visibility = Android.Views.ViewStates.Invisible;

                            CrossSettings.Current.AddOrUpdateValue("isOrdered","true");
                            Android.App.FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                            ActivityOrderPreis content = new ActivityOrderPreis();
                            transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                        }
                        else
                        {
                            Toast.MakeText(Context, o_data.Message, ToastLength.Long).Show();
                        }
                    }
                    
                    
                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }

        private void S_shipment_time_Click(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(
        delegate (DateTime time)
        {
            s_shipment_time.Text = time.ToShortTimeString();
        });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

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
                    StaticOrder.Destination_lat = place.LatLng.Latitude.ToString().Replace(",",".");
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
                Toast.MakeText(Context, ex.Message, ToastLength.Long).Show();
            }

        }

        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    var place = Autocomplete.GetPlaceFromIntent(data);

        //}

        void ClearField()
        {
            a_cargo_characteristic = "";
            a_hazard_class = "";
            a_loading_methodsc = "";
            s_edit_from.Text = "";
            s_edit_where.Text = "";
            s_shipment_time.Text = "";
            s_shipping_date.Text = "";
            s_length.Text = "";
            s_width.Text = "";
            s_weight.Text = "";
            s_height.Text = "";
            s_size.Text = "";
            s_sum_seats.Text = "";
            s_contact_person.Text = "";
            s_value.Text = "";
            chek_cargo_insurance.Checked = false;
            check_date.Checked = false;
            check_argue.Checked = false;
            check_receiver.Checked = false;
        }

    }
}