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
        public string Email { get; set; }

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Роль клиента.
        /// </summary>
        public string RoleName { get; set; }
    }
}