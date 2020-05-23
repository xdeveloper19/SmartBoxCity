using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model
{
    public class GeoLocation<T> 
    {
        public T lat { get; set; }
        public T lng { get; set; }
    }
}
