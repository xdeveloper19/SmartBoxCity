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
        /// <summary>
        /// Код клиента.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Email клиента.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Роль клиента.
        /// </summary>
        public string Role { get; set; }
    }
}