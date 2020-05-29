using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Model
{
    public enum ViewType
    {
        Header, List, Button
    }
    public interface IViewItemType
    {
        ViewType GetViewType();
    }
}
