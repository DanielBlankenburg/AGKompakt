using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BGH_Kompakt.Services.Interfaces
{
    public interface IFileDragDropTarget
    {
        void OnFileDrop(string[] files, string senderName);
    }

    public interface IFilesDropped
    {
        void OnFilesDropped(string[] files);
    }

}
