using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class nodoArbol : IComparable
    {
        public byte Letra { get; set; }

        public double Frecuencia { get; set; }

        public nodoArbol nodoIzquierdo { get; set; }

        public nodoArbol nodoDerecho { get; set; }

        //Auxiliar para sort
        public int CompareTo(object obj)
        {
            var comparador = (nodoArbol)obj;
            return Frecuencia.CompareTo(comparador.Frecuencia);
        }
    }
}