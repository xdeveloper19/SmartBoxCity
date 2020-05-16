using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AccountViewModel.AuthViewModel
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
