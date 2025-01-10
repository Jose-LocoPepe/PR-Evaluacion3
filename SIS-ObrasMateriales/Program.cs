using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SIS_ObrasMateriales
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Exportando obras...");
                using var context = new ObraspersonasContext();

                var obras = context.Obras.ToList();

                if (obras.Count == 0)
                {
                    Console.WriteLine("No hay obras en la base de datos.");
                    return;
                }

                // Ruta del archivo
                string filePath = "obras_exportadas.txt";

                // Leer las obras existentes en el archivo, si el archivo existe
                var obrasEnArchivo = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (File.Exists(filePath))
                {
                    obrasEnArchivo = new HashSet<string>(
                        File.ReadAllLines(filePath),
                        StringComparer.OrdinalIgnoreCase
                    );
                }

                int totalObras = obras.Count;
                int exportadas = 0;

                // Abrir el archivo en modo de agregar
                using (var writer = new StreamWriter(filePath, append: true))
                {
                    foreach (var obra in obras)
                    {
                        // Verificar si la obra ya está en el archivo
                        if (!obrasEnArchivo.Contains(obra.Nombre))
                        {
                            writer.WriteLine(obra.Nombre); // Agregar obra al archivo
                            exportadas++;
                        }
                    }
                }

                Console.WriteLine($"Se han exportado {exportadas} de {totalObras} obras correctamente.");
                Console.WriteLine($"Archivo generado en: {Path.GetFullPath(filePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la exportación: {ex.Message}");
            }
        }
    }
}
