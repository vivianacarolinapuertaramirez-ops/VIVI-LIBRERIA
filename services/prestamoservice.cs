using System;
using System.Collections.Generic;
using System.Linq;
using LibreriaVIVI.models;

namespace LibreriaVIVI.services
{
    public class PrestamoService
    {
        private List<Prestamo> _prestamos = new List<Prestamo>();

        public bool GenerarPrestamo(string idUsuario, string idLibro, LibroService libroServ, UsuarioService userServ)
        {
            var usuario = userServ.BuscarPorId(idUsuario);
            var libro = libroServ.BuscarPorId(idLibro);

            if (usuario != null && usuario.Activo && libro != null && libro.Disponible)
            {
                var nuevoPrestamo = new Prestamo
                {
                    IdPrestamo = Guid.NewGuid().ToString().Substring(0, 8),
                    IdUsuario = idUsuario,
                    IdLibro = idLibro,
                    FechaPrestamo = DateTime.Now,
                    FechaLimite = DateTime.Now.AddDays(7),
                    Estado = EstadoPrestamo.Activo
                };

                _prestamos.Add(nuevoPrestamo);
                libro.Disponible = false; 
                return true;
            }
            return false;
        }

        public List<Prestamo> ObtenerTodos() => _prestamos;

        // --- MÉTODOS KPI ---

        // Retorna la cantidad total de registros en la lista
        public int TotalPrestamos() => _prestamos.Count;

        // Retorna solo los préstamos que coincidan con un estado (Activo, Devuelto, Vencido)
        public List<Prestamo> BuscarPorEstado(EstadoPrestamo estado)
        {
            return _prestamos.Where(p => p.Estado == estado).ToList();
        }

        // Calcula el promedio de días que los libros han estado prestados
        public double PromedioDiasPrestamo()
        {
            if (_prestamos.Count == 0) return 0;

            // Calculamos los días transcurridos de cada préstamo y promediamos
            return _prestamos.Average(p => (DateTime.Now - p.FechaPrestamo).TotalDays);
        }
    }
}