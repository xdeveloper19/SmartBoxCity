using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.OrderResponse
{
    public class GeoResponseData: BaseResponseObject
    {
        public GeoResponseData()
        {
            this.MAP_WAYPOINTS = new List<GeoLocation<string>>();
        }
        public PartitialOrderResponse ORDER { get; set; }
        public List<GeoLocation<string>> MAP_WAYPOINTS { get; set; }
    }
}
