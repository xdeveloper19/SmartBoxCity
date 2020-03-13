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
    public class RegisterLegalModul
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string OrgPostalAddress { get; set; }
        public string ClientType { get; set; }
        public string ClientLastName { get; set; }
        public string ClientName { get; set; }
        public string ClientPatronymic { get; set; }
        public string OrgPhone { get; set; }
        public string OrgName { get; set; }
        public string OrgKpp { get; set; }
        public string OrgInn { get; set; }
        public string OrgOgrn { get; set; }
        public string OrgBank { get; set; }
        public string OrgBankpayment { get; set; }
        public string OrgBankCorrespondent { get; set; }
        public string OrgBankBik { get; set; }
        public string OrgLegalAddress { get; set; }
    }
}