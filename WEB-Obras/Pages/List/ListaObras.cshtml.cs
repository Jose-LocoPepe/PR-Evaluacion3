using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Obras.Models;
namespace MyApp.Namespace
{
    public class ListaObrasModel : PageModel
    {
        public List<Obra> Obras { get; set; }
        public void OnGet()
        {
            ObrasContext context = new ();
            Obras = context.Obras.ToList();
        }
    }
}
