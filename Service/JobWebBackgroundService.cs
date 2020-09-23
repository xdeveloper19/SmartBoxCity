using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Util;
using Android.App.Job;

namespace SmartBoxCity.Service
{
    [Service(Name = "com.xamarin.fjdtestapp.DemoJob", Permission = "android.permission.BIND_JOB_SERVICE")]

    public class JobWebBackgroundService : JobService
    {
        private const string TASK_TAG = "task-job-tag";
        public const string ACTION_PROCESS_TASKS = "SmartBoxCity.UPDATE_TASKS";
        public const string ACTION_PROCESS_CLIENT_PHOTO = "SmartBoxCity.CLIENT_PHOTO";

        MyTasksBroadcastReceiver receiver = new MyTasksBroadcastReceiver();

        public override void OnCreate()
        {
            base.OnCreate();
        }


        public override bool OnStartJob(JobParameters jobParameters)
        {
            Task.Run(() =>
            {
                switch (jobParameters.JobId)
                {
                    case 1:
                        {
                            RegisterReceiver(receiver, new IntentFilter(ACTION_PROCESS_TASKS));
                            Intent intent = new Intent();
                            intent.SetAction(ACTION_PROCESS_TASKS);
                            SendBroadcast(intent);
                            break;
                        }
                    case 2:
                        {
                            RegisterReceiver(receiver, new IntentFilter(ACTION_PROCESS_CLIENT_PHOTO));
                            Intent intent = new Intent();
                            intent.SetAction(ACTION_PROCESS_CLIENT_PHOTO);
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

        public override bool OnStopJob(JobParameters jobParameters)
        {
            Log.Debug(TASK_TAG, "DemoJob::OnStartJob");
            UnregisterReceiver(receiver);
            // nothing to do.
            return true;
        }
    }
}