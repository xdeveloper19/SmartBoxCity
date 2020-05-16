using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model
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
