using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class Archivos
    {
        public void Comprimir(string sPath)
        {
            Dictionary<byte, double> TablaLetras = new Dictionary<byte, double>();

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
                    foreach (var letra in TablaLetras)
                    {
                        TablaLetras[letra.Key] = letra.Value / Convert.ToDouble(TablaLetras.Count);
                    }
                }
            }
        }
    }
}