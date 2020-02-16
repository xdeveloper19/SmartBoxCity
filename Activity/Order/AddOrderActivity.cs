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

namespace SmartBoxCity.Activity.Order
{
    public class AddOrderActivity: Fragment
    {
        #region Переменные

        private EditText s_edit_from;

        private EditText s_edit_where;

        private EditText s_shipment_time;

        private EditText s_shipping_date;

        private EditText s_length;

        private EditText s_width;

        private EditText s_height;

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

        private string a_cargo_characteristic;

        private string a_hazard_class;

        private string a_loading_methodsc;
        #endregion
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_application_processing, container, false);

            try
            {
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
                //s_phone = view.FindViewById<EditText>(Resource.Id.s_phone);
                //s_email_notifications = view.FindViewById<EditText>(Resource.Id.s_email_notifications);
                //s_comment_order = view.FindViewById<EditText>(Resource.Id.s_comment_order);
                s_cargo_characteristic = view.FindViewById<Spinner>(Resource.Id.s_cargo_characteristic);
                s_hazard_class = view.FindViewById<Spinner>(Resource.Id.s_hazard_class);
                s_loading_methods = view.FindViewById<Spinner>(Resource.Id.s_loading_methods);
                chek_cargo_insurance = view.FindViewById<CheckBox>(Resource.Id.chek_cargo_insurance);


                ProgressBar preloader = view.FindViewById<ProgressBar>(Resource.Id.preloader);

                chek_cargo_insurance.Click += (o, e) => {

                };

                s_cargo_characteristic.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
                var adapter1 = ArrayAdapter.CreateFromResource(Context, Resource.Array.array_cargo_characteristic, Android.Resource.Layout.SimpleSpinnerItem);

                s_hazard_class.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
                var adapter2 = ArrayAdapter.CreateFromResource(Context, Resource.Array.array_hazard_class, Android.Resource.Layout.SimpleSpinnerItem);

                s_loading_methods.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
                var adapter3 = ArrayAdapter.CreateFromResource(Context, Resource.Array.array_loading_methodsc, Android.Resource.Layout.SimpleSpinnerItem);


                btn_make_request.Click += async delegate
                {
                    Toast.MakeText(Context, "Заявка оформлена", ToastLength.Long).Show();
                    //Intent ActivityUserBox = new Intent(this, typeof(ActivityUserBox));
                    //StartActivity(ActivityUserBox);
                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
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

    }
}