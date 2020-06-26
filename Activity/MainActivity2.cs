using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Box;
using SmartBoxCity.Activity.Driver;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity
{
    [Activity(Label = "SmartBoxCity")]
    public class MainActivity2: AppCompatActivity
    {
        GPSService _gpsService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_driver);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation_driver);

            FragmentTransaction transaction3 = this.FragmentManager.BeginTransaction();
            MapActivity content2 = new MapActivity();
            transaction3.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null).Commit();

            navigation.NavigationItemSelected += (sender, e) =>
            {
                FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();
                switch (e.Item.ItemId)
                {
                    case Resource.Id.tasks:
                        MapActivity content2 = new MapActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content2).AddToBackStack(null).Commit();
                        break;
                    case Resource.Id.boxes:
                        MainBoxStatusActivity content = new MainBoxStatusActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                        break;
                    case Resource.Id.c_status:
                        ManageBoxActivity content3 = new ManageBoxActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content3).AddToBackStack(null).Commit();
                        break;
                    case Resource.Id.alarms:
                        StaticUser.NamePadeAbsenceSomething = "AlarmsActivity";
                        AlarmsActivity content4 = new AlarmsActivity();
                        transaction2.Replace(Resource.Id.frameDriverlayout, content4).AddToBackStack(null).Commit();
                        break;

                    case Resource.Id.exit_driver:
                        Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                        alert.SetTitle("Внимание!");
                        alert.SetMessage("Вы действительно хотите выйти ?");
                        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        {
                        });
                        alert.SetPositiveButton("Да", (senderAlert, args) =>
                        {
                            Leaveprofile();
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                       
                        break;
                }
            };

            
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_driver_layout);
           
        }

        public interface IBackButtonListener
        {
            void OnBackPressed();
        }
        public override void OnBackPressed()
        {
            if (StaticUser.PresenceOnPage == true)
            {

                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Внимание!");
                alert.SetMessage("Вы действительно хотите выйти со своего профиля ?");
                alert.SetPositiveButton("Да", (senderAlert, args) =>
                {
                    Leaveprofile();
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                var currentFragment = SupportFragmentManager.FindFragmentById(Resource.Id.framelayout);
                var listener = currentFragment as IBackButtonListener;
                if (listener != null)
                {
                    listener.OnBackPressed();
                    return;
                }
            }         
        }
        private void Leaveprofile()
        {
            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            File.Delete(dir_path + "user_data.txt");

            StaticUser.PresenceOnPage = false;
            CrossSettings.Current.AddOrUpdateValue("role", "");

            if (StaticDriver.busy == "0")
            {
                _gpsService = new GPSService(this);
                //_gpsService.UpdateLocation();
                _gpsService.RemoveLocation();
            }

            Intent content1 = new Intent(this, typeof(MainActivity));
            StartActivity(content1);
            this.Finish();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }        
       
    }
}