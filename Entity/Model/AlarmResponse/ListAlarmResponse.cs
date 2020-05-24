using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AlarmResponse
{
    public class ListAlarmResponse: BaseResponseObject
    {
        public ListAlarmResponse()
        {
            this.ALARMS = new List<AlarmResponseData>();
        }
        public List<AlarmResponseData> ALARMS { get; set; }
    }
}
