using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Laboratorio_Arbol_Huffman_y_ZLV.Models;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class ArbolHuffman
    {
        public void Insertar(List<nodoArbol> ListaNodo)
        {
            while(ListaNodo.Count != 1)
            {
                nodoArbol nodoAux = new nodoArbol();

                nodoAux.Frecuencia = ListaNodo[0].Frecuencia + ListaNodo[1].Frecuencia;

                nodoAux.nodoIzquierdo = ListaNodo[1];
                nodoAux.nodoDerecho = ListaNodo[0];

                ListaNodo.RemoveRange(0, 2);
                ListaNodo.Add(nodoAux);
                ListaNodo.Sort();
            }

            Dictionary<byte, string> DiccionarioPrefijos = new Dictionary<byte, string>();


        }
        
        public void Recorrido(ref Dictionary<byte, string> DiccionarioPre, nodoArbol Raiz, ref string camino)
        {
            if (Raiz != null)
            {
                Recorrido(ref DiccionarioPre, Raiz.nodoDerecho);
                if (Raiz.Letra != 0)
                {
                    DiccionarioPre.Add(Raiz.Letra, camino);
                }
                Recorrido(ref DiccionarioPre, Raiz.nodoIzquierdo);
            }
            
        }
    }
}