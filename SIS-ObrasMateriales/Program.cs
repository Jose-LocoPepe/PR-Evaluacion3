using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace SIS_ObrasMateriales {
    class Program {
        static void Main(string[] args) {
            var context = new MaterialesContext();
            bool salir = false;

            while (!salir) {
                Console.WriteLine("\n--- Sistema de Materiales ---");
                Console.WriteLine("1. Listar Materiales");
                Console.WriteLine("2. Agregar Material");
                Console.WriteLine("3. Mover Material");
                Console.WriteLine("4. Importar Obras");
                Console.WriteLine("5. Salir");
                Console.Write("Opción: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        ListarMateriales(context);
                        break;
                    case "2":
                        AgregarMaterial(context);
                        break;
                    case "3":
                        MoverMaterial(context);
                        break;
                    case "4":
                        ImportarObras(context);
                        break;
                    case "5":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static void ListarMateriales(MaterialesContext context)
        {
            var materiales = context.Materials.ToList();
            Console.WriteLine("\n--- Lista de Materiales ---");
            foreach (var material in materiales)
            {
                Console.WriteLine($"ID: {material.Id}, Nombre: {material.Nombre}, Cantidad: {material.CantidadTotal}");
            }
        }

        static void AgregarMaterial(MaterialesContext context)
        {
            Console.Write("Ingrese el nombre del material: ");
            var nombre = Console.ReadLine();
            Console.Write("Ingrese la cantidad inicial: ");
            int cantidad = int.Parse(Console.ReadLine());

            var material = new Material { Nombre = nombre, CantidadTotal = cantidad };
            context.Materials.Add(material);
            context.SaveChanges();
            Console.WriteLine("Material agregado con éxito.");
        }

        static void MoverMaterial(MaterialesContext context)
        {
            Console.Write("Ingrese el ID del material: ");
            int idMaterial = int.Parse(Console.ReadLine());
            Console.Write("Ingrese la cantidad a mover: ");
            int cantidad = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el ID de la obra destino: ");
            int idObra = int.Parse(Console.ReadLine());

            var material = context.Materials.Find(idMaterial);
            if (material == null || material.CantidadTotal < cantidad)
            {
                Console.WriteLine("Material no encontrado o cantidad insuficiente.");
                return;
            }

            material.CantidadTotal -= cantidad;
            context.Movimientos.Add(new Movimiento { IdMaterial = idMaterial, IdObra = idObra, Cantidad = cantidad });
            context.SaveChanges();
            Console.WriteLine("Movimiento registrado con éxito.");
        }

        static void ImportarObras(MaterialesContext context)
        {
            Console.Write("Ingrese la ruta del archivo CSV: ");
            string rutaArchivo = Console.ReadLine();
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("Archivo no encontrado.");
                return;
            }

            var lineas = File.ReadAllLines(rutaArchivo);
            foreach (var linea in lineas)
            {
                var datos = linea.Split(',');
                context.Obras.Add(new Obra { Id = int.Parse(datos[0]), Nombre = datos[1] });
            }

            context.SaveChanges();
            Console.WriteLine("Obras importadas con éxito.");
        }
    }
}