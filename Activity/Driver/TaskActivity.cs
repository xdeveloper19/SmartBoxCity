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
        OK = 0,
        NotFound,
        ServerError
    }
    public class TaskActivity: Fragment
    {
        private ListView lstTask;
        public static List<IViewItemType> tasklist;
        GPSService _gpsService;
        Bundle bundle;

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
            lstTask = view.FindViewById<ListView>(Resource.Id.tasklistview);

            var result = GetTasks();

            if (result.Result == TaskStatus.OK)
            {
                //LayoutInflater layoutInflater = LayoutInflater.From(Activity);
                //ViewGroup view_task = (ViewGroup)layoutInflater.Inflate(Resource.Layout.driver_header_task, null);
                //lstTask.AddHeaderView(view_task);
                bundle = savedInstanceState;
                _gpsService = new GPSService(Activity);
                _gpsService.UpdateLocation();
            }
            else
            {
                FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                TaskNotFoundActivity content = new TaskNotFoundActivity();
                transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
            }
            return view;
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

       
       
        //public override void OnSaveInstanceState(Bundle outState)
        //{
        //    base.OnSaveInstanceState(outState);
        //    mMapView.OnSaveInstanceState(outState);
        //}

        //public override void OnResume()
        //{
        //    mMapView.OnResume();
        //    base.OnResume();
        //}

        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    mMapView.OnDestroy();
        //}

        //public void OnLowMemory()
        //{
        //    base.OnLowMemory();
        //    mMapView.OnLowMemory();
        //}


    }
}