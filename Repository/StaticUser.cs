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
using SmartBoxCity.Model.AuthViewModel;

namespace SmartBoxCity.Repository
{
    public static class StaticUser
    {
        /// <summary>
        /// Код клиента.
        /// </summary>
        public static string UserId { get; set; }

        /// <summary>
        /// Email клиента.
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// Имя клиента.
        /// </summary>
        public static string FirstName { get; set; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public static string LastName { get; set; }

        /// <summary>
        /// Отчество клиента.
        /// </summary>
        public static string MiddleName { get; set; }

        /// <summary>
        /// Телефон клиента.
        /// </summary>
        public static string PhoneNumber { get; set; }

        /// <summary>
        /// Почта клиента.
        /// </summary>
        public static string Email { get; set; }

        /// <summary>
        /// Подтверждение почты.
        /// </summary>
        public static bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Роль клиента.
        /// </summary>
        public static string UserRole { get; set; }


        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект авторизации/регистрации</param>
        public static void AddInfoAuth(AuthResponseData o_auth)
        {
            UserId = o_auth.UserId;
            UserName = o_auth.UserName;
            FirstName = o_auth.FirstName;
            LastName = o_auth.LastName;
            UserRole = o_auth.Role;
        }
    }
}