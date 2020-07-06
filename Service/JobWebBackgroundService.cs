using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Entity.Repository;
using Firebase.JobDispatcher;
using Plugin.Settings;
using SmartBoxCity.Activity.Driver;
using WebService;
using WebService.Driver;

namespace SmartBoxCity.Service
{
    [Service(Name = "com.xamarin.fjdtestapp.DemoJob")]
    [IntentFilter(new[] { FirebaseJobServiceIntent.Action })]
    public class JobWebBackgroundService : JobService
    {
        private const string TASK_TAG = "task-job-tag";
        public const string ACTION_PROCESS_TASKS = "SmartBoxCity.UPDATE_TASKS";
        MyTasksBroadcastReceiver receiver = new MyTasksBroadcastReceiver();

        public override void OnCreate()
        {
            base.OnCreate();
        }


        public override bool OnStartJob(IJobParameters jobParameters)
        {
            Task.Run(() =>
           {
               switch (jobParameters.Tag)
               {
                   case TASK_TAG:
                       {
                           RegisterReceiver(receiver, new IntentFilter(ACTION_PROCESS_TASKS));
                           Intent intent = new Intent();
                           intent.SetAction(ACTION_PROCESS_TASKS);
                           SendBroadcast(intent);
                           break;
                       }
                   default:
                       break;
               }
               JobFinished(jobParameters, true);
           });
            // Return true because of the asynchronous work
            return true;
        }

        public override bool OnStopJob(IJobParameters jobParameters)
        {
            Log.Debug(TASK_TAG, "DemoJob::OnStartJob");
            UnregisterReceiver(receiver);
            // nothing to do.
            return true;
        }
    }
}