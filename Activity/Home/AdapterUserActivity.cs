using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity.Model;
using Entity.Repository;
using Java.Nio.FileNio;
using Plugin.Settings;
using SmartBoxCity.Activity.Order;
using SmartBoxCity.Service;
using WebService;
using WebService.Client;

namespace SmartBoxCity.Activity.Home
{
    public class AdapterUserActivity : BaseAdapter<OrderAdapter>
    {
        Context context;
        List<OrderAdapter> orders;
        Android.App.FragmentTransaction manager;
        private TextView Cost;
        private TextView txtFrom;
        private TextView txtTo;
        private Button btn_pay;
        private Button btn_pass_delivery_service;
        private Button btn_make_photo;
        private Button btn_make_video;
        private Button btn_order_management;
        private ProgressBar progress;
        private int MaxElement;
        private bool _Clicked = false;
        private List<int> listPosition;
        private TextView NameContainer;
        private TextView Statusview;
        private TextView Payment;
        private const string URL = "https://smartboxcity.ru/";


        public AdapterUserActivity(Context Context, List<OrderAdapter> List, FragmentManager Manager)
        {
            this.manager = Manager.BeginTransaction();
            this.context = Context;
            this.orders = List;
            listPosition = new List<int>();
        }
        public override OrderAdapter this[int position] => orders[position];

        public override int Count => orders.Count;

