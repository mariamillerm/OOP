using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace lab1._2.Shapes
{
    interface ISelectable
    {
        //bool isSelected { get; set; }
        void Frame(Canvas canvas);
    }
}
