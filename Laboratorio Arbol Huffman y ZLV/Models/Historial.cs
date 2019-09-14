using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Laboratorio_Arbol_Huffman_y_ZLV.Models
{
    public class Historial 
    {
        //Variables a manejar  en el historial
        [Display(Name = "Nombre del archivo")]
        public string NombreArchivo { get; set; }

        [Display(Name = "Razon de compresion")]
        public double RazonCompre { get; set; }

        [Display(Name = "Factor de compresion")]
        public double FactorCompre { get; set; }

        [Display(Name = "Porcentaje de reduccion")]
        public double PorcentajeRedu { get; set; }
    }
}