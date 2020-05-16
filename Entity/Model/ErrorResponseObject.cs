using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model
{
    public class ErrorResponseObject: BaseResponseObject
    {
        public List<string> Errors { get; set; }
    }
}
