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

namespace SmartBoxCity.Model
{
    public class ListResponse<T1,T2>: BaseResponseObject
    {
        public ListResponse()
        {
            this.ORDERS = new List<T1>();
            this.ARCHIVE = new List<T2>();
        }
        public List<T1> ORDERS { get; set; }
        public List<T2> ARCHIVE { get; set; }
    }
}