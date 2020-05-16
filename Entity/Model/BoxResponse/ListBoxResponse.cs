using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class ListBoxResponse: BaseResponseObject
    {
        public ListBoxResponse()
        {
            this.CONTAINERS = new List<BoxResponse>();
        }
        public List<BoxResponse> CONTAINERS { get; set; }
    }
}
