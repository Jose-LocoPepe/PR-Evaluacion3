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

            Materiales = context.Materiales.ToList();
        }

        public async Task<IActionResult> OnPostAsync(int obraId, int materialId, int cantidad)
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
                Material material = await context.Materiales.FindAsync(materialId);

                // if (material.CantidadTotal < cantidad)
                // {
                //     ErrorMessage = "No hay suficiente material en inventario.";
                //     return Page();
                // }
                material.CantidadTotal -= cantidad;

                // if the material is not in the obra, add it
                if (!context.Movimientos.Any(m => m.IdObra == obraId && m.IdMaterial == materialId))
                {
                    Movimiento movimiento = new ();
                    movimiento.IdObra = obraId;
                    movimiento.IdMaterial = materialId;
                    movimiento.Cantidad = cantidad;       
                    context.Movimientos.Add(movimiento);
                }
                else
                {
                    Movimiento movimiento = await context.Movimientos
                        .Where(m => m.IdObra == obraId && m.IdMaterial == materialId)
                        .FirstOrDefaultAsync();

                    movimiento.Cantidad += cantidad;
                }
                await context.SaveChangesAsync();

                // Response.Redirect($"/List/AdministrarMaterialEnObra?id={ObraId}");
                // Response.Redirect("/Index");

                // redirect to index
                return RedirectToPage("/Index");

                // return RedirectToPage(new { id = obraId });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while assigning the material: {ex.Message}");
                throw;
            }
        }
    }
}
