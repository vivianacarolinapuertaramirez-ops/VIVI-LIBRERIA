using System.Collections.Generic;
using System.Linq;
using LibreriaVIVI.models;

namespace LibreriaVIVI.services
{
    public class UsuarioService
    {
        private List<Usuario> _usuarios = new List<Usuario>();

        public void Registrar(Usuario usuario) => _usuarios.Add(usuario);

        public List<Usuario> ObtenerTodos() => _usuarios;

        public Usuario? BuscarPorId(string id) => _usuarios.FirstOrDefault(u => u.Id == id);

        // --- MÉTODOS KPI ---

        // Total de usuarios registrados
        public int TotalUsuarios() => _usuarios.Count;

        // KPI: Porcentaje o conteo de usuarios activos (los que pueden prestar)
        public List<Usuario> ObtenerUsuariosActivos()
        {
            return _usuarios.Where(u => u.Activo).ToList();
        }

        // Buscar si un nombre ya existe (para evitar duplicados en reportes)
        public bool ExisteUsuario(string nombre)
        {
            return _usuarios.Any(u => u.Nombre.ToLower() == nombre.ToLower());
        }
    }
}