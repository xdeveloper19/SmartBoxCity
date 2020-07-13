using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Util;
using Android.Widget;
using Firebase.JobDispatcher;

namespace SmartBoxCity.Service
{
    public static class StartUp
    {
        static readonly string TAG = "X:StartService";


        /// <summary>
        /// Запуск задачи.
        /// </summary>
        public static void StartTracking(Context context, string tag)
        {
            Log.Debug(TAG, "Starting Tracking");

            var jobBuilder = CreateJobBuilderUsingJobId<JobWebBackgroundService>(context, 1);

            var jobInfo = jobBuilder
                .SetRequiredNetworkType(NetworkType.Any)
                //.SetImportantWhileForeground(true)
                .SetPersisted(true)
                .SetBackoffCriteria(5000, BackoffPolicy.Linear)
                .SetPeriodic(10000)
                .Build();  // creates a JobInfo object.

            var jobScheduler = (JobScheduler)context.GetSystemService(Context.JobSchedulerService);
            var scheduleResult = jobScheduler.Schedule(jobInfo);

            if (JobScheduler.ResultSuccess == scheduleResult)
            {
                Toast.MakeText(context, "SUCCESS", ToastLength.Long);
            }
            else
            {
                Toast.MakeText(context, "FAILED", ToastLength.Long);
            }

            Log.Debug(TAG, "Scheduling LocationJobService...");
        }

        public static JobInfo.Builder CreateJobBuilderUsingJobId<T>(this Context context, int jobId) where T : Android.App.Job.JobService
        {
            var javaClass = Java.Lang.Class.FromType(typeof(T));
            var componentName = new ComponentName(context, javaClass);

            // Sample usage - creates a JobBuilder for a DownloadJob and sets the Job ID to 1.
            return new JobInfo.Builder(jobId, componentName);
        }


        /// <summary>
        /// Отмена задачи.
        /// </summary>
        public static void StopTracking(Context context)
        {
            Log.Debug(TAG, "Stopping Tracking");
            var jobScheduler = (JobScheduler)context.GetSystemService(Context.JobSchedulerService);

            jobScheduler.CancelAll();
        }
    }


}