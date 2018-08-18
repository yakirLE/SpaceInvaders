using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C16_Ex03_Yakir_201049475_Omer_300471430.Infrastructure.CustomEventArgs
{
    public class MenuToggleItemEventArgs : EventArgs
    {
        public object CurrentToggleItem { get; set; }
    }
}
