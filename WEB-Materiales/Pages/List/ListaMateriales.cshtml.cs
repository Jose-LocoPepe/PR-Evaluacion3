using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Materiales.Models;
namespace MyApp.Namespace
{
    public class ListaMaterialesModel : PageModel
    {
        public List<Material> Materiales { get; set; }
        public void OnGet()
        {
            MaterialContext context = new();
            Materiales = context.Materiales.ToList();
        }
    }
}
