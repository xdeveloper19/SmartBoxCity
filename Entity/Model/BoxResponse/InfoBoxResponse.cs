using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class InfoBoxResponse: BaseResponseObject
    {
        public InfoBoxResponse()
        {
            this.ALARMS_STATUS = new List<AlarmResponse>();
        }
        public PartContainerResponse CONTAINER { get; set; }
        public string EVENT_COUNT { get; set; }
        public List<AlarmResponse> ALARMS_STATUS { get; set; }
        public SensorResponse SENSORS_STATUS { get; set; }
    }
}
