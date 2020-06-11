using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cheesebaron.SlidingUpPanel;
using Entity.Model;
using Entity.Model.OrderResponse;
using Entity.Repository;
using WebService.Client;

namespace SmartBoxCity.Activity.Order
{
    public class MapActivity: Fragment, IOnMapReadyCallback
    {
        private const string SavedStateActionBarHidden = "saved_state_action_bar_hidden";
        private TextView txtFrom;
        private TextView txtTo;
        private TextView Weight;
        private TextView LenhWidHeig;
        private GoogleMap GMap;
        MapView mMapView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;

            LatLng location = new LatLng(StaticOrder.way_points[0].lat, StaticOrder.way_points[0].lng);
            PolylineOptions rectOptions = new PolylineOptions()
            {

            };
            rectOptions.Geodesic(true);
            rectOptions.InvokeWidth(1);
            rectOptions.InvokeColor(Color.Blue);

            for (int i = 0; i < StaticOrder.way_points.Count; i++)
            {
                var latitude = StaticOrder.way_points[i].lat;
                var longitude = StaticOrder.way_points[i].lng;

                LatLng new_location = new LatLng(
                   latitude,
                    longitude);

                rectOptions.Add(new_location);

                if (i == 0)
                {
                    MarkerOptions markerOpt1 = new MarkerOptions();
                    //location = new LatLng(latitude, longitude);

                    markerOpt1.SetPosition(new LatLng(latitude, longitude));
                    markerOpt1.SetTitle("Start");
                    markerOpt1.SetSnippet("Текущее положение");

                    var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue);
                    markerOpt1.InvokeIcon(bmDescriptor);

                    googleMap.AddMarker(markerOpt1);

                    continue;
                }
                MarkerOptions markerOptions = new MarkerOptions();

                markerOptions.SetPosition(new_location);
                markerOptions.SetTitle(i.ToString());
                googleMap.AddMarker(markerOptions);

            }

            googleMap.AddPolyline(rectOptions);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(10);
            builder.Bearing(0);
            builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MoveCamera(cameraUpdate);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mMapView.OnSaveInstanceState(outState);
        }

        public override void OnResume()
        {
            mMapView.OnResume();
            base.OnResume();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            mMapView.OnDestroy();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mMapView.OnLowMemory();
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_map, container, false);

            txtFrom = view.FindViewById<TextView>(Resource.Id.MapTextFrom);
            txtTo = view.FindViewById<TextView>(Resource.Id.MapTextTo);
            Weight = view.FindViewById<TextView>(Resource.Id.MapTextWeight);
            LenhWidHeig = view.FindViewById<TextView>(Resource.Id.MapTextLenhWidHeig);

            var layout = view.FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_client_layout);
            view.FindViewById<TextView>(Resource.Id.txt_info_order_new).MovementMethod = new LinkMovementMethod();

            var result = GetParameters();

            if (result.Result == SmartBoxCity.Activity.Driver.TaskStatus.OK)
            {
                layout.AnchorPoint = 0.3f;
                layout.PanelExpanded += (s, e) => Log.Info(Tag, "PanelExpanded");
                layout.PanelCollapsed += (s, e) => Log.Info(Tag, "PanelCollapsed");
                layout.PanelAnchored += (s, e) => Log.Info(Tag, "PanelAnchored");
                layout.PanelSlide += (s, e) =>
                {
                    if (e.SlideOffset < 0.2)
                    {
                        //if (SupportActionBar.IsShowing)
                        //    SupportActionBar.Hide();
                    }
                    else
                    {
                        //if (!SupportActionBar.IsShowing)
                        //    SupportActionBar.Show();
                    }
                };

                var actionBarHidden = savedInstanceState != null &&
                                      savedInstanceState.GetBoolean(SavedStateActionBarHidden, false);
                //if (actionBarHidden)
                //    SupportActionBar.Hide();

                MapsInitializer.Initialize(Activity);
                mMapView = view.FindViewById<MapView>(Resource.Id.FragmentMapUser);

                switch (GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Activity))
                {
                    case ConnectionResult.Success:
                        Toast.MakeText(Activity, "SUCCESS", ToastLength.Long).Show();
                        mMapView.OnCreate(savedInstanceState);
                        mMapView.GetMapAsync(this);
                        break;
                    case ConnectionResult.ServiceMissing:
                        Toast.MakeText(Activity, "ServiceMissing", ToastLength.Long).Show();
                        break;
                    case ConnectionResult.ServiceVersionUpdateRequired:
                        Toast.MakeText(Activity, "Update", ToastLength.Long).Show();
                        break;
                    default:
                        Toast.MakeText(Activity, GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Activity), ToastLength.Long).Show();
                        break;
                }
            }

            return view;
        }

        private async Task<SmartBoxCity.Activity.Driver.TaskStatus> GetParameters()
        {
            var o_data = new ServiceResponseObject<GeoResponseData>();
            o_data = await OrderService.GeoOrder(StaticOrder.Order_id);

            if (o_data.Status == System.Net.HttpStatusCode.OK)
            {
                txtFrom.Text = o_data.ResponseData.ORDER.inception_address;
                txtTo.Text = o_data.ResponseData.ORDER.destination_address;
                Weight.Text = o_data.ResponseData.ORDER.weight;
                if (o_data.ResponseData.ORDER.length == null || o_data.ResponseData.ORDER.width == null || o_data.ResponseData.ORDER.height == null)
                {
                    LenhWidHeig.Text = "неизвестно";
                }
                else
                {
                    var length = double.Parse(o_data.ResponseData.ORDER.length, CultureInfo.InvariantCulture);
                    var width = double.Parse(o_data.ResponseData.ORDER.width, CultureInfo.InvariantCulture);
                    var height = double.Parse(o_data.ResponseData.ORDER.height, CultureInfo.InvariantCulture);
                    var sum = length.ToString() + "X" + width.ToString() + "X" + height.ToString();
                    LenhWidHeig.Text = sum;
                }
                var way_points = o_data.ResponseData.MAP_WAYPOINTS;
                StaticOrder.AddWayPoints(way_points);
                return SmartBoxCity.Activity.Driver.TaskStatus.OK;
            }
            return SmartBoxCity.Activity.Driver.TaskStatus.ServerError;
        }
    }
}