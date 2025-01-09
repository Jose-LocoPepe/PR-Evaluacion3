using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Personas;

namespace MyApp.Namespace
{
    public class ListarPersonasModel : PageModel
    {
        public List<Persona> Personas { get; set; }
        public void OnGet()
        {
            ObraspersonasContext context = new ObraspersonasContext();
            Personas = context.Personas.ToList();
        }
    }
}
