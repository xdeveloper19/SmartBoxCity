using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class InfoBoxResponse: BaseResponseObject
    {
        public PartContainerResponse CONTAINER { get; set; }
        public SensorResponse SENSORS_STATUS { get; set; }
    }
}
