using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Personas;

namespace MyApp.Namespace
{
    public class NuevoContratoModel : PageModel
    {
        public List<Persona> Personas { get; set; } = new List<Persona>();
        public List<Obra> Obras { get; set; } = new List<Obra>();
        public void OnGet()
        {
            ObraspersonasContext context = new ObraspersonasContext();
            Personas = context.Personas.ToList();
            Obras = context.Obras.ToList();
        }
        public void OnPost(int idPersona, string nombreObra, DateTime fechaInicio, DateTime fechaTermino, int idObra, Boolean vigente)
        {
            ObraspersonasContext context = new ObraspersonasContext();
            Contrato contrato = new Contrato();
            contrato.IdPersona = idPersona;
            contrato.NumContrato = nombreObra;
            contrato.FechaInicio = fechaInicio;
            contrato.FechaFin = fechaTermino;
            contrato.IdObra = idObra;
            contrato.Vigencia = vigente;
            context.Contratos.Add(contrato);
            context.SaveChanges();
            Response.Redirect("/Listar/ListarContratos");
        }

    }
}
