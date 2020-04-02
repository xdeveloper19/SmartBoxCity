using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Google.Places;
using SmartBoxCity.Model.OrderViewModel;

namespace SmartBoxCity.Activity.Order
{
    public class GooglePlacesResult: Fragment
    {
        Button displayButton;
        EditText s_geo;
        private string myCity;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            if (!PlacesApi.IsInitialized)
            {
                string key = GetString(Resource.String.google_key);
                PlacesApi.Initialize(Context, key);
            }

            List<Place.Field> fields = new List<Place.Field>();

            fields.Add(Place.Field.Id);
            fields.Add(Place.Field.Name);
            fields.Add(Place.Field.LatLng);
            fields.Add(Place.Field.Address);


            Intent intent = new Autocomplete.IntentBuilder(AutocompleteActivityMode.Overlay, fields)
                .SetCountry("RUS")
                .Build(Context);

            StartActivityForResult(intent, 0);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_enter_location, container, false);
            displayButton = view.FindViewById<Button>(Resource.Id.btn_submit);
            s_geo = view.FindViewById<EditText>(Resource.Id.s_geo);
            if (Arguments != null)
            {
                myCity = Arguments.GetString("isDestination");
            }

            displayButton.Click += DisplayButton_Click;

            
            return view;
        }

        private void DisplayButton_Click(object sender, System.EventArgs e)
        {
            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
            AddOrderActivity fragment = new AddOrderActivity();
            Bundle args = new Bundle();
            args.PutString("isDestination", myCity);
            fragment.Arguments = args;
            transaction1.Replace(Resource.Id.framelayout, fragment).AddToBackStack(null).Commit();

        }

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                var place = Autocomplete.GetPlaceFromIntent(data);
                

                if (myCity == "true")
                {
                    StaticOrder.Destination_address = place.Address;
                    StaticOrder.Destination_lat = place.LatLng.Latitude.ToString();
                    StaticOrder.Destination_lng = place.LatLng.Longitude.ToString();
                }
                else
                {
                    StaticOrder.Inception_address = place.Address;
                    StaticOrder.Inception_lat = place.LatLng.Latitude.ToString();
                    StaticOrder.Inception_lng = place.LatLng.Longitude.ToString();
                }

                s_geo.Text = place.Address;
                
            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, ex.Message, ToastLength.Long).Show();
            }
           
        }
    }
}