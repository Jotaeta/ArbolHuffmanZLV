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
                                    stringLetra += Convert.ToString(Convert.ToChar(bytebuffer[i]));

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
                            }

                            ListaCaracteres.Add(DiccionarioLetras[stringLetra]);

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
    }
}