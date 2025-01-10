using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Materiales.Models;
namespace MyApp.Namespace
{
    public class ListaObrasModel : PageModel
    {
        public List<Obra> Obras { get; set; }
        public void OnGet()
        {
            MaterialContext context = new ();
            Obras = context.Obras.ToList();
        }
    }
}
