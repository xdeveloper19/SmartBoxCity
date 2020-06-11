using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.AlarmViewModel
{
    public class AlarmBookModel
    {
        public string Acknowledged { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Raised_At { get; set; }
        public string Container_id { get; set; }
    }
}
