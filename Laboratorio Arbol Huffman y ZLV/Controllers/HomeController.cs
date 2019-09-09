﻿using System;
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
            DataInstance.Instance.sPath = Server.MapPath("~/Archivos");
            var directoryInfo = new DirectoryInfo(DataInstance.Instance.sPath);
            var files = directoryInfo.GetFiles("*.*");
            var listaArchivo = new List<Historial>();

            foreach (var item in files)
            {
                listaArchivo.Add( new Historial { sNombreArchivo = item.Name });
            }

            return View(listaArchivo);
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
                DataInstance.Instance.ClaseArchivo.Descomprimir(sPath);
            }
            else
            {
                DataInstance.Instance.ClaseArchivo.Comprimir(sPath);
            }

            return new RedirectResult("Index", false);
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