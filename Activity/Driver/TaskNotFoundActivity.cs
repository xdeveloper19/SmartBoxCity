using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Entity.Repository;
using Plugin.Settings;
using WebService;
using WebService.Driver;

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
            var txt_comment = view.FindViewById<TextView>(Resource.Id.txtNoTask);
            btn_switch.Focusable = true;

            if (StaticDriver.busy == "0")
            {
                btn_switch.Text = "Свободен. У вас нет задач на перевозку груза.";
                btn_switch.Checked = true;
            }
            else if (StaticDriver.busy == "1")
            {
                btn_switch.Text = "Занят. Задачи на перевозку вам не распределяются.";
                btn_switch.Checked = false;
            }
            else
            {
                txt_comment.Text = "Ошибка сервера. Зайдите на страницу позже.";
                btn_switch.Enabled = false;
            }
   
                
            btn_switch.Click += (o, e) => {
                // Perform action on clicks
                if (btn_switch.Checked)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Подтверждение действия");
                    alert.SetMessage("Задачи Вам не будут распределяться. Вы действительно освободились?");
                    alert.SetPositiveButton("Ок", (senderAlert, args) =>
                    {
                        var result = FreeStatus();
                        if (result.Result == TaskStatus.ServerError)
                        {
                            btn_switch.Text = "Занят. Задачи на перевозку вам не распределяются.";
                            btn_switch.Checked = false;
                        }
                        //to do...
                    });
                    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                    {
                        btn_switch.Text = "Свободен. У вас нет задач на перевозку груза.";
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
                    alert.SetMessage("Задачи Вам не будут распределяться. Вы действительно заняты?");
                    alert.SetPositiveButton("Ок", (senderAlert, args) =>
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

                        Task<TaskStatus> result;
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

                            result = BusyStatus();
                            if (result.Result == TaskStatus.ServerError)
                            {
                                btn_switch.Text = "Свободен. У вас нет задач на перевозку груза.";
                                btn_switch.Checked = true;
                            }
                        })
                        .SetNegativeButton("Отмена", delegate
                        {
                            btn_switch.Text = "Свободен. У вас нет задач на перевозку груза.";
                            btn_switch.Checked = true;
                            alert.Dispose();
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();

                      
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

        private async Task<TaskStatus> BusyStatus()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                TaskService.InitializeClient(client);
                var o_data = await TaskService.Abort(StaticTask.comment);

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.ResponseData.Message, ToastLength.Long).Show();
                    return TaskStatus.OK;
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();//"Unexpected character encountered while parsing value: {. Path 'ORDERS[0].last_stage_at', line 2, position 1086."
                    return TaskStatus.ServerError;

                }


            }
        }

        private async Task<TaskStatus> FreeStatus()
        {
            using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
            {
                DriverInfoService.InitializeClient(client);
                var o_data = await DriverInfoService.Free();

                if (o_data.Status == System.Net.HttpStatusCode.OK)
                {
                    //o_data.Message = "Успешно авторизован!";
                    Toast.MakeText(Activity, o_data.ResponseData.Message, ToastLength.Long).Show();
                    return TaskStatus.OK;
                   
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    return TaskStatus.ServerError;
                }
            }
        }
    }
}