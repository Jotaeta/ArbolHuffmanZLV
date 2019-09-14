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
        //Crea el arbol huffman para luego empezar la compresion
        public void Insertar(List<nodoArbol> ListaNodo, string nombre, string ArchivoActual, string extension)
        {
            while(ListaNodo.Count != 1)
            {
                var nodoAux = new nodoArbol();

                nodoAux.Frecuencia = ListaNodo[0].Frecuencia + ListaNodo[1].Frecuencia;

                nodoAux.nodoIzquierdo = ListaNodo[1];
                nodoAux.nodoDerecho = ListaNodo[0];

                ListaNodo.RemoveRange(0, 2);
                ListaNodo.Add(nodoAux);
                ListaNodo.Sort();
            }

            var DiccionarioPrefijos = new Dictionary<byte, string>();
            var camino = "";

            Recorrido( ref DiccionarioPrefijos, ListaNodo[0], camino);

            ComprimirArchivo(DiccionarioPrefijos, nombre, ArchivoActual, extension);
        }
        ////Recorre el arbol para obtener el camino
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
        ///Realiza la logica para leer el archivo original y comprimir
        public void ComprimirArchivo(Dictionary<byte, string> DiccionarioClave, string nombre, string ArchivoActual, string Extension)
        {
            var Path = $"{DataInstance.Instance.sPath}\\{nombre}.huff";

            using (var streamReader = new FileStream(ArchivoActual, FileMode.Open))
            {
                using (var reader = new BinaryReader(streamReader))
                {
                    using (var streamWriter = new FileStream(Path, FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(streamWriter))
                        {
                            writer.Write(Encoding.UTF8.GetBytes(Extension.PadLeft(8, '0').ToCharArray()));
                            writer.Write(Encoding.UTF8.GetBytes(Convert.ToString(DiccionarioClave.Count).PadLeft(8, '0').ToCharArray()));

                            foreach (var item in DiccionarioClave)
                            {
                                writer.Write(item.Key);
                                
                                var aux = $"{item.Value}|";

                                writer.Write(aux.ToCharArray());
                            }

                            //Traduce las letras del doc original al codigo ASCII
                            const int bufferLength = 10;

                            var byteBuffer = new byte[bufferLength];
                            var CadenaAux = "";
                            
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLength);

                                foreach (var LetraRecibida in byteBuffer)
                                {
                                    foreach (var Clave in DiccionarioClave)
                                    {
                                        if (LetraRecibida == Clave.Key)
                                        {
                                            CadenaAux += Clave.Value;
                                            if (CadenaAux.Length / 8 != 0)
                                            {
                                                for (int i = 0; i < CadenaAux.Length / 8; i++)
                                                {
                                                    var NuevaCadena = CadenaAux.Substring(0, 8);
                                                    writer.Write((byte)Convert.ToInt32(NuevaCadena, 2));
                                                    CadenaAux = CadenaAux.Substring(8);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (CadenaAux.Length <= 8)
                            {
                                writer.Write((byte)Convert.ToInt32(CadenaAux.PadRight(8, '0'), 2));
                            }
                            
                        }
                    }
                }
            }
        }
    }
}