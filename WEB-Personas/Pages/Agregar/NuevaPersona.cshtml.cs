using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Personas;

namespace MyApp.Namespace
{
    public class NuevaPersonaModel : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost(string nombre, string rut)
        {
            ObraspersonasContext context = new ObraspersonasContext();
            Persona persona = new Persona();
            persona.Nombre = nombre;
            persona.Rut = rut;
            context.Personas.Add(persona);
            
            context.SaveChanges();
            Response.Redirect("/Listar/ListarPersonas");
        }
    }
}
