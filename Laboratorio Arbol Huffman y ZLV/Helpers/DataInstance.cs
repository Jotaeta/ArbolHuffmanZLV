using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Laboratorio_Arbol_Huffman_y_ZLV.Models;
using System.IO;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Helpers
{
    public class DataInstance
    {
        private static DataInstance _instance = null;
        public static DataInstance Instance
        {
            get
            {
                if (_instance == null) _instance = new DataInstance();
                return _instance;
            }
        }

        public string sPath;
        public string sPathManejo;
        public string Ext;
        public Archivos ClaseArchivo = new Archivos();
        public ArbolHuffman ClaseArbol = new ArbolHuffman();
        public LZW ClaseLZW = new LZW();
        public List<Historial> listaArchivo = new List<Historial>();

        //Carga el historial que se muestra en pantalla por medio de una lista
        public void CargarLista()
        {
            using (var Reader = new StreamReader(Path.Combine(sPathManejo, "ManejoArchivos.txt")))
            {
                var Linea = "";
                while (!Reader.EndOfStream)
                {
                    var historialtemp = new Historial();
                    Linea = Reader.ReadLine();
                    historialtemp.NombreArchivo = Linea;
                    Linea = Reader.ReadLine();
                    historialtemp.RazonCompre = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.FactorCompre = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.PorcentajeRedu = Convert.ToDouble(Linea);
                    Linea = Reader.ReadLine();
                    historialtemp.TipoArchivo = Convert.ToString(Linea);
                    listaArchivo.Add(historialtemp);
                }
            }
        }

        //Maneja los documentos en el historial para mantener el mismo despues de cerrar la aplicacion
        public void ManejoArchivos(double bytesNuevo, double bytesActual, string nombre, string tipo)
        {

            using (var Writer = new StreamWriter(Path.Combine(sPathManejo, "ManejoArchivos.txt")))
            {
                foreach (var item in listaArchivo)
                {
                    Writer.WriteLine(item.NombreArchivo);
                    Writer.WriteLine(item.RazonCompre);
                    Writer.WriteLine(item.FactorCompre);
                    Writer.WriteLine(item.PorcentajeRedu);
                    Writer.Write(item.TipoArchivo);
                }
                Writer.WriteLine(nombre);
                Writer.WriteLine(bytesNuevo / bytesActual);
                Writer.WriteLine(bytesActual / bytesNuevo);
                Writer.WriteLine((1 - bytesNuevo / bytesActual));
                Writer.WriteLine(tipo);
            }

        }
    }
}