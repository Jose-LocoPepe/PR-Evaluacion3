using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Materiales.Models;
namespace MyApp.Namespace
{
    public class AgregarMaterialModel : PageModel
    {
        public void OnPost(string nombre, int cantidad)
        {
            try
            {
                MaterialContext context = new();
                Material material = new()
                {
                    Nombre = nombre,
                    CantidadTotal = cantidad
                };
                context.Materiales.Add(material);
                context.SaveChanges();
                Response.Redirect("/Index");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while adding the material: {ex.Message}");
                throw;
            }
        }
    }
}
