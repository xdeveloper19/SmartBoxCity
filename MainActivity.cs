using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using System.IO;
using Android.Views;
using Android.Widget;
using Com.Karumi.Dexter;
using Com.Karumi.Dexter.Listener;
using Com.Karumi.Dexter.Listener.Multi;
using Plugin.Settings;
using SmartBoxCity.Activity;
using SmartBoxCity.Activity.Auth;
using SmartBoxCity.Activity.Home;
using SmartBoxCity.Activity.Order;
using System.Text;
using Newtonsoft.Json;
using SmartBoxCity.Activity.Menu;
using SmartBoxCity.Activity.Registration;
using Android.Graphics.Drawables;
using SmartBoxCity.Service;
using WebService;
using WebService.Account;
using Entity.Model.AccountViewModel.AuthViewModel;
using System.Net;
using Java.Lang;
using Entity.Repository;

namespace SmartBoxCity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, 
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity 
    {
        private int MY_PERMISSIONS_REQUEST_CAMERA = 100;
        private FragmentTransaction transaction1;
        private IMenuItem btnAddOrder;
        private IMenuItem btnOrders;
        private IMenuItem btnExit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {                
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                SetContentView(Resource.Layout.activity_main);
                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);

                string[] permissions = { Manifest.Permission.AccessFineLocation, Manifest.Permission.WriteExternalStorage };
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, MY_PERMISSIONS_REQUEST_CAMERA);
                Dexter.WithActivity(this).WithPermissions(permissions).WithListener(new CompositeMultiplePermissionsListener(new SamplePermissionListener(this))).Check();

                string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);

                transaction1 = this.FragmentManager.BeginTransaction();
                btnAddOrder = navigation.Menu.FindItem(Resource.Id.title_about_us);
                btnOrders = navigation.Menu.FindItem(Resource.Id.title_reviews);
                btnExit = navigation.Menu.FindItem(Resource.Id.title_contacts);

                //AuthorizationAndReceivingToken();
                if (StaticUser.PresenceOnPage == true)
                {
                    if (CrossSettings.Current.GetValueOrDefault("role", "") == "driver")
                    {
                        Intent intent = new Intent(this, typeof(Activity.MainActivity2));
                        StartActivity(intent);
                        this.Finish();
                    }
                    else
                    {
                        try
                        {
                            UserActivity content2 = new UserActivity();
                            transaction1.Replace(Resource.Id.framelayout, content2).Commit();
                        }
                        catch (System.Exception ex)
                        {
                            Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        }

                        btnAddOrder.SetTitle("Заказать");
                        btnAddOrder.SetIcon(Resource.Drawable.ic_add_order);

                        btnOrders.SetTitle("Заказы");
                        btnOrders.SetIcon(Resource.Drawable.ic_orders);

                        btnExit.SetTitle("Выход");
                        btnExit.SetIcon(Resource.Drawable.ic_menu_exit);

                    }

                }
                else
                {
                    ContentMainActivity content = new ContentMainActivity();
                    transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();

                    btnAddOrder.SetTitle("О нас");
                    btnAddOrder.SetIcon(Resource.Drawable.ic_dashboard_black_24dp);

                    btnOrders.SetTitle("Отзывы");
                    btnOrders.SetIcon(Resource.Drawable.ic_notifications_black_24dp);

                    btnExit.SetTitle("Контакты");
                    btnExit.SetIcon(Resource.Drawable.ic_information);

                }

                navigation.NavigationItemSelected += async (sender, e) =>
                {

                    FragmentTransaction transaction2 = this.FragmentManager.BeginTransaction();

                    switch (e.Item.ItemId)
                    {

                        case Resource.Id.navigation_home:
                            if (StaticUser.PresenceOnPage == true)
                            {
                                UserActivity content2 = new UserActivity();
                                transaction2.Replace(Resource.Id.framelayout, content2).AddToBackStack(null).Commit();
                            }
                            else
                            {
                                ContentMainActivity content = new ContentMainActivity();
                                transaction2.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                            }


                            break;
                        case Resource.Id.title_about_us:
                            if (StaticUser.PresenceOnPage == true)
                            {
                                AddOrderActivity content = new AddOrderActivity();
                                transaction2.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                            }
                            else
                            {
                                Activity_About_As content3 = new Activity_About_As();
                                transaction2.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
                                Toast.MakeText(this, "Страница: О нас.", ToastLength.Long).Show();
                            }

                            break;
                        case Resource.Id.title_reviews:
                            if (StaticUser.PresenceOnPage == true)
                            {
                                ListOrdersActivity content1 = new ListOrdersActivity();
                                transaction2.Replace(Resource.Id.framelayout, content1).AddToBackStack(null).Commit();
                            }
                            else
                            {
                                Activity_Reviews content5 = new Activity_Reviews();
                                transaction2.Replace(Resource.Id.framelayout, content5).AddToBackStack(null).Commit();
                                Toast.MakeText(this, "Страница: Отзывы.", ToastLength.Long).Show();
                            }


                            break;
                        case Resource.Id.title_contacts:
                            if (StaticUser.PresenceOnPage == true)
                            {
                                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                                alert.SetTitle("Внимание!");
                                alert.SetMessage("Вы действительно хотите выйти ?");
                                alert.SetPositiveButton("Да", (senderAlert, args) =>
                                {
                                    string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                                    File.Delete(dir_path + "user_data.txt");

                                    using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                                    {
                                        AuthService.InitializeClient(client);
                                        AuthService.LogOut();
                                        CrossSettings.Current.AddOrUpdateValue("token", "");
                                        StaticUser.PresenceOnPage = false;
                                    }

                                    Intent content = new Intent(this, typeof(MainActivity));
                                    StartActivity(content);
                                });
                                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                                {
                                });
                                Dialog dialog = alert.Create();
                                dialog.Show();
                            }
                            else
                            {
                                Activity_List_Contacts content4 = new Activity_List_Contacts();
                                transaction2.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
                                Toast.MakeText(this, "Страница: Контакты.", ToastLength.Long).Show();
                            }
                            break;
                    }
                };



                DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
            
            //ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            //drawer.AddDrawerListener(toggle);
            //toggle.SyncState();
            //NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);// !
            //var account = navigationView.Menu.FindItem(Resource.Id.nav_auth);
            //var exit1 = navigationView.Menu.FindItem(Resource.Id.nav_exit);
            //if (CrossSettings.Current.GetValueOrDefault("isAuth","") == "true")
            //{
            //    UserActivity content = new UserActivity();
            //    transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
            //    account.SetTitle(StaticUser.FirstName + " " + StaticUser.LastName);
            //    exit1.SetVisible(true);
            //}
            //else
            //{
            //    ContentMainActivity home = new ContentMainActivity();
            //    transaction1.Replace(Resource.Id.framelayout, home).AddToBackStack(null).Commit();
            //    exit1.SetVisible(false);
            //    account.SetTitle("Войти");
            //}
            
            //navigationView.SetNavigationItemSelectedListener(this);
        }

        //private async void AuthorizationAndReceivingToken()
        //{
        //    AuthModel auth = new AuthModel
        //    {
        //        Login = CrossSettings.Current.GetValueOrDefault("login",""),
        //        Password = CrossSettings.Current.GetValueOrDefault("password", "")
        //    };

        //    using (var client = ClientHelper.GetClient(auth.Login, auth.Password))
        //    {
        //        AuthService.InitializeClient(client);
        //        var o_data = await AuthService.Login(auth);

        //        if (o_data.Status == HttpStatusCode.OK)
        //        {
        //            AuthResponse o_user_data = new AuthResponse();
        //            o_user_data = o_data.ResponseData;

        //            CrossSettings.Current.AddOrUpdateValue("token", o_user_data.Token);
        //            CrossSettings.Current.AddOrUpdateValue("role", o_user_data.Role);

        //            if (StaticUser.PresenceOnPage == true)
        //            {
        //                if (CrossSettings.Current.GetValueOrDefault("role", "") == "driver")
        //                {
        //                    Intent intent = new Intent(this, typeof(Activity.MainActivity2));
        //                    StartActivity(intent);
        //                    this.Finish();
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        UserActivity content2 = new UserActivity();
        //                        transaction1.Replace(Resource.Id.framelayout, content2).Commit();
        //                    }
        //                    catch (System.Exception ex)
        //                    {
        //                        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
        //                    }                          

        //                    btnAddOrder.SetTitle("Заказать");
        //                    btnAddOrder.SetIcon(Resource.Drawable.ic_add_order);

        //                    btnOrders.SetTitle("Заказы");
        //                    btnOrders.SetIcon(Resource.Drawable.ic_orders);

        //                    btnExit.SetTitle("Выход");
        //                    btnExit.SetIcon(Resource.Drawable.ic_menu_exit);

        //                }

        //            }
        //            else
        //            {
        //                ContentMainActivity content = new ContentMainActivity();
        //                transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();

        //                btnAddOrder.SetTitle("О нас");
        //                btnAddOrder.SetIcon(Resource.Drawable.ic_dashboard_black_24dp);

        //                btnOrders.SetTitle("Отзывы");
        //                btnOrders.SetIcon(Resource.Drawable.ic_notifications_black_24dp);

        //                btnExit.SetTitle("Контакты");
        //                btnExit.SetIcon(Resource.Drawable.ic_information);

        //            }
        //        }
        //        else
                
        //        {
        //            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
        //        }

        //    }
        //}

        public interface IBackButtonListener
        {
            void OnBackPressed();
        }
        public override void OnBackPressed()
        {
            // Ignoring stuff about DrawerLayout, etc for demo purposes.
            var currentFragment = SupportFragmentManager.FindFragmentById(Resource.Id.framelayout);
            var listener = currentFragment as IBackButtonListener;
            if (listener != null)
            {
                listener.OnBackPressed();
                return;
            }
            base.OnBackPressed();
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


        //public bool OnNavigationItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;
        //    //MenuItem register = item.findItem(R.id.menuregistrar);
        //    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
            
        //    if (id == Resource.Id.nav_auth)
        //    {
        //        if (CrossSettings.Current.GetValueOrDefault("isAuth", "") == "true")
        //        {
        //            UserActivity content2 = new UserActivity();
        //            transaction1.Replace(Resource.Id.framelayout, content2).AddToBackStack(null).Commit();
        //        }
        //        else
        //        {
        //            AuthActivity home = new AuthActivity();
        //            transaction1.Replace(Resource.Id.framelayout, home).AddToBackStack(null).Commit();
        //        }
                
        //    }
        //    if(id == Resource.Id.nav_registration)
        //    {
        //        Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
        //        alert.SetTitle("Внимание !");
        //        alert.SetMessage("Необходимо выбрать вид регистрации.");
        //        alert.SetPositiveButton("Для физ.лица", (senderAlert, args) => {
        //            Activity_Legal_Entity_Registration content3 = new Activity_Legal_Entity_Registration();
        //            transaction1.Replace(Resource.Id.framelayout, content3).AddToBackStack(null).Commit();
        //        });
        //        alert.SetNegativeButton("Для юр.лица", (senderAlert, args) => {
        //            Activity_Registration_Individual_Person content4 = new Activity_Registration_Individual_Person();
        //            transaction1.Replace(Resource.Id.framelayout, content4).AddToBackStack(null).Commit();
        //        });
        //        Dialog dialog = alert.Create();
        //        dialog.Show();
        //    }
        //    if (id == Resource.Id.nav_camera)
        //    {

                
        //    }
        //    else if (id == Resource.Id.nav_gallery)
        //    {
        //        if (CrossSettings.Current.GetValueOrDefault("isAuth","") == "true")
        //        {
        //            UserActivity content = new UserActivity();
        //            transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
        //        }
        //    }
        //    else if (id == Resource.Id.nav_slideshow)
        //    {
        //        if (CrossSettings.Current.GetValueOrDefault("isAuth", "") == "true")
        //        {
        //            AddOrderActivity content = new AddOrderActivity();
        //            transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
        //        }   
        //    }
        //    else if (id == Resource.Id.nav_manage)
        //    {
        //        if (CrossSettings.Current.GetValueOrDefault("isAuth", "") == "true")
        //        {
        //            ListOrdersActivity content = new ListOrdersActivity();
        //            transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
        //        }
        //        else
        //        {
        //            SearchOrderActivity content = new SearchOrderActivity();
        //            transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
        //        }
        //    }
        //    else if (id == Resource.Id.nav_slideshow)
        //    {
        //        AddOrderActivity content = new AddOrderActivity();
        //        transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
        //    }
        //    else if (id == Resource.Id.nav_send)
        //    {

        //    }
        //    else if (id == Resource.Id.nav_exit)
        //    {
        //        string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //        File.Delete(dir_path + "user_data.txt");
        //        CrossSettings.Current.AddOrUpdateValue("isAuth", "false");
                
        //        Intent content = new Intent(this, typeof(MainActivity));
        //        StartActivity(content);
        //    }

        //    DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
        //    drawer.CloseDrawer(GravityCompat.Start);
        //    return true;
        //}
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private class SamplePermissionListener : Java.Lang.Object, IMultiplePermissionsListener
        {
            MainActivity activity;
            public SamplePermissionListener(MainActivity activity)
            {
                this.activity = activity;
            }

            public void OnPermissionDenied(PermissionDeniedResponse p0)
            {
                //Snackbar.Make(activity.main_form, "Permission Denied", Snackbar.LengthShort).Show();
            }

            public void OnPermissionGranted(PermissionGrantedResponse p0)
            {
                //Snackbar.Make(activity.main_form, "Permission Granted", Snackbar.LengthShort).Show();
            }

            //public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken token)
            //{
            //    activity.ShowRequestPermissionRationale(token);
            //}

            public void OnPermissionRationaleShouldBeShown(IList<PermissionRequest> p0, IPermissionToken p1)
            {
                p1.ContinuePermissionRequest();
                throw new System.NotImplementedException();
            }

            public void OnPermissionsChecked(MultiplePermissionsReport p0)
            {
                if (p0.AreAllPermissionsGranted())
                {

                }

                // check for permanent denial of any permission
                if (p0.IsAnyPermissionPermanentlyDenied)
                {
                    // show alert dialog navigating to Settings

                }
            }
        }

       
    }

   

    internal class MyDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
    {
        private IPermissionToken token;

        public MyDismissListener(IPermissionToken token)
        {
            this.token = token;
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            token.CancelPermissionRequest();
        }
    }
}

