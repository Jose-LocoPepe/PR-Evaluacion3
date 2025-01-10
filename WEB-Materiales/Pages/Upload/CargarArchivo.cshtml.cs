using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_Materiales.Models;
namespace MyApp.Namespace
{
    public class CargarArchivoModel : PageModel
    {
        public void OnGet()
        {
        }


        public void OnPost()
        {
            List<Obra> obras = new ();

            // Create a StreamReader to read the uploaded file
            StreamReader lector = new (Request.Form.Files["archivo"].OpenReadStream());
            string linea = lector.ReadLine();
            while (linea != null)
            {
                string[] datos = linea.Split(",");
                if (datos.Length == 2 && int.TryParse(datos[0], out int id))
                {
                    Obra obra = new ()
                    {
                        Id = id,
                        Nombre = datos[1]
                    };
                    obras.Add(obra);
                }
                linea = lector.ReadLine();
            }
            lector.Close();

            // Guardar en la base de datos
            MaterialContext context = new ();
            
            foreach (Obra obra in obras)
            {
                if (!context.Obras.Any(o => o.Id == obra.Id))
                {
                    context.Obras.Add(obra);
                }
            }
            context.SaveChanges();

            Response.Redirect("/List/ListaObras");
        }

    }
}
