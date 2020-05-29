using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.TaskViewModel
{
    public class CurrentTaskModel : IViewItemType
    {
        public string Order_Id { get; set; }
        public string Description { get; set; }
        public ViewType GetViewType()
        {
            return ViewType.Header;
        }
    }
}
