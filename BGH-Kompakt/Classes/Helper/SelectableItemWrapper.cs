using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.Helper
{
    internal class SelectableItemWrapper<T> where T : class
    {
        public bool IsSelected { get; set; }
        public T Item { get; set; }
    }
}
