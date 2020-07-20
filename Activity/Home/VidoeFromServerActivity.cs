using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Plugin.Settings;
using WebService;
using WebService.Client;

namespace SmartBoxCity.Activity.Home
{
    public class VideoFromServerActivity : Fragment
    {
        private VideoView videoView;
        private const string URL = "https://smartboxcity.ru/";
        private string id;
        private string video_url;

        public VideoFromServerActivity(string id, string video_url)
        {
            this.id = id;
            this.video_url = video_url;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var view = inflater.Inflate(Resource.Layout.activity_video_from_server, container, false);

                //this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

                videoView = view.FindViewById<VideoView>(Resource.Id.ViewVideoFromServer);

                if(video_url == "")
                {
                    GetVideo();
                }
                else
                {
                    PlayVideoMethod();
                }
                

                var mediaController = new MediaController(Activity);
                mediaController.SetAnchorView(videoView);
                videoView.SetMediaController(mediaController);

                return view;
            }
            catch (Exception ex)
            {
                var view = inflater.Inflate(Resource.Layout.activity_errors_handling, container, false);
                var TextOfError = view.FindViewById<TextView>(Resource.Id.TextOfError);
                TextOfError.Text += "\n(Ошибка: " + ex.Message + ")";
                return view;
            }
        }
        //public override void OnStart()
        //{
        //    base.OnStart();
        //    videoView.Prepared += OnVideoPlayerPrepared;
        //    Play("MyVids/PreviewCourse.mp4");
        //}

        //public override void OnStop()
        //{
        //    base.OnStop();
        //    videoView.Prepared -= OnVideoPlayerPrepared;
        //}

        //private void OnVideoPlayerPrepared(object sender, EventArgs e)
        //{
        //    mediaController.SetAnchorView(videoView);

        //    //show media controls for 3 seconds when video starts to play
        //    mediaController.Show(3000);
        //}
        private async void GetVideo()
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.GetVideo(id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        video_url = o_data.Message;
                        PlayVideoMethod();
                        //controller = new MediaController(context);
                        //img_get_video.CanPause();
                        // controller.SetAnchorView(img_get_video);
                        //img_get_video.SetMediaController(controller);
                        //img_get_video.SetOnPreparedListener(new MediaOPlayerListener(context, img_get_video));
                        //controller.Show(50000);
                    }
                    else
                    {
                        Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }

        private void PlayVideoMethod()
        {
            try
            {
                var src = Android.Net.Uri.Parse(URL + video_url);
                videoView.SetVideoURI(src);
                videoView.Start();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }            
        }
    }
}