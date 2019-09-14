using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio_Arbol_Huffman_y_ZLV.Helpers;
using Laboratorio_Arbol_Huffman_y_ZLV.Models;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Obtiene direccion de donde se almacenan los archivos y los datos de historial
            DataInstance.Instance.sPath = Server.MapPath("~/Archivos");
            DataInstance.Instance.sPathManejo = Server.MapPath("~/Historial");
            var directoryInfo = new DirectoryInfo(DataInstance.Instance.sPath);
            var files = directoryInfo.GetFiles("*.*");

            //Carga la lista que muestra la vista de los archivos comprimidos y descomprimidos
            DataInstance.Instance.CargarLista();

            return View(DataInstance.Instance.listaArchivo);
        }


        [HttpPost]
        public RedirectResult SubirArchivo(HttpPostedFileBase fArchivo)
        {
            if (fArchivo == null) return new RedirectResult("Index", false);

            //Direccion de archivo
            var sPath = fArchivo.FileName;
            /////
            //Decide si el archivo se debe de comprimir o descomprimir
            if (Path.GetExtension(fArchivo.FileName) == ".huff")
            {
                DataInstance.Instance.ClaseArchivo.Descomprimir(sPath, Path.GetFileNameWithoutExtension(sPath));
                var NameCompre = Path.GetFileNameWithoutExtension(sPath);
                var fileActual = new FileInfo($"{DataInstance.Instance.sPath}\\{NameCompre}{DataInstance.Instance.Ext}");
                var fileDescompre = new FileInfo(sPath);
                DataInstance.Instance.ManejoArchivos((double)fileDescompre.Length, (double)fileActual.Length, $"{NameCompre}{DataInstance.Instance.Ext}");
            }
            else
            {
                DataInstance.Instance.ClaseArchivo.Comprimir(sPath);
                var NameCompre = Path.GetFileNameWithoutExtension(sPath);
                var fileComprimido = new FileInfo($"{DataInstance.Instance.sPath}\\{NameCompre}.huff");
                var fileActual = new FileInfo(sPath);
                DataInstance.Instance.ManejoArchivos((double)fileComprimido.Length, (double)fileActual.Length, $"{NameCompre}.huff");
            }
            DataInstance.Instance.listaArchivo.Clear();
            return new RedirectResult("Index", false);
        }

        public ActionResult Descargar (string filename)
        {
            var fullpath = Path.Combine(DataInstance.Instance.sPath, filename);
            return File(fullpath, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        #region OTRAS FUNCIONES
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion
    }
}