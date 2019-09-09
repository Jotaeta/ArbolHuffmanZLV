using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Laboratorio_Arbol_Huffman_y_ZLV.Models;
using Laboratorio_Arbol_Huffman_y_ZLV.Helpers;
using System.Text;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class ArbolHuffman
    {
        public void Insertar(List<nodoArbol> ListaNodo, string nombre, string ArchivoActual)
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
            string camino = "";

            Recorrido( ref DiccionarioPrefijos, ListaNodo[0], camino);

            ComprimirArchivo(DiccionarioPrefijos, nombre, ArchivoActual);
        }
        ////      
        public void Recorrido(ref Dictionary<byte, string> DiccionarioPre, nodoArbol Raiz, string camino)
        {
            if (Raiz != null)
            {
                var caminoDer = $"{camino}1";
                Recorrido(ref DiccionarioPre, Raiz.nodoDerecho, caminoDer);
                if (Raiz.Letra != 0)
                {
                    DiccionarioPre.Add(Raiz.Letra, camino);
                }
                var caminoIzq = $"{camino}0";
                Recorrido(ref DiccionarioPre, Raiz.nodoIzquierdo, caminoIzq);
            }

        }
        ////
        ///
        public void ComprimirArchivo(Dictionary<byte, string> DiccionarioClave, string nombre, string ArchivoActual)
        {
            string Path = $"{DataInstance.Instance.sPath}\\{nombre}.huff";

            using (var streamReader = new FileStream(ArchivoActual, FileMode.Open))
            {
                using (var reader = new BinaryReader(streamReader))
                {
                    using (var streamWriter = new FileStream(Path, FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(streamWriter))
                        {
                            var PosicionInicial = 0;
                            foreach (var item in DiccionarioClave)
                            {
                                //var binLetra = string.Format("{0, 8}", Convert.ToString(Convert.ToInt32(item.Key), 2));
                                //var binCamino = string.Format("{0, 8}", Convert.ToString(Convert.ToInt32(item.Value), 2));

                                //var key = Encoding.UTF8.GetBytes(item.Key.ToString("00000000; -00000000"));
                                //var value = Encoding.UTF8.GetBytes(string.Format("{0, 8}", item.Value));

                                streamWriter.Seek(PosicionInicial, SeekOrigin.Begin);
                                PosicionInicial += 8;
                                writer.Write(item.Key);
                                streamWriter.Seek(PosicionInicial, SeekOrigin.Begin);
                                PosicionInicial += 9;
                                writer.Write(Convert.ToInt32(item.Value, 2).ToString());
                            }
                            writer.Seek(PosicionInicial, SeekOrigin.Begin);
                            writer.Write(Encoding.ASCII.GetBytes("fi"));
                            PosicionInicial += 8;
                            const int bufferLength = 100;

                            var byteBuffer = new byte[bufferLength];
                            bool prueba = (reader.BaseStream.Position != reader.BaseStream.Length);
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLength);

                                foreach (var letra in byteBuffer)
                                {
                                    foreach (var item in DiccionarioClave)
                                    {
                                        if (letra == item.Key)
                                        {
                                            writer.Write(Convert.ToInt32(item.Value, 2).ToString());
                                            writer.Seek(PosicionInicial, SeekOrigin.Begin);
                                            PosicionInicial += 8;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}