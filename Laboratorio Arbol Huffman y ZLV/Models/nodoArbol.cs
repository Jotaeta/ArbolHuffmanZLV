using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class nodoArbol : IComparable
    {
        public char Letra { get; set; }

        public double Frecuencia { get; set; }

        public int CompareTo(object obj)
        {
            var comparador = (nodoArbol)obj;
            return Frecuencia.CompareTo(comparador.Frecuencia);
        }
    }
}