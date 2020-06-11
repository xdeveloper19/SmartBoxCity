using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Text.Method;
using Android.Util;
using Android.Widget;
using Android.Views;
using Cheesebaron.SlidingUpPanel;
using Entity.Repository;
using Plugin.Settings;
using WebService;
using WebService.Driver;
using Entity.Model;
using Entity.Model.TaskViewModel;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Driver
{
    public class MapActivity : Fragment, IOnMapReadyCallback
    {
        private const string SavedStateActionBarHidden = "saved_state_action_bar_hidden";
        private GoogleMap GMap;
        private ListView lstTask;
        private GPSService _gpsService;
        public static List<IViewItemType> tasklist;
        MapView mMapView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view =  inflater.Inflate(Resource.Layout.driver_map, container, false);
            mMapView = view.FindViewById<MapView>(Resource.Id.fragmentMap3);
            var layout = view.FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
            view.FindViewById<TextView>(Resource.Id.txt_title_tasks).MovementMethod = new LinkMovementMethod();
            lstTask = view.FindViewById<ListView>(Resource.Id.tasklistview);
            var result = GetTasks();

            if (result.Result == TaskStatus.OK)
            {

                _gpsService = new GPSService(Activity);
                _gpsService.UpdateLocation();
                // layout.ShadowDrawable = Resources.GetDrawable(Resource.Drawable.above_shadow);
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
                // HomeService.SetListViewHeightBasedOnChildren(lstTask);

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
            else
            {
                FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                TaskNotFoundActivity content = new TaskNotFoundActivity();
                transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
            }
            
            return view;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            //outState.PutBoolean(SavedStateActionBarHidden, !SupportActionBar.IsShowing);
        }

        private async Task<TaskStatus> GetTasks()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                TaskService.InitializeClient(client);
                var o_data = await TaskService.GetTasks();

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    StaticDriver.AddInfoDriver(o_data.ResponseData.DRIVER);

                    if (o_data.ResponseData.TASKS.Count == 0)
                    {
                        return TaskStatus.NotFound;
                    }


                    var firstTask = o_data.ResponseData.TASKS.First();
                    StaticTask.AddInfoTask(firstTask);

                    var boxes_id = o_data.ResponseData.CONTAINERS.Select(s => s.id).ToList();
                    StaticTask.AddContainersID(boxes_id);

                    var way_points = o_data.ResponseData.MAP_WAYPOINTS;
                    StaticTask.AddWayPoints(way_points);

                    List<IViewItemType> tasks = new List<IViewItemType>();
                    tasks.Add(new CurrentTaskModel
                    {
                        Description = StaticTask.title,
                        Order_Id = StaticTask.order_id
                    });

                    o_data.ResponseData.TASKS.Remove(firstTask);
                    foreach (var task in o_data.ResponseData.TASKS)
                    {
                        tasks.Add(new TaskBookModel
                        {
                            address = task.address,
                            order_id = task.order_id,
                            priority = task.priority,
                            title = task.title
                        }
                        );
                    }
                    //TaskBookModel p2 = new TaskBookModel()
                    //{
                    //    order_id = "OP5887450402",
                    //    priority = "2",
                    //    address = "Славный переулок, 5, Новошахтинск",
                    //    title = "г Ростов-на-Дону, ул Орбитальная, д 76. Доставить пустой контейнер."
                    //};
                    //TaskBookModel p3 = new TaskBookModel()
                    //{
                    //    order_id = "OP5887450402",
                    //    priority = "3",
                    //    address = "Славный переулок, 5, Новошахтинск",
                    //    title = "г Ростов-на-Дону, ул Орбитальная, д 76. Доставить пустой контейнер."
                    //};

                    tasklist = tasks;
                    UpdateList();
                    lstTask.ItemClick += ListOrders_ItemClick;
                    return TaskStatus.OK;
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."
                    return TaskStatus.ServerError;
                }
            }

        }

        private void ListOrders_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Toast.MakeText(Activity, "Выбран заказ №" + e.Position.ToString(), ToastLength.Long).Show();
        }

        public void UpdateList()
        {
            TaskListAdapter adapter = new TaskListAdapter(Activity, tasklist, this.FragmentManager);
            lstTask.Adapter = adapter;
        }


        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;

            LatLng location = new LatLng(StaticTask.way_points[0].lat, StaticTask.way_points[0].lng);
            PolylineOptions rectOptions = new PolylineOptions()
            {

            };
            rectOptions.Geodesic(true);
            rectOptions.InvokeWidth(1);
            rectOptions.InvokeColor(Color.Blue);

            for (int i = 0; i < StaticTask.way_points.Count; i++)
            {
                var latitude = StaticTask.way_points[i].lat;
                var longitude = StaticTask.way_points[i].lng;

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

        public override void OnResume()
        {
            base.OnResume();
            mMapView.OnResume();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mMapView.OnLowMemory();
        }

        public override void OnPause()
        {
            base.OnPause();
            mMapView.OnPause();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            mMapView.OnDestroy();
        }
    }
}