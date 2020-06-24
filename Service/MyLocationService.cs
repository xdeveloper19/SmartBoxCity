using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Repository;
using Plugin.Settings;
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Service
{
    [BroadcastReceiver]
    public class MyLocationService : BroadcastReceiver
    {
        public static string ACTION_PROCESS_LOCATION = "SmartBoxCity.UPDATE_LOCATION";
        GPSService _gpsService;
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent != null)
            {
                string action = intent.Action;
                if (action.Equals(ACTION_PROCESS_LOCATION))
                {
                    LocationResult result = LocationResult.ExtractResult(intent);
                    if (result != null)
                    {
                        var location = result.LastLocation;

                        try
                        {
                            //when app in foreground
                            if (!StaticTask.IsStoppedGeo)
                                PostGeoData(location);
                            else
                                _gpsService = new GPSService(context);
                                _gpsService.RemoveLocation();
                        }
                        catch (Exception ex)
                        {
                            //when app is killed
                        }
                    }

                }
            }
        }

        private async void PostGeoData(Location location)
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                GeoModel model = new GeoModel
                {
                    gps_time = DateTime.Now,
                    lat = location.Latitude.ToString().Replace(",","."),
                    lng = location.Longitude.ToString().Replace(",", ".")
                };

                DriverInfoService.InitializeClient(client);
                var o_data = new ServiceResponseObject<SuccessResponse>();
                o_data = await DriverInfoService.PostGeoData(model);

                Toast.MakeText(Application.Context, o_data.Message, ToastLength.Long).Show();
            }
        }
    }
}