using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.HomeViewModel
{
    public class ContactsResponse: BaseResponseObject
    {
        public string Image { get; set; }
        public string Message { get; set; }
    }
}
