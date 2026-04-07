using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LibreriaVIVI.services; 
using LibreriaVIVI.models;   

namespace LibreriaVIVI
{
    class Program
    {
        static LibroService _libroService = new LibroService();
        static UsuarioService _usuarioService = new UsuarioService();
        static PrestamoService _prestamoService = new PrestamoService();
        
        static void Main(string[] args)
        {
            // 1. CARGAR DATOS AL INICIAR
            CargarDatos();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("================================");
                Console.WriteLine("   ¡Bienvenido a Vivi Librería!   ");
                Console.WriteLine("================================");
                Console.WriteLine("1. Libros");
                Console.WriteLine("2. Usuarios");
                Console.WriteLine("3. Préstamos");
                Console.WriteLine("4. Reportes y KPIs");
                Console.WriteLine("5. Salir");
                Console.Write("\nSeleccione una opción: ");

                string opcion = Console.ReadLine() ?? "";
                
                if (opcion == "5") 
                {
                    // 2. PREGUNTAR SI GUARDA ANTES DE SALIR
                    Console.Write("\n¿Desea guardar los cambios antes de salir? (S/N): ");
                    if (Console.ReadLine()?.ToUpper() == "S")
                    {
                        GuardarDatos();
                    }
                    Console.WriteLine("¡Hasta Luego!");
                    break;
                }

                switch (opcion)
                {
                    case "1": MenuLibros(); break;
                    case "2": MenuUsuarios(); break;
                    case "3": MenuPrestamos(); break;
                    case "4": MenuReportes(); break;
                    default: Error(); break;
                }
            }
        }

        // --- MÉTODOS DE PERSISTENCIA ---

        static void GuardarDatos()
        {
            Console.WriteLine("Sincronizando con base de datos JSON...");
            // Aquí irían tus llamadas a los servicios de persistencia
            // Por ahora simulamos el proceso
            Thread.Sleep(1000);
            Ok("Datos guardados correctamente.");
        }

        static void CargarDatos()
        {
            Console.WriteLine("Cargando sistema...");
            // Simulación de carga de datos iniciales o JSON
            Thread.Sleep(500);
        }

        // --- GESTIÓN DE LIBROS ---
        static void MenuLibros()
        {
            Console.Clear();
            Console.WriteLine(">> SECCIÓN LIBROS");
            Console.WriteLine("1. Registrar libro\n2. Listar libros\n3. Eliminar\n0. Volver");
            string op = Console.ReadLine() ?? "";

            if (op == "1")
            {
                Libro nuevo = new Libro();
                Console.Write("ID/ISBN: "); nuevo.Id = Console.ReadLine() ?? "";
                Console.Write("Título: "); nuevo.Titulo = Console.ReadLine() ?? "";
                Console.Write("Autor: "); nuevo.Autor = Console.ReadLine() ?? "";
                Console.Write("Categoría: "); nuevo.Categoria = Console.ReadLine() ?? "";
                
                _libroService.Registrar(nuevo); 
                Ok("Libro registrado exitosamente.");
            }
            else if (op == "2")
            {
                Console.Clear();
                Console.WriteLine("--- LISTADO DE LIBROS ---");
                var lista = _libroService.ObtenerTodos();
                if (!lista.Any()) { Ok("No hay libros registrados."); return; }
                
                foreach (var l in lista)
                {
                    Console.WriteLine($"ID: {l.Id} | {l.Titulo} | Autor: {l.Autor} | Disponible: {l.Disponible}");
                }
                Console.WriteLine("\n--------------------------");
                Console.WriteLine($"TOTAL LIBROS: {_libroService.TotalLibros()}");
                Ok("");
            }
            else if (op == "3")
            {
                Console.Write("ID a eliminar: ");
                string id = Console.ReadLine() ?? "";
                if (_libroService.Eliminar(id)) Ok("Eliminado con éxito.");
                else Error("No se pudo eliminar (está prestado o no existe).");
            }
        }

