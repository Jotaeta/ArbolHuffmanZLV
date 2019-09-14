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
        public void Descomprimir(string sPath, string nombre)
        {
            var TablaPrefijos = new Dictionary<byte, string>();
            using (var streamReader = new FileStream(sPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(streamReader))
                {
                    int bufferLength = 8;

                    var byteBuffer = new byte[bufferLength];

                    byteBuffer = reader.ReadBytes(8);

                    var Extension = Encoding.UTF8.GetString(byteBuffer).TrimStart('0');
                    DataInstance.Instance.Ext = Extension;

                    using (var streamWriter = new FileStream($"{DataInstance.Instance.sPath}\\{nombre}{Extension}", FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(streamWriter))
                        {
                            byteBuffer = reader.ReadBytes(8);
                            var cantDiccionario = Convert.ToInt32(Encoding.UTF8.GetString(byteBuffer));

                            bufferLength = 1;

                            byteBuffer = reader.ReadBytes(bufferLength);

                            for (int i = 0; i < cantDiccionario; i++)
                            {
                                var camino = new List<byte>();

                                var letra = byteBuffer[0];

                                byteBuffer = reader.ReadBytes(bufferLength);

                                bool DentroCamino = true;

                                while (DentroCamino)
                                {
                                    if (byteBuffer[0] != 124)
                                    {
                                        camino.Add(byteBuffer[0]);
                                    }
                                    else
                                    {
                                        DentroCamino = false;
                                    }
                                    byteBuffer = reader.ReadBytes(bufferLength);
                                }

                                TablaPrefijos.Add(letra, Encoding.UTF8.GetString(camino.ToArray()));
                            }


                            bufferLength = 1;

                            var PosiblesCaracteres = new List<string>();
                            var TempPosiblesCaracteres = new List<string>();
                            var AuxCadena = "";

                            foreach (var item in TablaPrefijos)
                            {
                                PosiblesCaracteres.Add(item.Value);
                            }

                            TempPosiblesCaracteres = PosiblesCaracteres.ToList();

                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                var Linea = ObtenerBinario(Convert.ToString(byteBuffer[0])).PadLeft(8, '0');

                                while (Linea.Length > 0)
                                {
                                    
                                    AuxCadena += Linea.Substring(0, 1);
                                    Linea = Linea.Substring(1);

                                    bool EliminacionCompleta = false;
                                    var contLista = 0;

                                    while (!EliminacionCompleta && contLista < TempPosiblesCaracteres.Count())
                                    {
                                        if (TempPosiblesCaracteres[contLista].Substring(0, AuxCadena.Length) != AuxCadena)
                                        {
                                            TempPosiblesCaracteres.RemoveAt(contLista);
                                            contLista = 0;
                                        }
                                        else
                                        {
                                            contLista++;
                                        }
                                    }

                                    foreach (var item in TablaPrefijos)
                                    {
                                        if (item.Value == AuxCadena)
                                        {
                                            writer.Write(item.Key);
                                            AuxCadena = "";
                                            TempPosiblesCaracteres = PosiblesCaracteres.ToList();
                                        }
                                    }
                                }
                                byteBuffer = reader.ReadBytes(1);
                            }
                        }
                    }
                }
            }

        }

        public string ObtenerBinario(string Snumero)
        {
            var numero = Convert.ToInt32(Snumero);
            var Aux = "";
            var binario = "";

            while ((numero >= 2))
            {
                Aux = Aux + (numero % 2).ToString();
                numero = numero / 2;
            }
            Aux = Aux + numero.ToString();

            for (int i = Aux.Length; i >= 1; i += -1)
            {
                binario = binario + Aux.Substring(i - 1, 1);
            }

            return binario;
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
                    const int bufferLength = 10;

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

            DataInstance.Instance.ClaseArbol.Insertar(ListaNodosArbol, Path.GetFileNameWithoutExtension(sPath), sPath, Path.GetExtension(sPath));
        }
    }
}