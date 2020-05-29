using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model.TaskViewModel
{
    public class TaskBookModel: IViewItemType
    {
        public string order_id { get; set; }

        public string priority { get; set; }

        public string address { get; set; }

        public string title { get; set; }

        public ViewType GetViewType()
        {
            return ViewType.List;
        }
    }
}
