using Entity.Model.AlarmResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.BoxResponse
{
    public class InfoBoxResponse: BaseResponseObject
    {
        public InfoBoxResponse()
        {
            this.ALARMS_STATUS = new List<AlarmResponseData>();
        }
        public PartContainerResponse CONTAINER { get; set; }
        public string EVENT_COUNT { get; set; }
        public List<AlarmResponseData> ALARMS_STATUS { get; set; }
        public SensorResponse SENSORS_STATUS { get; set; }
    }
}
