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

        public void OnGet(int id)
        {
            MaterialContext context = new ();
            
            Obra = context.Obras.Find(id);
            Movimientos = context.Movimientos
                .Where(m => m.IdObra == id)
                .Include(m => m.IdMaterialNavigation)
                .ToList();
        }
    }
}
