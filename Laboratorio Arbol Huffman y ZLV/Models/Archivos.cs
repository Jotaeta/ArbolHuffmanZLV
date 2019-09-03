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
            Dictionary<char, double> TablaLetras = new Dictionary<char, double>();

            //using (var LeerArchivo = new FileStream(sPath, FileMode.OpenOrCreate))
            //{

            //}

            using (var stream = new FileStream(sPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[10000];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(10000);
                        //writer.Write(byteBuffer);
                    }
                }
            }
        }
    }
}