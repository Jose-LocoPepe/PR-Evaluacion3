using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_Personas;

namespace MyApp.Namespace
{
    public class ListarContratosModel : PageModel
    {
        // Asegúrate de que esta propiedad sea pública
        public List<PersonaConContratosVigentes> PersonasConContratos { get; set; } = new List<PersonaConContratosVigentes>();

        public void OnGet()
        {
            using var context = new ObraspersonasContext();

            PersonasConContratos = context.Personas
                .Include(p => p.Contratos)
                .ThenInclude(c => c.IdObraNavigation)
                .Where(p => p.Contratos.Any(c => c.Vigencia))
                .Select(p => new PersonaConContratosVigentes
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Rut = p.Rut,
                    Contratos = p.Contratos
                        .Where(c => c.Vigencia)
                        .Select(c => new ContratoConObra
                        {
                            NumContrato = c.NumContrato,
                            FechaInicio = c.FechaInicio,
                            FechaFin = c.FechaFin,
                            NombreObra = c.IdObraNavigation.Nombre
                        })
                        .ToList()
                })
                .ToList();
        }
    }

    public class PersonaConContratosVigentes
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rut { get; set; }
        public List<ContratoConObra> Contratos { get; set; }
    }

    public class ContratoConObra
    {
        public string NumContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string NombreObra { get; set; }
    }
}
