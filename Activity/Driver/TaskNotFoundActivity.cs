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
    }
}