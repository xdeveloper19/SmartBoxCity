using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;

using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.TaskViewModel;
using Entity.Repository;
using Plugin.Settings;
using SmartBoxCity.Activity.Order;

using SmartBoxCity.Service;
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Activity.Driver
{
    public enum TaskStatus
    {
        OK,
        NotFound,
        ServerError
    }
    public class TaskActivity: Fragment, IOnMapReadyCallback
    {
        private ListView lstTask;
        public static List<TaskBookModel> tasklist;
        private GoogleMap GMap;
        GPSService _gpsService;
        MapView mMapView;

        private Button btn_interrupt;

        private Button btn_perform;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

           
        }

        //private void SetUpMap(View view)
        //{
        //    if (GMap == null)
        //    {
        //       

        //        FragmentManager.FindViewById<MapView>(Resource.Id.fragmentMap3).GetMapAsync(this);
        //    }
        //}


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = null;
            view = inflater.Inflate(Resource.Layout.driver_tasks, container, false);
            var txt_order_id = view.FindViewById<TextView>(Resource.Id.btn_about_order);
            var txt_title = view.FindViewById<TextView>(Resource.Id.txtTaskName);
            lstTask = view.FindViewById<ListView>(Resource.Id.tasklistview);

            var result = GetTasks();

            if (result.Result == TaskStatus.OK)
            {
                btn_perform = view.FindViewById<Button>(Resource.Id.btn_prime2);
                btn_interrupt = view.FindViewById<Button>(Resource.Id.btn_prime);

                _gpsService = new GPSService(Activity);
                _gpsService.UpdateLocation();

                MapsInitializer.Initialize(Activity);
                mMapView = view.FindViewById<MapView>(Resource.Id.fragmentMap3);
               
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

                

              

                txt_order_id.Text = StaticTask.order_id;
                txt_title.Text = StaticTask.title;
                //btn_about_order.Click += async delegate
                //{
                //    OrderActivity content = new OrderActivity();
                //    transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                //};

                btn_perform.Click += async delegate
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.driver_choice_box, null);
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetView(view);
                    #region Объявление переменных в диалоговом окне
                    var RadioGroupBoxDriver = view.FindViewById<RadioGroup>(Resource.Id.RadioGroupBoxDriver);
                    var txt_choice_box = view.FindViewById<TextView>(Resource.Id.txt_choice_box);
                    #endregion

                    if (StaticTask.containers_id == null)
                        txt_choice_box.Visibility = ViewStates.Invisible;
                    else if(StaticTask.containers_id.Count != 0)
                    {
                        txt_choice_box.Visibility = ViewStates.Visible;
                        // Create Radio Group
                        //var rg = new RadioGroup(this);
                        //layout.AddView(rg);
                        var rb = new RadioButton(Activity) { Text = StaticTask.containers_id[0], Focusable = true };
                        RadioGroupBoxDriver.AddView(rb);
                        StaticTask.box_id = StaticTask.containers_id[0];
                        rb.Checked = true;

                        for (int i = 1; i < StaticTask.containers_id.Count; i++)
                        {
                            var rb1 = new RadioButton(Activity) { Text = StaticTask.containers_id[i] };
                            RadioGroupBoxDriver.AddView(rb1);
                        }


                        #region Обработка событий кнопок

                        // Show Radio Button Selected
                        RadioGroupBoxDriver.CheckedChange += (s, e) => {
                            StaticTask.box_id = StaticTask.containers_id[e.CheckedId - 1];
                        };

                        #endregion
                    }


                    alert.SetCancelable(false)
                    .SetPositiveButton("Выполнил", delegate
                    {
                        PerformTask();
                       
                    })
                    .SetNegativeButton("Отмена", delegate
                    {
                        alert.Dispose();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                };

                btn_interrupt.Click += async delegate
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                    View view = layoutInflater.Inflate(Resource.Layout.driver_confirm_task, null);
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetView(view);
                    #region Объявление переменных в диалоговом окне
                    var edit_text_other_task = view.FindViewById<EditText>(Resource.Id.edit_text_other_task);
                    var rbnt_malfunction_task = view.FindViewById<RadioButton>(Resource.Id.rbnt_malfunction_task);
                    var rbnt_relaxation_task = view.FindViewById<RadioButton>(Resource.Id.rbnt_relaxation_task);
                    var rbnt_finished_shift_task = view.FindViewById<RadioButton>(Resource.Id.rbnt_finished_shift_task);
                    var rbnt_other_task = view.FindViewById<RadioButton>(Resource.Id.rbnt_other_task);

                    edit_text_other_task.Enabled = false;
                    #endregion

                    #region Обработка событий кнопок

                    rbnt_other_task.Click += delegate
                    {
                        edit_text_other_task.Enabled = true;
                        StaticTask.comment = edit_text_other_task.Text;
                    };

                    rbnt_finished_shift_task.Click += delegate
                    {
                        edit_text_other_task.Enabled = false;
                        StaticTask.comment = "Закончил смену";
                    };

                    rbnt_relaxation_task.Click += delegate
                    {
                        edit_text_other_task.Enabled = false;
                        StaticTask.comment = "Отдых";
                    };

                    rbnt_malfunction_task.Click += delegate
                    {
                        edit_text_other_task.Enabled = false;
                        StaticTask.comment = "Неисправность";
                    };

                    #endregion

                    alert.SetCancelable(false)
                    .SetPositiveButton("Прервать", delegate
                    {
                        if (rbnt_other_task.Checked)
                        {
                            StaticTask.comment = edit_text_other_task.Text;
                        }
                        AbortTask();
                       
                    })
                    .SetNegativeButton("Отмена", delegate
                    {
                        alert.Dispose();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();

                };

               
            }
            else
            {
                FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                TaskNotFoundActivity content = new TaskNotFoundActivity();
                transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
            }
            return view;
        }

    
        

        private async void PerformTask()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                TaskService.InitializeClient(client);
                var o_data = await TaskService.CompleteTask(StaticTask.id, StaticTask.box_id);

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.ResponseData.Message, ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."
                    
                }

                FragmentTransaction ft = this.FragmentManager.BeginTransaction();
                TaskActivity act = new TaskActivity();
                ft.Replace(Resource.Id.frameDriverlayout, act);
                ft.Commit();
            }
        }

        private async void AbortTask()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                TaskService.InitializeClient(client);
                var o_data = await TaskService.Abort(StaticTask.comment);

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.ResponseData.Message, ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."

                }
                FragmentTransaction ft = this.FragmentManager.BeginTransaction();
                TaskActivity act = new TaskActivity();
                ft.Replace(Resource.Id.frameDriverlayout, act);
                ft.Commit();
            }
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

                    List<TaskBookModel> tasks = new List<TaskBookModel>();
                    o_data.ResponseData.TASKS.Remove(firstTask);
                    foreach(var task in o_data.ResponseData.TASKS)
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

                    tasklist = tasks.OrderBy(o => o.priority).ToList();
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

        public void OnLowMemory()
        {
            base.OnLowMemory();
            mMapView.OnLowMemory();
        }


    }
}