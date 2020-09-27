using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Repository;
using Plugin.Settings;
using WebService;
using WebService.Client;
using WebService.Driver;

namespace SmartBoxCity.Activity.Home
{
    public class VideoFromServerActivity: Fragment
    {
        private VideoView videoView;
        private const string URL = "https://smartboxcity.ru/";
        private ProgressBar preloader;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var view = inflater.Inflate(Resource.Layout.activity_video_from_server, container, false);

                //this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

                videoView = view.FindViewById<VideoView>(Resource.Id.ViewVideoFromServer);
                preloader = view.FindViewById<ProgressBar>(Resource.Id.preloader);

                PlayVideoMethod();

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
       

        private void PlayVideoMethod()
        {
            try
            {
                //controller = new MediaController(context);
                //img_get_video.CanPause();
                // controller.SetAnchorView(img_get_video);
                //img_get_video.SetMediaController(controller);
                //img_get_video.SetOnPreparedListener(new MediaOPlayerListener(context, img_get_video));
                //controller.Show(50000);
                preloader.Visibility = ViewStates.Visible;

                var src = Android.Net.Uri.Parse(URL + StaticOrder.File_Name);
                videoView.SetVideoURI(src);
                var mediaController = new MediaController(Activity);
                mediaController.SetAnchorView(videoView);
                videoView.SetMediaController(mediaController);
                videoView.SetOnPreparedListener(new MediaOnPlayerListener(mediaController, preloader));
                videoView.Start();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
            }
        }
    }

    public class MediaOnPlayerListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        private MediaController controller;
        private ProgressBar preloader;

        public MediaOnPlayerListener(MediaController controller, ProgressBar preloader)
        {
            this.controller = controller;
            this.preloader = preloader;
        }


        public bool CanPause()
        {
            //throw new NotImplementedException();
            return true;
        }

        public bool CanSeekBackward()
        {
            //throw new NotImplementedException();
            return false;
        }

        public bool CanSeekForward()
        {
            return false;
        }

        //public IntPtr Handle => throw new NotImplementedException();

        //public int JniIdentityHashCode => throw new NotImplementedException();

        //public JniObjectReference PeerReference => throw new NotImplementedException();

        //public JniPeerMembers JniPeerMembers => throw new NotImplementedException();

        //public JniManagedPeerStates JniManagedPeerState => throw new NotImplementedException();

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Disposed()
        {
            //throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            //throw new NotImplementedException();
        }

        public void Finalized()
        {
            //throw new NotImplementedException();
        }

        public void OnPrepared(MediaPlayer mp)
        {
            //MediaController controller = new MediaController(context);
            //mediaPlayer = new MediaPlayer();
            //mediaPlayer = mp;
            ////img_get_video.CanPause();
            //controller.SetMediaPlayer(this);
            //controller.SetAnchorView(img_get_video);
            ////img_get_video.SetMediaController(controller);
            preloader.Visibility = ViewStates.Invisible;

            controller.Enabled = true;
            controller.Show(0);
            mp.Start();
            //throw new NotImplementedException();
        }

        public void Pause()
        {
            //throw new NotImplementedException();
        }

        public void SeekTo(int pos)
        {
            //throw new NotImplementedException();
        }

        public void SetJniIdentityHashCode(int value)
        {
            // throw new NotImplementedException();
        }



        public void Start()
        {
            //throw new NotImplementedException();
        }

        public void UnregisterFromRuntime()
        {
            //throw new NotImplementedException();
        }
    }
}