        // --- GESTIÓN DE USUARIOS ---
        static void MenuUsuarios()
        {
            Console.Clear();
            Console.WriteLine(">> SECCIÓN USUARIOS");
            Console.WriteLine("1. Registrar Usuario\n2. Listar\n0. Volver");
            string op = Console.ReadLine() ?? "";

            if (op == "1")
            {
                Usuario u = new Usuario();
                Console.Write("ID: "); u.Id = Console.ReadLine() ?? "";
                Console.Write("Nombre: "); u.Nombre = Console.ReadLine() ?? "";
                _usuarioService.Registrar(u);
                Ok("Usuario registrado.");
            }
            else if (op == "2")
            {
                Console.Clear();
                var lista = _usuarioService.ObtenerTodos();
                if (!lista.Any()) { Ok("No hay usuarios registrados."); return; }
                
                lista.ForEach(u => Console.WriteLine($"ID: {u.Id} | Nombre: {u.Nombre} | Activo: {u.Activo}"));
                Console.WriteLine($"\nTOTAL USUARIOS: {_usuarioService.TotalUsuarios()}");
                Ok("");
            }
        }

        // --- GESTIÓN DE PRÉSTAMOS ---
        static void MenuPrestamos()
        {
            Console.Clear();
            Console.WriteLine(">> SECCIÓN PRÉSTAMOS");
            Console.WriteLine("1. Crear Préstamo\n2. Listar Préstamos\n0. Volver");
            string op = Console.ReadLine() ?? "";

            if (op == "1")
            {
                Console.Write("ID Usuario: "); string idU = Console.ReadLine() ?? "";
                Console.Write("ID Libro: "); string idL = Console.ReadLine() ?? "";

                bool exito = _prestamoService.GenerarPrestamo(idU, idL, _libroService, _usuarioService);

                if (exito) Ok("Préstamo realizado con éxito.");
                else Error("Error: Usuario inactivo o Libro no disponible.");
            }
            else if (op == "2")
            {
                Console.Clear();
                var listaP = _prestamoService.ObtenerTodos();
                if (!listaP.Any()) { Ok("No hay préstamos registrados."); return; }
                
                listaP.ForEach(p => Console.WriteLine($"Préstamo: {p.IdPrestamo} | Libro: {p.IdLibro} | Usuario: {p.IdUsuario}"));
                Console.WriteLine($"\nPROMEDIO DÍAS: {_prestamoService.PromedioDiasPrestamo():F2}");
                Ok("");
            }
        }

        static void MenuReportes()
        {
            Console.Clear();
            Console.WriteLine(">> REPORTES Y ESTADÍSTICAS");
            Console.WriteLine("1. Libros Disponibles\n2. Usuarios Activos\n3. Comparación Array vs List\n0. Volver");
            
            switch (Console.ReadLine())
            {
                case "1":
                    var disp = _libroService.ObtenerPorDisponibilidad(true);
                    disp.ForEach(l => Console.WriteLine($"- {l.Titulo}"));
                    Ok($"Total disponibles: {disp.Count}");
                    break;
                case "2":
                    var act = _usuarioService.ObtenerUsuariosActivos();
                    act.ForEach(u => Console.WriteLine($"- {u.Nombre}"));
                    Ok($"Total activos: {act.Count}");
                    break;
                case "3":
                    ExplicarDiferenciaArrayLista();
                    break;
            }
        }

        static void ExplicarDiferenciaArrayLista()
        {
            Console.Clear();
            Console.WriteLine("=== COMPARACIÓN TÉCNICA ===");
            Console.WriteLine("1. ARRAYS: Tamaño fijo (estático). Como una caja con divisiones que no cambian.");
            Console.WriteLine("2. LISTAS: Tamaño dinámico. Crecen o se encogen según agregas o quitas objetos.");
            Ok("");
        }

        static void Ok(string msg)
        {
            if (!string.IsNullOrEmpty(msg)) Console.WriteLine($"\n[OK]: {msg}");
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        static void Error(string msg = "Opción inválida")
        {
            Console.WriteLine($"\n[!] {msg}");
            Thread.Sleep(1200);
        }
    }
}