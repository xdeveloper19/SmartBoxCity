using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common;
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
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Activity.Driver
{
    public class TaskListAdapter: BaseAdapter<IViewItemType>, IOnMapReadyCallback
    {
        Context context;
        Bundle savedInstanceState;
        List<IViewItemType> tasks;
        private GoogleMap GMap;
        MapView mMapView;
        private LayoutInflater inflater;
        Android.App.FragmentTransaction manager;

        private Button btn_interrupt;

        private Button btn_perform;

        public TaskListAdapter(Context Context, List<IViewItemType> List, FragmentManager Manager, Bundle savedInstance)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.tasks = List;
            this.savedInstanceState = savedInstance;

            inflater = (LayoutInflater)this.context.GetSystemService(Context.LayoutInflaterService);
        }
        public override IViewItemType this[int position] => tasks[position];

        public override int Count => tasks.Count;

        public override long GetItemId(int position)
        {
            return position;//Convert.ToInt64(orders[position].Id);
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            try
            {
                IViewItemType viewItem = tasks[position];
                if (viewItem.GetViewType() == ViewType.Header)
                {
                    CurrentTaskModel taskViewContent = (CurrentTaskModel)viewItem;
                    view = inflater.Inflate(Resource.Layout.driver_header_task, null);

                    #region Объявление переменных в заголовке
                    var txt_order_id = view.FindViewById<TextView>(Resource.Id.btn_about_order);
                    var txt_title = view.FindViewById<TextView>(Resource.Id.txtTaskName);
                    mMapView = view.FindViewById<MapView>(Resource.Id.fragmentMap3);
                    btn_perform = view.FindViewById<Button>(Resource.Id.btn_prime2);
                    btn_interrupt = view.FindViewById<Button>(Resource.Id.btn_prime);
                    #endregion

                    MapsInitializer.Initialize(context);
                    // HomeService.SetListViewHeightBasedOnChildren(lstTask);

                    switch (GooglePlayServicesUtil.IsGooglePlayServicesAvailable(context))
                    {
                        case ConnectionResult.Success:
                            Toast.MakeText(context, "SUCCESS", ToastLength.Long).Show();
                            mMapView.OnCreate(savedInstanceState);
                            mMapView.GetMapAsync(this);
                            break;
                        case ConnectionResult.ServiceMissing:
                            Toast.MakeText(context, "ServiceMissing", ToastLength.Long).Show();
                            break;
                        case ConnectionResult.ServiceVersionUpdateRequired:
                            Toast.MakeText(context, "Update", ToastLength.Long).Show();
                            break;
                        default:
                            Toast.MakeText(context, GooglePlayServicesUtil.IsGooglePlayServicesAvailable(context), ToastLength.Long).Show();
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
                        LayoutInflater layoutInflater = LayoutInflater.From(context);
                        View view = layoutInflater.Inflate(Resource.Layout.driver_choice_box, null);
                        AlertDialog.Builder alert = new AlertDialog.Builder(context);
                        alert.SetView(view);
                        #region Объявление переменных в диалоговом окне
                        var RadioGroupBoxDriver = view.FindViewById<RadioGroup>(Resource.Id.RadioGroupBoxDriver);
                        var txt_choice_box = view.FindViewById<TextView>(Resource.Id.txt_choice_box);
                        #endregion

                        if (StaticTask.containers_id == null)
                            txt_choice_box.Visibility = ViewStates.Invisible;
                        else if (StaticTask.containers_id.Count != 0)
                        {
                            txt_choice_box.Visibility = ViewStates.Visible;
                            // Create Radio Group
                            //var rg = new RadioGroup(this);
                            //layout.AddView(rg);
                            var rb = new RadioButton(context) { Text = StaticTask.containers_id[0], Focusable = true };
                            RadioGroupBoxDriver.AddView(rb);
                            StaticTask.box_id = StaticTask.containers_id[0];
                            rb.Checked = true;

                            for (int i = 1; i < StaticTask.containers_id.Count; i++)
                            {
                                var rb1 = new RadioButton(context) { Text = StaticTask.containers_id[i] };
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
                        LayoutInflater layoutInflater = LayoutInflater.From(context);
                        View view = layoutInflater.Inflate(Resource.Layout.driver_confirm_task, null);
                        AlertDialog.Builder alert = new AlertDialog.Builder(context);
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
                else if (viewItem.GetViewType() == ViewType.List)
                {
                    TaskBookModel listViewContent = (TaskBookModel)viewItem;
                    view = inflater.Inflate(Resource.Layout.task_book_parameter, null);

                    view.FindViewById<TextView>(Resource.Id.txtTaskName).Text = "Заказ: " + listViewContent.order_id;
                    view.FindViewById<TextView>(Resource.Id.txtComment).Text = listViewContent.title;

                    var btn = view.FindViewById<Button>(Resource.Id.btn_info_order);

                    btn.Click += async delegate
                    {
                        try
                        {
                            MainOrderStatusActivity content = new MainOrderStatusActivity();
                            manager.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                        }

                    };

                }
            }
            catch
            {
            }

            return view;
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
                    Toast.MakeText(context, o_data.ResponseData.Message, ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."

                }

                TaskActivity act = new TaskActivity();
                manager.Replace(Resource.Id.frameDriverlayout, act);
                manager.Commit();
            }
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
                    Toast.MakeText(context, o_data.ResponseData.Message, ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."

                }

                TaskActivity act = new TaskActivity();
                manager.Replace(Resource.Id.frameDriverlayout, act);
                manager.Commit();
            }
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
    }
}