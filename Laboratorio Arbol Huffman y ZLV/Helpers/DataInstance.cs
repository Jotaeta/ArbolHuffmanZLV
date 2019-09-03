using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Helpers
{
    public class DataInstance
    {
        private static DataInstance _instance = null;
        public static DataInstance Instance
        {
            get
            {
                if (_instance == null) _instance = new DataInstance();
                return _instance;
            }
        }

    }
}