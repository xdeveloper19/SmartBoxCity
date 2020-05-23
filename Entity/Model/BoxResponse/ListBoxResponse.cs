using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class ListBoxResponse: BaseResponseObject
    {
        public ListBoxResponse()
        {
            this.CONTAINERS = new List<ContainerResponse>();
            this.DEPOT_CONTAINERS = new List<ContainerResponse>();
        }
        public List<ContainerResponse> CONTAINERS { get; set; }
        public List<ContainerResponse> DEPOT_CONTAINERS { get; set; }
    }
}
