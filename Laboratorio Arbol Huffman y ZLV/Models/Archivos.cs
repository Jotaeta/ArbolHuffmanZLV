using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Laboratorio_Arbol_Huffman_y_ZLV.Helpers;
using System.Text;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class Archivos
    {
        public void Descomprimir(string sPath)
        {
            var TablaPrefijos = new Dictionary<byte, decimal>();
            using (var stream = new FileStream(sPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    const int bufferLength = 17;
                    var byteBuffer = new byte[bufferLength];
                    byteBuffer = reader.ReadBytes(bufferLength);

                    bool DiccionarioTerminado = false;
                    
                    while (!DiccionarioTerminado)
                    {
                        var lineValor = new byte();
                        var lineCamino = new byte();
                        Decimal Dec = 0;

                            lineValor += byteBuffer[0];
                            lineCamino += byteBuffer[9];
                            Dec = lineCamino;
                        if (byteBuffer[0] == Encoding.ASCII.GetBytes("fi")[0])
                        {
                            DiccionarioTerminado = true;
                        }
                        else
                        {
                            TablaPrefijos.Add(lineValor, Dec);
                        }
                    }                    
                }
            }

        }

        public void Comprimir(string sPath)
        {
            var TablaLetras = new Dictionary<byte, double>();

            var ListaNodosArbol = new List<nodoArbol>();

            using (var stream = new FileStream(sPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    //Cantidad de letras en buffer
                    const int bufferLength = 100;

                    var byteBuffer = new byte[bufferLength];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);

                        foreach (var letra in byteBuffer)
                        {
                            if (TablaLetras.ContainsKey(letra))
                            {
                                TablaLetras[letra]++;
                            }
                            else
                            {
                                TablaLetras.Add(letra, 1);
                            }
                        }
                    }

                    double totalLetras = 0;

                    foreach (var letra in TablaLetras)
                    {
                        totalLetras += letra.Value;
                    }

                    foreach (var letra in TablaLetras)
                    {
                        ListaNodosArbol.Add(new nodoArbol { Letra = letra.Key, Frecuencia = letra.Value / totalLetras});
                    }

                    ListaNodosArbol.Sort();

                    
                }
            }

            DataInstance.Instance.ClaseArbol.Insertar(ListaNodosArbol, Path.GetFileNameWithoutExtension(sPath), sPath);
        }
    }
}