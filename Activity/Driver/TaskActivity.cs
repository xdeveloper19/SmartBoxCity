using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model.TaskViewModel;
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
    public class TaskActivity: Fragment
    {
        private ListView lstTask;
        private EditText editEnterOrder;
        public static List<TaskBookModel> tasklist;

        private Button btn_interrupt;

        private Button btn_perform;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = null;
            var result = GetTasks();

            if (result.Result == TaskStatus.OK)
            {
                view = inflater.Inflate(Resource.Layout.driver_tasks, container, false);
                //Button btn_about_order = view.FindViewById<Button>(Resource.Id.btn_about_order);
                btn_perform = view.FindViewById<Button>(Resource.Id.btn_prime2);
                btn_interrupt = view.FindViewById<Button>(Resource.Id.btn_prime);
                //btn_about_order.Click += async delegate
                //{
                //    OrderActivity content = new OrderActivity();
                //    transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                //};

                btn_perform.Click += async delegate
                {
                    PerformTask();
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

                    };

                    rbnt_finished_shift_task.Click += delegate
                    {
                        edit_text_other_task.Enabled = false;

                    };

                    rbnt_relaxation_task.Click += delegate
                    {
                        edit_text_other_task.Enabled = false;

                    };

                    rbnt_malfunction_task.Click += delegate
                    {
                        edit_text_other_task.Enabled = false;

                    };

                    #endregion

                    alert.SetCancelable(false)
                    .SetPositiveButton("Прервать", delegate
                    {
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
                var o_data = await TaskService.CompleteTask("GP1511631120-1", "");

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.ResponseData.Message, ToastLength.Long).Show();

                   
                    //StaticUser.Email = s_login.Text;
                    //StaticUser.AddInfoAuth(o_user_data);

                    //обязательно должен быть прогресс бар при обращении к серверу, типо такого
                    //preloader.Visibility = Android.Views.ViewStates.Invisible;
                    //foreach (var task in o_data.ResponseData.ARCHIVE)
                    //{
                    //    orderlist.Add(new TaskBookModel
                    //    {
                    //        Id = order.id,
                    //        Destination = order.destination_address,
                    //        Inception = order.inception_address,
                    //        Price = order.payment_amount + " рублей",
                    //        OrderName = "Заказ " + order.id,
                    //        Date = order.stage2_datetime.ToString()
                    //    }
                    //    );
                    //}
                   
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."
                    
                }


            }
        }

        private async void AbortTask()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                TaskService.InitializeClient(client);
                var o_data = await TaskService.Abort("Отдых");

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.ResponseData.Message, ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."

                }


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

                    if (o_data.ResponseData == null)
                        return TaskStatus.NotFound;
                    //StaticUser.Email = s_login.Text;
                    //StaticUser.AddInfoAuth(o_user_data);

                    //обязательно должен быть прогресс бар при обращении к серверу, типо такого
                    //preloader.Visibility = Android.Views.ViewStates.Invisible;
                    //foreach (var task in o_data.ResponseData.ARCHIVE)
                    //{
                    //    orderlist.Add(new TaskBookModel
                    //    {
                    //        Id = order.id,
                    //        Destination = order.destination_address,
                    //        Inception = order.inception_address,
                    //        Price = order.payment_amount + " рублей",
                    //        OrderName = "Заказ " + order.id,
                    //        Date = order.stage2_datetime.ToString()
                    //    }
                    //    );
                    //}
                    TaskBookModel p2 = new TaskBookModel()
                    {
                        order_id = "OP5887450402",
                        priority = "2",
                        address = "Славный переулок, 5, Новошахтинск",
                        title = "г Ростов-на-Дону, ул Орбитальная, д 76. Доставить пустой контейнер."
                    };
                    TaskBookModel p3 = new TaskBookModel()
                    {
                        order_id = "OP5887450402",
                        priority = "3",
                        address = "Славный переулок, 5, Новошахтинск",
                        title = "г Ростов-на-Дону, ул Орбитальная, д 76. Доставить пустой контейнер."
                    };

                    List<TaskBookModel> tasks = new List<TaskBookModel>();
         
                    tasks.Add(p2);
                    tasks.Add(p3);

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

        public override void OnResume()
        {
            // UpdateList();
            base.OnResume();
        }

        public void UpdateList()
        {
            TaskListAdapter adapter = new TaskListAdapter(Activity, tasklist, this.FragmentManager);
            lstTask.Adapter = adapter;
        }
    }
}