using System.Web;
using System.Web.Mvc;

namespace Laboratorio_Arbol_Huffman_y_ZLV
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
