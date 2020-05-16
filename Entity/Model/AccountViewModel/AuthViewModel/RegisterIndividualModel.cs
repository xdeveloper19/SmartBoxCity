using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AccountViewModel.AuthViewModel
{
    public class RegisterIndividualModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ClientType { get; set; }
        public string ClientLastName { get; set; }
        public string ClientName { get; set; }
        public string ClientPatronymic { get; set; }
        public string ClientBirthday { get; set; }
        public string ClientPassportSerie { get; set; }
        public string ClientPassportId { get; set; }
        public string ClientPassportCode { get; set; }
    }
}
