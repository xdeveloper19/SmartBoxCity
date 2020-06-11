using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AlarmResponse
{
    public class ListAlarmResponse: BaseResponseObject
    {
        public ListAlarmResponse()
        {
            this.ALARMS_STATUS = new List<AlarmResponseData>();
        }
        public List<AlarmResponseData> ALARMS_STATUS { get; set; }
    }
}
