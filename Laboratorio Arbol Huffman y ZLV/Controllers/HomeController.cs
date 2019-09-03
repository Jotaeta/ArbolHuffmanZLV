using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public RedirectResult SubirArchivo(HttpPostedFileBase fArchivo)
        {
            if (fArchivo == null) return new RedirectResult("Index", false);

            //Direccion de archivo

            var sPath = fArchivo.FileName;



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