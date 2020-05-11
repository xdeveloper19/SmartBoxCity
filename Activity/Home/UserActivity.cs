using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using SmartBoxCity.Model.OrderViewModel;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Service;
using System.Net;
using SmartBoxCity.Model;
using SmartBoxCity.Model.BoxViewModel;

namespace SmartBoxCity.Activity.Home
{
    public class UserActivity : Fragment
    {
        private ListView lstOrder;

        public static List<OrderResponse> orderlist;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //не сохраняет значение 
            var is_ordered = CrossSettings.Current.GetValueOrDefault("isOrdered", "");
            if (is_ordered == "true")
            {
                MakeOrderModel model = new MakeOrderModel()
                {
                    destination_address = StaticOrder.Destination_address,
                    for_date = StaticOrder.For_date,
                    for_time = StaticOrder.For_time,
                    height = StaticOrder.Height,
                    inception_address = StaticOrder.Inception_address,
                    cargo_class = StaticOrder.Cargo_class,
                    cargo_loading = StaticOrder.Cargo_loading,
                    cargo_type = StaticOrder.Cargo_type,
                    destination_lat = StaticOrder.Destination_lat,
                    destination_lng = StaticOrder.Destination_lng,
                    inception_lat = StaticOrder.Inception_lat,
                    inception_lng = StaticOrder.Inception_lng,
                    insurance = StaticOrder.Insurance,
                    receiver = StaticOrder.Receiver,
                    length = StaticOrder.Length,
                    qty = StaticOrder.Qty,
                    weight = StaticOrder.Weight,
                    width = StaticOrder.Width
                };

                AddOrder(model);
            }
           
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if(CrossSettings.Current.GetValueOrDefault("isAuth", "") != "true")
            {
                var view = inflater.Inflate(Resource.Layout.activity_not_found_order, container, false);
                return view;
            }
            else 
            {
                var view = inflater.Inflate(Resource.Layout.activity_user, container, false);/// ошибка при нажати на кнопку "назад" на лефоне(Binary XML file line #1: Binary XML file line #1: Error inflating class fragment' )
                lstOrder = view.FindViewById<ListView>(Resource.Id.CurrentOrderListView);
                orderlist = new List<OrderResponse>();
                string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                GetOrders();


                //btn_state_sensors_user.Click += Btn_Show_State_Sensors;

                //btn_lock_unlock_castle.Click += async delegate
                //{

                //    try
                //    {
                //        Android.Support.V7.App.AlertDialog alertDialog;
                //        List<string> Item = new List<string>();
                //        Item.Add("Выгрузка завершена. Контейнер готов к отправке.");


                //        var builder = new Android.Support.V7.App.AlertDialog.Builder(Activity);
                //        builder.SetTitle("Вы действительно хотите открыть замок контейнера?");

                //        bool[] toDownload = { false };
                //        builder.SetMultiChoiceItems(Item.ToArray(), toDownload, (sender, e) =>
                //        {
                //            int index = e.Which;

                //            toDownload[index] = e.IsChecked;
                //        });


                //        builder.SetNegativeButton("Отмена", delegate
                //        {
                //            //Some to do...
                //        })
                //        .SetPositiveButton("Открыть", delegate
                //        {
                //            if (toDownload[0] == true)
                //            {
                //                //to do...
                //            }
                //            //if (s_lock_unlock_castle.Text == "заблокирована")
                //            //    s_lock_unlock_castle.Text = "разблокирована";
                //            //else
                //            //    s_lock_unlock_castle.Text = "заблокирована";
                //        });

                //        alertDialog = builder.Create();
                //        alertDialog.Show();
                //    }
                //    catch (Exception ex)
                //    {
                //        Toast.MakeText(Activity, "" + ex.Message, ToastLength.Long).Show();
                //    }

                //};

                //btn_pay.Click += async delegate
                //{
                //    try
                //    {
                //        if (s_payment.Text != "Оплачено")
                //        {
                //            progressBar.Progress = 8;
                //            status_view.Text = "8. Завершение использования";

                //            //s_pin_access_code.Text = "1324";
                //            s_payment.Text = "Оплачено";
                //            Toast.MakeText(Activity, "Оплата произведена", ToastLength.Long).Show();
                //            GetInfoAboutBox(dir_path);

                //        }
                //        else
                //        {
                //            Toast.MakeText(Activity, "Оплата уже была произведена", ToastLength.Long).Show();
                //        }
                //    }
                //    catch(Exception ex)
                //    {
                //        Toast.MakeText(Activity, "" + ex.Message, ToastLength.Long).Show();
                //    }
                //};

                //btn_pass_delivery_service.Click += async delegate
                //{
                //    FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();
                //    MainOrderStatusActivity content = new MainOrderStatusActivity();
                //    transaction1.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                //};

                //btn_exit_.Click += async delegate
                //{
                //    File.Delete(dir_path + "user_data.txt");
                //    CrossSettings.Current.AddOrUpdateValue("isAuth", "false");
                //    Intent content = new Intent(Activity, typeof(MainActivity));
                //    StartActivity(content);
                //};
               
                return view;
            }                        
        }

