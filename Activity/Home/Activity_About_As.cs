using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmartBoxCity.Model.AuthViewModel;
using SmartBoxCity.Service;

namespace SmartBoxCity.Activity
{
    
    public class Activity_About_As : Fragment
    {
        private TextView TextAboutUsTop;

        private TextView TextAboutUs;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.title_about_us, container, false);

            try
            {

                TextAboutUsTop = view.FindViewById<TextView>(Resource.Id.TextAboutUsTop);
                TextAboutUs = view.FindViewById<TextView>(Resource.Id.TextAboutUs);
                AboutMethod();

            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }

        private async void AboutMethod()
        {
            using (var client = ClientHelper.GetClient())
            {
                HomeService.InitializeClient(client);
                var o_data = await HomeService.About();

                if (o_data.Status == HttpStatusCode.OK)
                {
                    RegisterResponseData o_user_data = new RegisterResponseData();
                    o_user_data = o_data.ResponseData;
                    TextAboutUs.Text = o_user_data.Message;
                }
                else
                {
                    Toast.MakeText(Context, o_data.Message, ToastLength.Long).Show();
                }
            };
        }
    }
}