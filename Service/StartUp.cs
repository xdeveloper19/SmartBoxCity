using Android.App;
using Android.Icu.Util;
using Android.Util;
using Android.Widget;
using Firebase.JobDispatcher;

namespace SmartBoxCity.Service
{
    public class StartUp
    {
        static readonly string TAG = "X:StartService";
        static FirebaseJobDispatcher dispatcher;

        
        /// <summary>
        /// Запуск задачи.
        /// </summary>
        public static void StartTracking(string tag)
        {
            Log.Debug(TAG, "Starting Tracking");

            // This is the "Java" way to create a FirebaseJobDispatcher object
            IDriver driver = new GooglePlayDriver(Application.Context);

          
            dispatcher = new FirebaseJobDispatcher(driver);

            //RetryStrategy retry = dispatcher.NewRetryStrategy(RetryStrategy.RetryPolicyLinear, retryTime, deadline);
            //long second = TimeUnit.SECONDS.toSeconds(seconds) - (TimeUnit.SECONDS.toMinutes(seconds) * 60);
            JobTrigger myTrigger = Trigger.ExecutionWindow(0, 5);

            // FirebaseJobDispatcher dispatcher = context.CreateJobDispatcher();
            Job myJob = dispatcher.NewJobBuilder()
                       .SetService<JobWebBackgroundService>(tag)
                       .SetLifetime(Lifetime.Forever)
                       .SetTrigger(myTrigger)
                       .AddConstraint(Constraint.OnAnyNetwork)
                       .Build();
            
            // This method will not throw an exception; an integer result value is returned
            int scheduleResult = dispatcher.Schedule(myJob);
            //dispatcher.MustSchedule(myJob);

            Log.Debug(TAG, "Scheduling LocationJobService...");

            if (scheduleResult != FirebaseJobDispatcher.ScheduleResultSuccess)
            {
                Log.Warn(TAG, "Job Scheduler failed to schedule job!");
                Toast.MakeText(Application.Context, "Job Scheduler failed to schedule job!", ToastLength.Long);
            }
        }


        /// <summary>
        /// Отмена задачи.
        /// </summary>
        public static void StopTracking()
        {
            Log.Debug(TAG, "Stopping Tracking");

            int cancelResult = dispatcher.CancelAll();

            // to cancel a single job:

            //int cancelResult = dispatcher.Cancel("unique-tag-for-job");
        }
    }
}