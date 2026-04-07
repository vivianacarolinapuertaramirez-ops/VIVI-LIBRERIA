using System;

namespace LibreriaVIVI.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Contacto { get; set; }
        public bool Activo { get; set; }

        public Usuario()
        {
            Activo = true;
        }

        public string ResumenCorto()
        {
            return $"{Nombre} - {Contacto}";
        }

        public string DetalleCompleto()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Contacto: {Contacto}, Activo: {Activo}";
        }

        public override string ToString()
        {
            return DetalleCompleto();
        }
    }
}