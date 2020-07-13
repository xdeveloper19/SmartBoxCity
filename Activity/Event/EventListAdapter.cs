using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Model.OrderViewModel.OrderInfoViewModel;
using SmartBoxCity.Service;
using WebService;
using WebService.Client;

namespace SmartBoxCity.Activity.Event
{
    public class EventListAdapter : BaseAdapter<EventModel>
    {
        Context context;
        List<EventModel> events;
        Android.App.FragmentTransaction manager;
        private int MaxElement;
        private List<int> listPosition;
        private bool _Clicked = false;
        private string Message;
        private const string URL = "https://smartboxcity.ru/";
        public EventListAdapter(Context Context, List<EventModel> List, FragmentManager Manager)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.events = List;
            this.Message = "";
            this.listPosition = new List<int>();
        }
        public override EventModel this[int position] => events[position];

        public override int Count => events.Count;

        public override long GetItemId(int position)
        {
            return /*events[position].id*/ position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = LayoutInflater.From(context).Inflate(Resource.Layout.EventCardView, null);

            var message = view.FindViewById<TextView>(Resource.Id.EventCardTextName);
            var btn_video = view.FindViewById<Button>(Resource.Id.btn_video);

            view.FindViewById<TextView>(Resource.Id.EventCardTextTime).Text = events[position].Time;
            view.FindViewById<TextView>(Resource.Id.EventCardTextDate).Text = events[position].Date;


            if (events[position].ContentType == "image")
            {
                btn_video.Visibility = ViewStates.Visible;
                btn_video.Enabled = true;
                message.Text = "Сделано фото";
                btn_video.Text = "Просмотреть фото";
            }
            else if (events[position].ContentType == "video")
            {
                message.Text = "Получено видео";
                btn_video.Text = "Просмотреть видео";
                btn_video.Visibility = ViewStates.Visible;
                btn_video.Enabled = true;
            }
            else
            {
                message.Text = events[position].Name;
                btn_video.Visibility = ViewStates.Invisible;
                btn_video.Enabled = false;
                btn_video.Text = "";
            }

            btn_video.Click += delegate
            {
                try
                {
                    listPosition.Add(position);
                    if (_Clicked == false)
                    {
                        _Clicked = true;
                       
                        AlertDialog.Builder alert = new AlertDialog.Builder(context);
                        alert.SetTitle("Внимание!");
                        alert.SetMessage("Вы действительно хотите открыть медиа файл?");
                        alert.SetPositiveButton("Открыть", (senderAlert, args) =>
                        {
                            
                            _Clicked = false;
                            MaxElement = listPosition.Max();
                            if (btn_video.Text == "Просмотреть фото")
                                SetPhoto(events[MaxElement].Name);
                            else
                                SetVideo(events[MaxElement].Name);
                            listPosition.Clear();
                        });
                        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        {
                            _Clicked = false;
                            listPosition.Clear();
                        });
                        Dialog dialog = alert.Create();
                        dialog.Show();
                    }
                    
                    //if (events[position].ContentType == "image")
                    //{
                    //    string Message = "Фотография успешно получена. Вы можете её посмотреть."; 
                    //}
                    //else
                    //{
                    //    string Message = "Видео успешно получено. Вы можете его просмотреть.";
                    //}
                }
                catch (Exception ex)
                {
                    Toast.MakeText(context, ex.Message, ToastLength.Long);
                }
            };
            
            return view;
        }

        private void SetVideo(string video_url)
        {
            try
            {
                LayoutInflater layoutInflater = LayoutInflater.From(context);
                View view = layoutInflater.Inflate(Resource.Layout.modal_video, null);
                var img_get_video = view.FindViewById<VideoView>(Resource.Id.img_get_video);

                var src = Android.Net.Uri.Parse(URL + video_url);
                img_get_video.SetVideoURI(src);
                img_get_video.Start();

                Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(context);
                alert1.SetTitle("Сделать видео");
                alert1.SetView(view);
                alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                {
                });
                Dialog dialog1 = alert1.Create();
                dialog1.Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long);
            }            
        }

        private void SetPhoto(string photo_url)
        {
            try
            {
                LayoutInflater layoutInflater = LayoutInflater.From(context);
                View view = layoutInflater.Inflate(Resource.Layout.modal_photo, null);
                var img_get_photo = view.FindViewById<ImageView>(Resource.Id.img_get_photo);

                var src = Android.Net.Uri.Parse(URL + photo_url);
                img_get_photo.SetImageURI(src);

                //var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.Message);
                Bitmap imageBitmap = HomeService.GetImageBitmapFromUrl(URL + photo_url);

                //SaveFileDialog dialog = new SaveFileDialog();
                //if (dialog.ShowDialog() == DialogResult.OK)
                //{
                //    int width = Convert.ToInt32(drawImage.Width);
                //    int height = Convert.ToInt32(drawImage.Height);
                //    Bitmap bmp = new Bitmap(width, height);
                //    drawImage.DrawToBitmap(bmp, new Rectangle(0, 0, width, height);
                //    bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                //}

                //Android.Provider.MediaStore.Images.Media.InsertImage(imageBitmap, "photo", "");

                img_get_photo.SetImageBitmap(imageBitmap);
                //var storageDir = new File(
                //    Environment.ExternalStorageDirectory(
                //        Environment.DIRECTORY_PICTURES
                //    ),
                //    getAlbumName()
                //);
                Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(context);
                alert1.SetView(view);
                ////
                alert1.SetCancelable(false);
                //alert1.SetPositiveButton("Скачать", (senderAlert1, args1) =>
                //{
                //    //SaveImage(imageBitmap);
                //});
                alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                {
                });
                Dialog dialog1 = alert1.Create();
                dialog1.Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long);
            }           
        }
    }
}