using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AccountViewModel.AuthResponse
{
    public class RegisterResponse: BaseResponseObject
    {
        public string message { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string birthday { get; set; }
        public string created_at { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string last_activity_at { get; set; }
        public string last_login_at { get; set; }
        public string last_name { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string organization_bank { get; set; }
        public string organization_bank_bik { get; set; }
        public string organization_bank_correspondent { get; set; }
        public string organization_bank_payment { get; set; }
        public string organization_inn { get; set; }
        public string organization_kpp { get; set; }
        public string organization_legal_address { get; set; }
        public string organization_name { get; set; }
        public string organization_ogrn { get; set; }
        public string organization_phone { get; set; }
        public string organization_postal_address { get; set; }
        public string passport_code { get; set; }
        public string passport_date { get; set; }
        public string passport_id { get; set; }
        public string passport_serie { get; set; }
        public string password { get; set; }
        public string patronymic { get; set; }
        public string phone { get; set; }
        public string token { get; set; }
        public string type { get; set; }
        public int user_role_id { get; set; }
    }
}
