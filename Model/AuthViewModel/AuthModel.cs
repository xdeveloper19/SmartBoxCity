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
    public class AuthModel
    {
        /// <summary>
        /// Логин клиента.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        public string Password { get; set; }
    }
}