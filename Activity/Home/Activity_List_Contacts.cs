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
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity.Menu
{
    public class Activity_List_Contacts : Fragment
    {
        private TextView EditTextTopContact;

        private TextView TextContacts;

        private ImageView ImageViewContacts;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_list_contacts, container, false);

            try
            {

                EditTextTopContact = view.FindViewById<TextView>(Resource.Id.TextTopContact);
                TextContacts = view.FindViewById<TextView>(Resource.Id.TextContacts);
                ImageViewContacts = view.FindViewById<ImageView>(Resource.Id.ImageViewContacts);
                AboutMethod();

            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }
        private async void AboutMethod()
        {
            using (var client = ClientHelper.GetClient())
            {
                HomeService.InitializeClient(client);
                var o_data = await HomeService.ContactUs();

                if (o_data.Status == HttpStatusCode.OK)
                {
                    ContactsResponseData o_user_data = new ContactsResponseData();
                    o_user_data = o_data.ResponseData;
                    TextContacts.Text = o_user_data.Message;
                    var src = Android.Net.Uri.Parse(o_user_data.Image);
                    ImageViewContacts.SetImageURI(src);

                    var imageBitmap = HomeService.GetImageBitmapFromUrl(o_user_data.Image);
                    ImageViewContacts.SetImageBitmap(imageBitmap);

                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            };
        }
    }
}