using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Laboratorio_Arbol_Huffman_y_ZLV.Helpers;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class LZW
    {
        public void Comprimir(string sPath, string nombre)
        {
            using (var streamReader = new FileStream(sPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(streamReader))
                {
                    using (var streamWriter = new FileStream($"{DataInstance.Instance.sPath}\\{nombre}.lzw", FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(streamWriter))
                        {
                            var DiccionarioLetras = new Dictionary<string, string>();
                            var bufferLength = 10000;
                            var bytebuffer = new byte[bufferLength];
                            var stringLetra = string.Empty;

                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                bytebuffer = reader.ReadBytes(bufferLength);

                                for (int i = 0; i < bytebuffer.Count(); i++)
                                {
                                    stringLetra = Convert.ToString(Convert.ToChar(bytebuffer[i]));

                                    if (!DiccionarioLetras.ContainsKey(stringLetra))
                                    {
                                        var stringnum = Convert.ToString(DiccionarioLetras.Count() + 1, 2);
                                        DiccionarioLetras.Add(stringLetra, stringnum);
                                        stringLetra = string.Empty;
                                    }
                                }
                            }

                            writer.Write(Convert.ToByte(DiccionarioLetras.Count() - 1));

                            foreach (var fila in DiccionarioLetras)
                            {
                                writer.Write(Convert.ToByte(fila.Key[0]));
                            }

                            var contPosicion = 0;

                            reader.BaseStream.Position = contPosicion;
                            stringLetra = string.Empty;
                            var anterior = string.Empty;
                            var MayorBits = string.Empty;

                            var ListaCaracteres = new List<string>();

                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                bytebuffer = reader.ReadBytes(bufferLength);

                                for (int i = 0; i < bytebuffer.Count(); i++)
                                {
                                    stringLetra += Convert.ToChar(bytebuffer[i]);

                                    if (!DiccionarioLetras.ContainsKey(stringLetra))
                                    {
                                        var stringnum = Convert.ToString(DiccionarioLetras.Count() + 1, 2);
                                        DiccionarioLetras.Add(stringLetra, stringnum);
                                        ListaCaracteres.Add(DiccionarioLetras[anterior]);
                                        if (MayorBits.Length < DiccionarioLetras[anterior].Length)
                                        {
                                            MayorBits = DiccionarioLetras[anterior];
                                        }
                                        anterior = string.Empty;
                                        anterior += stringLetra.Last();
                                        stringLetra = anterior;

                                    }
                                    else
                                    {
                                        anterior = stringLetra;
                                    }
                                }
                                if (stringLetra != "")
                                {
                                    ListaCaracteres.Add(DiccionarioLetras[anterior]);
                                }
                            }


                            if (MayorBits.Length < DiccionarioLetras[stringLetra].Length)
                            {
                                MayorBits = DiccionarioLetras[stringLetra];
                            }

                            var CantidadGruposBits = MayorBits.Length % 8 == 0 ? MayorBits.Length / 8 : (MayorBits.Length / 8) + 1;

                            writer.Write(Convert.ToByte(CantidadGruposBits));

                            for (int i = 0; i < ListaCaracteres.Count(); i++)
                            {
                                ListaCaracteres[i] = ListaCaracteres[i].PadLeft(CantidadGruposBits * 8, '0');

                                for (int j = 0; j < CantidadGruposBits; j++)
                                {
                                    writer.Write(Convert.ToByte(Convert.ToInt32(ListaCaracteres[i].Substring(0, 8), 2)));

                                    ListaCaracteres[i] = ListaCaracteres[i].Substring(8, ListaCaracteres[i].Length - 8);
                                }
                            }

                        }
                    }
                }
            }
        }
        
        public void Descomprimir(string sPath, string nombre)
        {
            using (var streamReader = new FileStream(sPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(streamReader))
                {
                    using (var streamWriter = new FileStream($"{DataInstance.Instance.sPath}\\{nombre}.txt", FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(streamWriter))
                        {
                            var DiccionarioLetras = new Dictionary<int, string>();
                            var bufferLength = 10000;
                            var bytebuffer = new byte[bufferLength];

                            bytebuffer = reader.ReadBytes(1);
                            var CantidadDiccionario = Convert.ToInt32(bytebuffer[0]) + 1;

                            for (int i = 0; i < CantidadDiccionario; i++)
                            {
                                bytebuffer = reader.ReadBytes(1);
                                var Letra = Convert.ToChar(bytebuffer[0]).ToString();
                                DiccionarioLetras.Add(DiccionarioLetras.Count() + 1, Letra);
                            }

                            bytebuffer = reader.ReadBytes(1);
                            var cantidadBits = Convert.ToInt32(bytebuffer[0]);

                            var bytebufferSegundaParte = new byte[cantidadBits * 10000];

                            var nuevo = 0;
                            var auxActual = string.Empty;
                            var auxPrevio = string.Empty;

                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                bytebufferSegundaParte = reader.ReadBytes(cantidadBits * 10000);

                                for (int i = 0; i < bytebufferSegundaParte.Count(); i += cantidadBits)
                                {
                                    var auxConca = string.Empty;
                                    for (int j = 0; j < cantidadBits; j++)
                                    {
                                        auxConca += Convert.ToString(Convert.ToInt32(bytebufferSegundaParte[i + j].ToString()), 2).PadLeft(8, '0');
                                    }
                                    nuevo = Convert.ToInt32(auxConca, 2);
                                    var aux = string.Empty;

                                    if (nuevo > DiccionarioLetras.Count())
                                    {
                                        nuevo = DiccionarioLetras.Count();

                                        aux = DiccionarioLetras[nuevo];

                                        aux = aux.Substring(0, 1);
                                    }
                                    else
                                    {
                                        aux = DiccionarioLetras[nuevo];
                                    }

                                    foreach (var item in aux)
                                    {
                                        auxActual += item;

                                        if (!DiccionarioLetras.ContainsValue(auxActual))
                                        {
                                            DiccionarioLetras.Add(DiccionarioLetras.Count() + 1, auxActual);
                                            writer.Write(auxPrevio.ToArray());
                                            auxActual = string.Empty;
                                            auxActual += item;
                                        }

                                        auxPrevio = auxActual;

                                    }

                                }
                            }
                            writer.Write(auxPrevio.ToArray());
                        }
                    }
                }
            }
        }
    }
}