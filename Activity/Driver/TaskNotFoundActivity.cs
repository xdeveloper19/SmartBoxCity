using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace SmartBoxCity.Activity.Driver
{
    public class TaskNotFoundActivity: Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_not_found_tasks, container, false);
            SwitchCompat btn_switch = view.FindViewById<SwitchCompat>(Resource.Id.switch_compat);
            btn_switch.Focusable = false;
            

            btn_switch.Click += (o, e) => {
                // Perform action on clicks
                if (btn_switch.Checked)
                {
                    btn_switch.Text = "Свободен. У вас нет задач на перевозку груза.";
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Подтверждение действия");
                    alert.SetMessage("Задачи Вам не будут распределяться. Вы действительно заняты?");
                    alert.SetPositiveButton("Ок", (senderAlert, args) =>
                    {
                        
                        //to do...
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                        btn_switch.Text = "Занят. Задачи на перевозку вам не распределяются.";
                        btn_switch.Checked = false;
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    btn_switch.Text = "Занят. Задачи на перевозку вам не распределяются.";
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Подтверждение действия");
                    alert.SetMessage("Вам будут распределяться задачи. Вы действительно освободились?");
                    alert.SetPositiveButton("Ок", (senderAlert, args) =>
                    {
                       
                        //to do...
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                        btn_switch.Text = "Свободен. У вас нет задач на перевозку груза.";
                        btn_switch.Checked = true;
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    
                }
                    
            };
            return view;
        }

        //private async Task<TaskStatus> GetTasks()
        //{
        //    using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
        //    {
        //        TaskService.InitializeClient(client);
        //        var o_data = await TaskService.GetTasks();

        //        if (o_data.Status == System.Net.HttpStatusCode.OK)
        //        {
        //            //o_data.Message = "Успешно авторизован!";
        //            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();

        //            if (o_data.ResponseData == null)
        //                return TaskStatus.NotFound;
        //            //StaticUser.Email = s_login.Text;
        //            //StaticUser.AddInfoAuth(o_user_data);

        //            //обязательно должен быть прогресс бар при обращении к серверу, типо такого
        //            //preloader.Visibility = Android.Views.ViewStates.Invisible;
        //            //foreach (var task in o_data.ResponseData.ARCHIVE)
        //            //{
        //            //    orderlist.Add(new TaskBookModel
        //            //    {
        //            //        Id = order.id,
        //            //        Destination = order.destination_address,
        //            //        Inception = order.inception_address,
        //            //        Price = order.payment_amount + " рублей",
        //            //        OrderName = "Заказ " + order.id,
        //            //        Date = order.stage2_datetime.ToString()
        //            //    }
        //            //    );
        //            //}
        //            TaskBookModel p2 = new TaskBookModel()
        //            {
        //                order_id = "OP5887450402",
        //                priority = "2",
        //                address = "Славный переулок, 5, Новошахтинск",
        //                title = "г Ростов-на-Дону, ул Орбитальная, д 76. Доставить пустой контейнер."
        //            };
        //            TaskBookModel p3 = new TaskBookModel()
        //            {
        //                order_id = "OP5887450402",
        //                priority = "3",
        //                address = "Славный переулок, 5, Новошахтинск",
        //                title = "г Ростов-на-Дону, ул Орбитальная, д 76. Доставить пустой контейнер."
        //            };

        //            List<TaskBookModel> tasks = new List<TaskBookModel>();

        //            tasks.Add(p2);
        //            tasks.Add(p3);

        //            tasklist = tasks.OrderBy(o => o.priority).ToList();
        //            UpdateList();
        //            lstTask.ItemClick += ListOrders_ItemClick;
        //            return TaskStatus.OK;
        //        }
        //        else
        //        {
        //            Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."
        //            return TaskStatus.ServerError;
        //        }


        //    }

        //}
    }
}