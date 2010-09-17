using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hiMapNet
{
    static public class MapTools
    {
        static MapToolAddEdit mapToolAddEdit;
        public static MapToolAddEdit MapToolAddEdit
        {
            get 
            {
                if (mapToolAddEdit == null) mapToolAddEdit = new MapToolAddEdit();
                return mapToolAddEdit; 
            }
        }
        /*
        static MapToolSelectMove mapToolSelectMove;
        public static MapToolSelectMove MapToolSelectMove
        {
            get
            {
                if (mapToolSelectMove == null) mapToolSelectMove = new MapToolSelectMove();
                return mapToolSelectMove;
            }
        }*/


        static MapToolInfo mapToolInfo;
        public static MapToolInfo MapToolInfo(List<LayerAbstract> layers)
        {
            if (mapToolInfo == null) mapToolInfo = new MapToolInfo(layers);
            return mapToolInfo;
        }


    }
}
