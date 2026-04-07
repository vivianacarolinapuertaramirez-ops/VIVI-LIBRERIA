using System.Collections.Generic;
using System.Linq;
using LibreriaVIVI.models;

namespace LibreriaVIVI.services
{
    public class LibroService
    {
        private List<Libro> _libros = new List<Libro>();

        public void Registrar(Libro libro) => _libros.Add(libro);

        public List<Libro> ObtenerTodos() => _libros;

        public Libro? BuscarPorId(string id) => _libros.FirstOrDefault(l => l.Id == id);

        public bool Eliminar(string id)
        {
            var libro = BuscarPorId(id);
            if (libro != null && libro.Disponible)
            {
                _libros.Remove(libro);
                return true;
            }
            return false;
        }

        // --- MÉTODOS KPI ---

        // Retorna cuántos libros hay en total en el sistema
        public int TotalLibros() => _libros.Count;

        // Retorna la lista de libros filtrada por disponibilidad
        public List<Libro> ObtenerPorDisponibilidad(bool disponible)
        {
            return _libros.Where(l => l.Disponible == disponible).ToList();
        }

        // KPI: Conteo por categoría (útil para estadísticas de inventario)
        public int ContarPorCategoria(string categoria)
        {
            return _libros.Count(l => l.Categoria.ToLower() == categoria.ToLower());
        }
    }
}