using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArbolHuffmanZLV.Models
{
    public class LogicArchivos
    {
        public void Comprimir(string Lector)
        {
            List<frecuencia> lfrecuencia = new List<frecuencia>();
            using (var lector = new StreamReader(Lector))
            {
                int contador = 0;
                while (!lector.EndOfStream)
                {
                    contador++;
                    String linea = Convert.ToString(lector.ReadLine());
                    for (int i = 0; i < linea.Length; i++)
                    {
                        ContarLetras(linea.Substring(i, 1), ref frecuencia);
                    }
                }
                if (contador > 1)
                {
                    frecuencia.Add(new frecuencia { letra = "/n", frecuencia = contador - 1 });
                }
                frecuencia.Sort();
            }
        }

        public void ContarLetras(string letra, ref List<frecuencia> fre)
        {
            bool BuscarLetra = true, ExisteLetra = true;
            int cont = 0;
            while (BuscarLetra && ExisteLetra)
            {

            }
        }

    }
}