using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Gms.Location.Places;
using Android.Gms.Common.Apis;
using static Android.Gms.Common.Apis.GoogleApiClient;
using Android.Views;
using Android.Widget;
using Android.Gms.Common;
using Android.Util;
using Android.Gms.Location.Places.UI;
using Android.Support.V4.App;

namespace SmartBoxCity.Activity.Order
{
    [Activity(Label = "SmartBoxCity")]
    public class EnterLocationActivity : FragmentActivity /*IPlaceSelectionListener, IOnConnectionFailedListener, GoogleApiClient.IConnectionCallbacks, IResultCallback*/
    {
        private GoogleMap GMap;
        const int REQUEST_CODE_RESOLUTION = 3;
        const string TAG = "GPlaceExample";
        GoogleApiClient mGoogleApiClient;

    //    protected override void OnCreate(Bundle bundle)
    //    {
    //        base.OnCreate(bundle);
    //        SetContentView(Resource.Layout.activity_enter_location);
    //        // SetUpMap();


    //        mGoogleApiClient = new GoogleApiClient.Builder(this)
    //            .AddApi(PlacesClass.GEO_DATA_API)
    //            .AddApi(PlacesClass.PLACE_DETECTION_API)
    //            .EnableAutoManage(this, this).AddConnectionCallbacks(this)
    //            .AddOnConnectionFailedListener(OnConnectionFailed)
    //            .Build();

    //        PlaceAutocompleteFragment autocompleteFragment = (PlaceAutocompleteFragment)FragmentManager.FindFragmentById(Resource.Id.place_autocomplete_fragment);

    //        autocompleteFragment.SetOnPlaceSelectedListener(this);

    //        autocompleteFragment.SetBoundsBias(new LatLngBounds(
    //            new LatLng(4.5931, -74.1552),
    //            new LatLng(4.6559, -74.0837)));

    //    }

    //public void OnConnectionFailed(ConnectionResult result)
    //    {
    //        Log.Info("GPlaceExample", "GoogleApiClient connection failed: " + result);
    //        if (!result.HasResolution)
    //        {
    //            GoogleApiAvailability.Instance.GetErrorDialog(this, result.ErrorCode, 0).Show();
    //            return;
    //        }
    //        try
    //        {
    //            result.StartResolutionForResult(this, REQUEST_CODE_RESOLUTION);
    //        }
    //        catch (IntentSender.SendIntentException e)
    //        {
    //            Log.Error("GPlaceExample", "Exception while starting resolution activity", e);
    //        }
    //    }

    //    public void OnError(Statuses status)
    //    {
    //        Log.Info("xamarin", "An error occurred: " + status);
    //    }

    //    public void OnPlaceSelected(IPlace place)
    //    {
    //        var placeadd = place.AddressFormatted;
    //        var placename = place.NameFormatted;
    //        var namadd = placename + "," + placeadd;
    //        var latlng = place.LatLng;
    //        Log.Info("xamarin", "Place: " + place.NameFormatted);
    //    }

    //    public void OnConnected(Bundle connectionHint)
    //    {
    //        Log.Info(TAG, "Client connected.");
    //        //PlacesClass.GeoDataApi.GetPlaceById(mGoogleApiClient, placeId).SetResultCallback(this); 
    //    }

    //    public void OnConnectionSuspended(int cause)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnResult(Java.Lang.Object result)
    //    {
    //        Log.Info(TAG, result.ToString());
    //    }

        //public void OnMapReady(GoogleMap googleMap)
        //{
        //    this.GMap = googleMap;
        //    LatLng latlng = new LatLng(Convert.ToDouble(12.733027), Convert.ToDouble(77.83016));
        //    CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
        //    GMap.MoveCamera(camera);
        //    MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle("Hosur");
        //    GMap.AddMarker(options);
        //}



        //private void SetUpMap()
        //{
        //    if (GMap == null)
        //    {
        //        FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googlemap).GetMapAsync(this);
        //    }
        //}
    }
}