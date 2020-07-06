using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Entity.Repository;

namespace SmartBoxCity.Service
{
    [Service]
    public class GPSService 
    {
        private LocationRequest locationRequest;
        private FusedLocationProviderClient providerClient;
        private Context context;

        public GPSService(Context _context)
        {
            context = _context;
        }

        public void UpdateLocation()
        {
            BuildLocationRequest();

            providerClient = LocationServices.GetFusedLocationProviderClient(context);
            if (Android.Support.V4.App.ActivityCompat.CheckSelfPermission(context, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted)
                return;
            providerClient.RequestLocationUpdates(locationRequest, GetPendingIntent());
        }

        private PendingIntent GetPendingIntent()
        {
            Intent intent = new Intent(context, typeof(MyLocationBroadcastReceiver));
            intent.SetAction(MyLocationBroadcastReceiver.ACTION_PROCESS_LOCATION);
            return PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        private void BuildLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(8000);
            locationRequest.SetFastestInterval(6000);
            locationRequest.SetSmallestDisplacement(10f);
        }

        public void RemoveLocation()
        {
            UpdateLocation();
            providerClient.RemoveLocationUpdates(GetPendingIntent());
        }
    }
}