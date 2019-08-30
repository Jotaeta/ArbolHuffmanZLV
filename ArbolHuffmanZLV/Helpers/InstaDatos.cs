using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArbolHuffmanZLV.Helpers
{
    public class InstaDatos
    {
        private static InstaDatos _instance = null;
        public static InstaDatos Instance
        {
            get
            {
                if (_instance == null) _instance = new InstaDatos();
                return _instance;
            }
        }
        //public Stack <mCompresiones> = new Stack<>();
        public LogicArchivos lArchivo= new LogicArchivos;

    }
}