using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmartBoxCity.Activity.Order;

namespace SmartBoxCity.Activity.Driver
{
    public class TaskActivity: Fragment
    {
        private Button btn_interrupt;

        private Button btn_perform;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.driver_tasks, container, false);
            //Button btn_about_order = view.FindViewById<Button>(Resource.Id.btn_about_order);
            btn_perform = view.FindViewById<Button>(Resource.Id.btn_prime2);
            btn_interrupt = view.FindViewById<Button>(Resource.Id.btn_prime);

            FragmentTransaction transaction1 = this.FragmentManager.BeginTransaction();

            //btn_about_order.Click += async delegate
            //{
            //    OrderActivity content = new OrderActivity();
            //    transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
            //};

            btn_perform.Click += async delegate
            {
                TaskNotFoundActivity content = new TaskNotFoundActivity();
                transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
            };

            btn_interrupt.Click += async delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(Context);
                View view = layoutInflater.Inflate(Resource.Layout.driver_confirm_task, null);
                AlertDialog.Builder alert = new AlertDialog.Builder(Context);
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
                    TaskNotFoundActivity content = new TaskNotFoundActivity();
                    transaction1.Replace(Resource.Id.frameDriverlayout, content).AddToBackStack(null).Commit();
                })
                .SetNegativeButton("Отмена", delegate
                {
                    alert.Dispose();
                });
                Dialog dialog = alert.Create();
                dialog.Show();

            };
            return view;
        }
    }
}