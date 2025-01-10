using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Obras.Models;
using Microsoft.EntityFrameworkCore;
namespace MyApp.Namespace
{
    public class AdministrarTareasEnObraModel : PageModel
    {
        public List<Tarea> Tareas { get; set; }
        public Obra Obra { get; set; }

        public void OnGet(int id)
        {
            ObrasContext context = new ();
            Obra = context.Obras.Find(id);
            Tareas = context.Tareas
                .Where(t => t.IdObra == id)
                .ToList();
        }

        public void OnPost(int obraId, string nombre, float porcentajeAvance)
        {
            try
            {
                ObrasContext context = new ();

                // update the percentage of the obra that includes all the tareas
                // Obra obra = context.Obras.Find(obraId);
                Obra obra = context.Obras
                    .Include(o => o.Tareas)
                    .FirstOrDefault(o => o.Id == obraId);
                int totalTareas = obra.Tareas.Count() + 1;
                float totalPorcentajeSumado = obra.Tareas.Select(t => t.PorcentajeAvance).Sum();
                float nuevoPorcentaje = (totalPorcentajeSumado + porcentajeAvance) / totalTareas;

                obra.AvanceGeneral = nuevoPorcentaje;
                context.Obras.Update(obra);

                Tarea tarea = new ();
                tarea.Nombre = nombre;
                tarea.PorcentajeAvance = porcentajeAvance;
                tarea.IdObra = obraId;

                context.Tareas.Add(tarea);
                context.SaveChanges();

                Response.Redirect("/Index");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while adding the task: {ex.Message}");
                throw;
            }
        }
    }
}