        public override long GetItemId(int position)
        {
            return orders[position].Id;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return orders[position];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            try
            {
                View view = convertView;
                if (view == null)
                    view = LayoutInflater.From(context).Inflate(Resource.Layout.activity_user_CardView, null);

                txtFrom = view.FindViewById<TextView>(Resource.Id.UserCardViewTxtFrom);
                txtTo = view.FindViewById<TextView>(Resource.Id.UserCardViewTxtTo);
                NameContainer = view.FindViewById<TextView>(Resource.Id.container_name);
                Statusview = view.FindViewById<TextView>(Resource.Id.status_view);
                Cost = view.FindViewById<TextView>(Resource.Id.s_cost);
                Payment = view.FindViewById<TextView>(Resource.Id.s_payment);
                Payment.Text = orders[position].payment_status;
                Cost.Text = orders[position].payment_amount;
                NameContainer.Text = orders[position].id;
                Statusview.Text = orders[position].order_stage_id + ". " + orders[position].order_stage_name;

                btn_pay = view.FindViewById<Button>(Resource.Id.btn_pay);
                progress = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
                btn_pass_delivery_service = view.FindViewById<Button>(Resource.Id.btn_pass_delivery_service);
                btn_make_photo = view.FindViewById<Button>(Resource.Id.btn_make_photo);
                btn_make_video = view.FindViewById<Button>(Resource.Id.btn_make_video);
                btn_order_management = view.FindViewById<Button>(Resource.Id.btn_order_management);

                txtFrom.Text = orders[position].inception_address;
                txtTo.Text = orders[position].destination_address;

                if (Payment.Text == "1")
                {
                    Payment.Text = "Оплачено";
                    Payment.SetTextColor(Color.ParseColor("#8EF892"));
                }
                else
                {
                    Payment.Text = "Не оплачено";
                    Payment.SetTextColor(Color.ParseColor("#EC8F9B"));
                }

                btn_pass_delivery_service.Click += delegate
                {
                    try
                    {
                        MainOrderStatusActivity content = new MainOrderStatusActivity();
                        StaticOrder.Order_id = orders[position].id;
                        StaticOrder.Payment_Amount = orders[position].payment_amount;
                        StaticOrder.Payment_Status = orders[position].payment_status;
                        StaticOrder.Event_Count = orders[position].event_count;
                        manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null);
                        manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                    }
                };

                btn_pay.Enabled = (orders[position].order_stage_id == "5") ? true : false;
                btn_make_photo.Enabled = (orders[position].order_stage_id == "7" ||
                    orders[position].order_stage_id == "8" ||
                     orders[position].order_stage_id == "1") ? false : true;

                btn_make_video.Enabled = (orders[position].order_stage_id == "7" ||
                    orders[position].order_stage_id == "8" ||
                     orders[position].order_stage_id == "1") ? false : true;

                btn_pay.Click += delegate
                {
                    try
                    {
                        listPosition.Add(position);
                        if (_Clicked == false)
                        {
                            _Clicked = true;
                            AlertDialog.Builder alert = new AlertDialog.Builder(context);
                            alert.SetTitle("Внесение оплаты");
                            alert.SetMessage("Вы действительно хотите оплатить заказ?");
                            alert.SetPositiveButton("Продолжить", (senderAlert, args) =>
                            {
                                _Clicked = false;
                                MaxElement = listPosition.Max();
                                StaticOrder.Order_id = orders[MaxElement].id;
                                MakePayment(alert);
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
                        //if (Payment.Text == "неизвестно")
                        //{
                        //    Toast.MakeText(context, "В настоящий момент невозможно использовать эту кнопку!\nПричина: Неизвестно состояние об оплате.", ToastLength.Long).Show();
                        //}
                        //else
                        //{
                        //    AlertDialog.Builder alert = new AlertDialog.Builder(context);
                        //    alert.SetTitle("Внесение оплаты");
                        //    alert.SetMessage("Вы действительно хотите оплатить заказ?");
                        //    alert.SetPositiveButton("Продолжить", (senderAlert, args) =>
                        //    {
                        //        StaticOrder.Order_id = orders[position].id;
                        //        MakePayment(alert);
                        //    });
                        //    alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        //    {
                        //    });
                        //    Dialog dialog = alert.Create();
                        //    dialog.Show();
                        //}
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                    }
                };
                //btn_make_photo.Click += MakePhotoClick;
                btn_make_photo.Click += delegate
                {
                    try
                    {
                        listPosition.Add(position);
                        if (_Clicked == false)
                        {
                            _Clicked = true;
                            AlertDialog.Builder alert = new AlertDialog.Builder(context);
                            alert.SetTitle("Сделать фотографию");
                            alert.SetMessage("Вы действительно хотите сделать фотографию с камеры контейнера?");
                            alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                            {
                                _Clicked = false;
                                MaxElement = listPosition.Max();
                                GetPhoto(orders[MaxElement].id, alert);
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
                        //AlertDialog.Builder alert = new AlertDialog.Builder(context);
                        //alert.SetTitle("Сделать фотографию");
                        //alert.SetMessage("Вы действительно хотите сделать фотографию с камеры контейнера?");
                        //alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                        //{
                        //    GetPhoto(orders[position].id, alert);
                        //});
                        //alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        //{
                        //});
                        //Dialog dialog = alert.Create();
                        //dialog.Show();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                    }
                };
                btn_make_video.Click += delegate
                {
                    try
                    {
                        listPosition.Add(position);
                        if (_Clicked == false)
                        {
                            _Clicked = true;
                            AlertDialog.Builder alert = new AlertDialog.Builder(context);
                            alert.SetTitle("Сделать видео");
                            alert.SetMessage("Вы действительно хотите сделать видео с камеры контейнера?");
                            alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                            {
                                _Clicked = false;
                                MaxElement = listPosition.Max();
                                GetVideo(orders[MaxElement].id, alert);
                                listPosition.Clear();

                            });
                            alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                            {
                                listPosition.Clear();
                                _Clicked = false;
                            });
                            Dialog dialog = alert.Create();
                            dialog.Show();
                        }
                        //AlertDialog.Builder alert = new AlertDialog.Builder(context);
                        //alert.SetTitle("Сделать видео");
                        //alert.SetMessage("Вы действительно хотите сделать видео с камеры контейнера?");
                        //alert.SetPositiveButton("Сделать", (senderAlert, args) =>
                        //{
                        //    GetVideo(orders[position].id, alert);
                        //});
                        //alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                        //{
                        //});
                        //Dialog dialog = alert.Create();
                        //dialog.Show();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                    }
                };
                btn_order_management.Click += delegate
                {
                    try
                    {
                        ManageOrderActivity content = new ManageOrderActivity();
                        StaticOrder.Order_id = orders[position].id;
                        StaticOrder.Payment_Amount = orders[position].payment_amount;
                        StaticOrder.Payment_Status = orders[position].payment_status;
                        StaticOrder.Event_Count = orders[position].event_count;
                        manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                    }

                };

                int order_stage;
                var result = int.TryParse(orders[position].order_stage_id, out order_stage);

                if (result == true)
                    progress.Progress = order_stage;
                else
                    progress.Progress = 0;
                //btn.Click += async delegate
                //{
                //    OrderActivity content = new OrderActivity();
                //    manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null).Commit();
                //};

                return view;
            }
            catch (Exception ex)
            {
                var view = convertView;
                var TextOfError = view.FindViewById<TextView>(Resource.Id.TextOfError);
                TextOfError.Text += "\n(Ошибка: " + ex.Message + ")";
                return view;
            }            
        }

        private void BtnMethodGetPhoto(object sender, EventArgs e, int position)
        {

        }

        //private void MakePhotoClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        AlertDialog.Builder alert = new AlertDialog.Builder(context);
        //        alert.SetTitle("Сделать фотографию");
        //        alert.SetMessage("Вы действительно хотите сделать фотографию с камеры контейнера?");
        //        alert.SetPositiveButton("Сделать", (senderAlert, args) =>
        //        {
        //            GetPhoto(orders[position].id, alert);
        //        });
        //        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
        //        {
        //        });
        //        Dialog dialog = alert.Create();
        //        dialog.Show();
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
        //    }
        //}

        //private EventHandler MakePhotoClick(int position)
        //{
        //    try
        //    {
        //        AlertDialog.Builder alert = new AlertDialog.Builder(context);
        //        alert.SetTitle("Сделать фотографию");
        //        alert.SetMessage("Вы действительно хотите сделать фотографию с камеры контейнера?");
        //        alert.SetPositiveButton("Сделать", (senderAlert, args) =>
        //        {
        //            GetPhoto(orders[position].id, alert);
        //        });
        //        alert.SetNegativeButton("Отмена", (senderAlert, args) =>
        //        {
        //        });
        //        Dialog dialog = alert.Create();
        //        dialog.Show();
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
        //    }
        //}

        private async void GetVideo(string id, AlertDialog.Builder alert)
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
                        alert.Dispose();

                        LayoutInflater layoutInflater = LayoutInflater.From(context);
                        View view = layoutInflater.Inflate(Resource.Layout.modal_video, null);
                        var img_get_video = view.FindViewById<VideoView>(Resource.Id.img_get_video);

                        var src = Android.Net.Uri.Parse(URL + o_data.Message);
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
                    else
                    {
                        Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }            
        }

        private async void GetPhoto(string id, AlertDialog.Builder alert)
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.GetPhoto(id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        alert.Dispose();

                        LayoutInflater layoutInflater = LayoutInflater.From(context);
                        View view = layoutInflater.Inflate(Resource.Layout.modal_photo, null);
                        var img_get_photo = view.FindViewById<ImageView>(Resource.Id.img_get_photo);

                        var src = Android.Net.Uri.Parse(URL + o_data.Message);
                        img_get_photo.SetImageURI(src);

                        //var imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.Message);
                        Bitmap imageBitmap = HomeService.GetImageBitmapFromUrl(URL + o_data.Message);

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
                        alert1.SetPositiveButton("Скачать", (senderAlert1, args1) =>
                        {
                            SaveImage(imageBitmap);
                        });
                        alert1.SetNegativeButton("Закрыть", (senderAlert1, args1) =>
                        {
                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();
                    }
                    else
                    {
                        Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }         
        }

        //private string GetRealPathFromURI(Android.Net.Uri uri)
        //{
        //    string doc_id = "";
        //    using (var c1 = ContentResolver.Query(uri, null, null, null, null))
        //    {
        //        c1.MoveToFirst();
        //        string document_id = c1.GetString(0);
        //        doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
        //    }

        //    string path = null;

        //    // The projection contains the columns we want to return in our query.
        //    string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
        //    using (var cursor = ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
        //    {
        //        if (cursor == null) return path;
        //        var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
        //        cursor.MoveToFirst();
        //        path = cursor.GetString(columnIndex);
        //    }
        //    return path;
        //}

        private void SaveImage(Bitmap imageBitmap)
        {

            try
            {
                #region 1-й метод
                byte[] bitmapData;
                var stream = new MemoryStream();
                imageBitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);
                string path = MediaStore.Images.Media.InsertImage(context.ContentResolver, imageBitmap, "screen", "shot");
                #endregion

                #region 2-й метод
                //var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                //documentsPath = System.IO.Path.Combine(documentsPath, "Orders", "temp");
                //Directory.CreateDirectory(documentsPath);
                //string FileName = "Image" + DateTime.Today.Day.ToString()
                //            + DateTime.Today.Month.ToString()
                //            + DateTime.Today.Year.ToString()
                //            + ".png";
                //string filePath = System.IO.Path.Combine(documentsPath, FileName);

                //byte[] bitmapData;
                //var stream = new MemoryStream();
                //imageBitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);
                //bitmapData = stream.ToArray();

                //using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                //{
                //    int length = bitmapData.Length;
                //    fs.Write(bitmapData, 0, length);
                //}
                #endregion

                //bitmapData = stream.ToArray();
                //var fileContent = new ByteArrayContent(bitmapData);
                //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                //fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                //{
                //    Name = "file",
                //    FileName = "Image" + DateTime.Today.Day.ToString()
                //            + DateTime.Today.Month.ToString()
                //            + DateTime.Today.Year.ToString()
                //            + ".png",
                //};   
                //string boundary = "---8d0f01e6b3b5dafaaadaad";
                //MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
                //multipartContent.Add(fileContent);

                // var basePath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
                //    if (path != null)
                //{
                //    var uriPath = Android.Net.Uri.Parse(path);
                //    var realPath = GetRealPathFromURI(uriPath);
                //    imageBitmap = loadAndResizeBitmap(realPath);
                //}
                //{
                #region 3-й метод
                //byte[] bitmapData;
                //var stream = new MemoryStream();
                //imageBitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);
                //bitmapData = stream.ToArray();

                //string localPath = string.Empty;
                //String fname = "Image" + DateTime.Today.Day.ToString()
                //    + DateTime.Today.Month.ToString()
                //    + DateTime.Today.Year.ToString()
                //    + ".png";

                //var path = MediaStore.Images.Media.InsertImage(context.ContentResolver, imageBitmap, "screen", "shot");
                //if (path != null)
                //{
                //    var uriPath = Android.Net.Uri.Parse(path);
                //    var realPath = GetRealPathFromURI(uriPath);
                //    bitmap = loadAndResizeBitmap(realPath);
                //    photobox.SetImageBitmap(bitmap);
                //}

                //localPath = System.IO.Path.Combine(Android.OS.Environment.
                //    GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath, fname);

                //System.IO.File.WriteAllBytes(path, bitmapData);
                #endregion
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }

        }
        //public async static Task SaveImage(this byte[] image, String fileName, IFolder rootFolder = null)
        //{
        //    // get hold of the file system  
        //    IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;

        //    // create a file, overwriting any existing file  
        //    IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

        //    // populate the file with image data  
        //    using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
        //    {
        //        stream.Write(image, 0, image.Length);
        //    }
        //}

        private async void MakePayment(AlertDialog.Builder alert)
        {
            try
            {
                using (var client = ClientHelper.GetClient(CrossSettings.Current.GetValueOrDefault("token", "")))
                {
                    ManageOrderService.InitializeClient(client);
                    var o_data = new ServiceResponseObject<SuccessResponse>();
                    o_data = await ManageOrderService.MakePayment(StaticOrder.Order_id);

                    if (o_data.Status == HttpStatusCode.OK)
                    {
                        alert.Dispose();
                        Android.App.AlertDialog.Builder alert1 = new Android.App.AlertDialog.Builder(context);
                        alert1.SetTitle("Внесение оплаты");
                        alert1.SetMessage(o_data.ResponseData.Message);
                        alert1.SetPositiveButton("Закрыть", (senderAlert1, args1) =>
                        {
                            try
                            {
                                UserActivity content = new UserActivity();
                                manager.Replace(Resource.Id.framelayout, content).AddToBackStack(null);
                                manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Toast.MakeText(context, ex.Message, ToastLength.Long);
                            }
                        });
                        Dialog dialog1 = alert1.Create();
                        dialog1.Show();
                    }
                    else
                    {
                        Toast.MakeText(context, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
            }          
        }       
    }
}