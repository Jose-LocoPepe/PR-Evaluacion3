using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Materiales.Models;
using Microsoft.EntityFrameworkCore;
namespace MyApp.Namespace
{
    public class AdministrarMaterialEnObraModel : PageModel
    {
        public Obra Obra { get; set; }
        public List<Movimiento> Movimientos { get; set; }
        public List<Material> Materiales { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet(int id)
        {
            MaterialContext context = new ();
            
            Obra = context.Obras.Find(id);
            Movimientos = context.Movimientos
                .Where(m => m.IdObra == id)
                .Include(m => m.IdMaterialNavigation)
                .ToList();

            if (Obra != null)
            {
                Movimientos = Obra.Movimientos.ToList();
            }
            Materiales = context.Materiales.ToList();
        }

        public void OnPost(int ObraId, int materialId, int cantidad)
        {
            try
            {
                // if (cantidad <= 0)
                // {
                //     ErrorMessage = "La cantidad debe ser un número positivo.";
                //     OnGet(ObraId); // Reload data
                //     return;
                // }

                MaterialContext context = new ();

                // reducir material en inventario
                Material material = context.Materiales.Find(materialId);

                // if (material.CantidadTotal < cantidad)
                // {
                //     ErrorMessage = "No hay suficiente material en inventario.";
                //     OnGet(ObraId); // Reload data
                // }
                material.CantidadTotal -= cantidad;
                // context.SaveChanges();

                Movimiento movimiento = new ()
                {
                    IdObra = ObraId,
                    IdMaterial = materialId,
                    Cantidad = cantidad,
                    IdObraNavigation = context.Obras.Find(ObraId),
                    IdMaterialNavigation = material
                };
                context.Movimientos.Add(movimiento);
                context.SaveChanges();

                // Response.Redirect($"/List/AdministrarMaterialEnObra?id={ObraId}");
                Response.Redirect("/Index");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while adding the client: {ex.Message}");
                throw;
            }
        }
    }
}
