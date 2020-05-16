using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AccountViewModel.AuthViewModel
{
    public class AuthResponse: BaseResponseObject
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
