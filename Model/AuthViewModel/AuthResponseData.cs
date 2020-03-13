using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SmartBoxCity.Model.AuthViewModel
{
    public class AuthResponseData: BaseResponseObject
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }
}