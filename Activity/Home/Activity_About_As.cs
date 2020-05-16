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
using Entity.Model;
using Entity.Model.AccountViewModel.AuthViewModel;
using SmartBoxCity.Service;
using WebService;

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
                Toast.MakeText(Activity, "" + ex.Message, ToastLength.Long).Show();
            }
            return view;
        }

        private async void AboutMethod()
        {
            using (var client = ClientHelper.GetClient())
            {
                WebService.Home.HomeService.InitializeClient(client);
                var o_data = await WebService.Home.HomeService.About();

                if (o_data.Status == HttpStatusCode.OK)
                {
                    SuccessResponse o_user_data = new SuccessResponse();
                    o_user_data = o_data.ResponseData;
                    TextAboutUs.Text = o_user_data.Message;
                }
                else
                {
                    Toast.MakeText(Activity, o_data.Message, ToastLength.Long).Show();
                }
            };
        }
    }
}