        private async void GetOrders()
        {
            //var o_data1 = new ServiceResponseObject<SensorResponse>();
            //using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            //{
            //    OrderService.InitializeClient(client);
            //    o_data1 = await OrderService.GetSensorParameters();
            //}

            var o_data = new ServiceResponseObject<ListResponse<OrderResponse, ArchiveResponse>>();
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                OrderService.InitializeClient(client);
                o_data = await OrderService.GetOrders(client);

                if (o_data.Status == HttpStatusCode.OK)
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    foreach (var order in o_data.ResponseData.ORDERS)
                    {
                        orderlist.Add(new OrderResponse
                        {
                            id = order.id,
                            inception_address = order.id,
                            inception_lat = order.inception_lat,
                            cargo_class = order.cargo_class,
                            distance = order.distance,
                            insurance = order.insurance,
                            stage2_datetime = order.stage2_datetime,
                            stage5_datetime = order.stage5_datetime,
                            payment_id = order.payment_id,
                            order_stage_id = order.order_stage_id,
                            created_at = order.created_at,
                            payment_amount = order.payment_amount,
                            payment_status = order.payment_status,
                            order_stage_name = order.order_stage_name,
                            last_stage_at = order.last_stage_at,
                            container_id = order.container_id,
                            sensors_status = order.sensors_status,
                            event_count = order.event_count,
                        }
                        );
                    }
                    UpdateList();
                    lstOrder.ItemClick += ListOrders_ItemClick;
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: <. Path '', line 0, position 0."

                }
            }
        }

        private void ListOrders_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Activity, "Выбран заказ №" + e.Position.ToString(), ToastLength.Long).Show();
        }

        private void UpdateList()
        {
            AdapterUserActivity adapter = new AdapterUserActivity(Activity, orderlist, this.FragmentManager);
            lstOrder.Adapter = adapter;
        }

        public async void AddOrder(MakeOrderModel model)
        {
            var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", ""));
            OrderService.InitializeClient(client);
            var o_data = await OrderService.AddOrder(model);

            if (o_data.Status == HttpStatusCode.OK)
            {

                OrderSuccessResponse o_user_data = new OrderSuccessResponse();
                o_user_data = o_data.ResponseData;
                StaticOrder.Order_id = o_user_data.order_id;
                //AlertDialog.Builder alert = new AlertDialog.Builder(Context);
                //alert.SetTitle("Предупреждение.");
                //alert.SetMessage("Заказ оформлен успешно !");
                //alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                //{
                //});
                //Dialog dialog = alert.Create();
                //dialog.Show();
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
            }
            else
            {
                //AlertDialog.Builder alert = new AlertDialog.Builder(Context);
                //alert.SetTitle("Внимание!");
                //alert.SetMessage("Не получилось оформить заказ.\nПричина: " + o_data.Message + 
                //    "\nДля повторного оформления заказа зайдите в раздел 'Заказать'.");
                //alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                //{
                //});
                //Dialog dialog = alert.Create();
                //dialog.Show();
                Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
            }
            CrossSettings.Current.AddOrUpdateValue("isOrdered", "false");

        }
        //private async void GetSensorParameters()
        //{
        //    var o_data = new ServiceResponseObject<SensorResponse>();
        //    using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
        //    {
        //        //надо было сначала клиента указать, а потом вызывать метод
        //        //и обязательно с токеном
        //        OrderService.InitializeClient(client);
        //        o_data = await OrderService.GetSensorParameters();

        //        if (o_data.Status == HttpStatusCode.OK)
        //        {
        //            //o_data.Message = "Успешно авторизован!";
        //            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

        //        }
        //        else
        //        {
        //            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

        //        }
        //    }
        //}

        //private void Btn_Show_State_Sensors(object sender, EventArgs e)
        //{
        //    //GetSensorParameters();
        //    //    LayoutInflater layoutInflater = LayoutInflater.From(Activity);
        //    //    View view = layoutInflater.Inflate(Resource.Layout.activity_create_task, null);
        //    //    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
        //    //    alert.SetView(view);
        //    //    #region Объявление переменных в диалоговом окне
        //    //    var EditName = view.FindViewById<EditText>(Resource.Id.EditCreateTaskName);
        //    //    var EditTime = view.FindViewById<EditText>(Resource.Id.EditCreateTaskTime);
        //    //    var EditNote = view.FindViewById<EditText>(Resource.Id.EditCreateTaskNote);
        //    //    var RatingImportance = view.FindViewById<RatingBar>(Resource.Id.RatingCreateTaskImportance);
        //    //    var CheckBoxkReminder = view.FindViewById<CheckBox>(Resource.Id.CheckBoxCreateTaskReminder);
        //    //    #endregion

        //    //    alert.SetCancelable(false)
        //    //    .SetPositiveButton("Создать", delegate
        //    //    {
        //    //    })
        //    //    .SetNegativeButton("Отмена", delegate
        //    //    {
        //    //        alert.Dispose();
        //    //    });
        //    //    Dialog dialog = alert.Create();
        //    //    dialog.Show();
        //}

        public async void GetInfoAboutBox(string dir_path)
        {
            //try
            //{
            //    ContainerResponse container = new ContainerResponse();



            //    //пример чтения данных с файла
            //    string file_data_remember;
            //    using (FileStream file = new FileStream(dir_path + "box_data.txt", FileMode.Open, FileAccess.Read))
            //    {
            //        // преобразуем строку в байты
            //        byte[] array = new byte[file.Length];
            //        // считываем данные
            //        file.Read(array, 0, array.Length);
            //        // декодируем байты в строку
            //        file_data_remember = Encoding.Default.GetString(array);
            //        file.Close();
            //    }

            //    container = JsonConvert.DeserializeObject<ContainerResponse>(file_data_remember);

            //    string name = container.Name;//!!!!


            //    //if(s_payment.Text == "Оплачено")
            //    //{
            //    var myHttpClient = new HttpClient();

            //    var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + container.SmartBoxId);
            //    HttpResponseMessage response = await myHttpClient.GetAsync(uri);

            //    AuthApiData<BoxDataResponse> o_data = new AuthApiData<BoxDataResponse>();

            //    string s_result;
            //    using (HttpContent responseContent = response.Content)
            //    {
            //        s_result = await responseContent.ReadAsStringAsync();
            //    }

            //    o_data = JsonConvert.DeserializeObject<AuthApiData<BoxDataResponse>>(s_result);
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        if (o_data.Status == "0")
            //        {
            //            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
            //            BoxDataResponse exported_data = new BoxDataResponse();
            //            exported_data = o_data.ResponseData;

            //            StaticBox.AddInfoBox(exported_data);
            //            //добавляем инфу о найденном контейнере
            //            //container_name.Text = exported_data.Name.ToString();
            //            //container_name.Text = name;

            //            s_temperature.Text = exported_data.Temperature;
            //            s_light.Text = exported_data.Light.ToString();
            //            s_humidity.Text = exported_data.Wetness;
            //            StaticBox.Longitude = exported_data.Longitude;
            //            StaticBox.Latitude = exported_data.Latitude;
            //            s_longitude.Text = StaticBox.Longitude.ToString();
            //            s_latitude.Text = StaticBox.Latitude.ToString();
            //            //coordinates lat lon
            //            s_weight.Text = "100.0";
            //            //progressBar.Progress = 6;

            //            Text3.Text = "ПИН-код доступа отобразится после оплаты";

            //            //status_view.Text = "6. Ожидание выгрузки";
            //            if (s_payment.Text == "Оплачено")
            //                s_pin_access_code.Text = (exported_data.Code == null) ? "0000" : "1324";// !!!!  
            //            else
            //                s_pin_access_code.Text = "****";
            //            if (exported_data.IsOpenedDoor.ToString() == "true")
            //            {
            //                s_lock_unlock_door.Text = "разблокирована";
            //            }
            //            else
            //            {
            //                s_lock_unlock_door.Text = "заблокирована";
            //            }
            //            s_cost.Text = "39537.5";


            //            //var boxState = s_open_close_container.Text;
            //            //var doorState = s_lock_unlock_door.Text;


            //        }
            //        else
            //        {
            //            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
            //        }
            //    }

            //    //else if(s_payment.Text == "Не оплачено")
            //    //{
            //    //    s_temperature.Text = "****";
            //    //    s_light.Text = "****";
            //    //    s_humidity.Text = "****";
            //    //    s_weight.Text = "****";
            //    //    s_pin_access_code.Text = "****";
            //    //    s_lock_unlock_door.Text = "****";
            //    //    s_cost.Text = "1000";
            //    //    container_name.Text = name;
            //    //    s_latitude.Text = "****";
            //    //    s_longitude.Text = "****";
            //    //}

            //}
            //catch (Exception ex)
            //{
            //    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            //}

        }
      
    }
}