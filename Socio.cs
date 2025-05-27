using System;
using System.Collections.Generic;


namespace ClubDeportivo
{
    public class Socio : Persona
    {
        public int NroSocio { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public bool EstadoActivo { get; set; }
        public DateTime? FechaVencimientoCuota { get; set; }
       // public List<Cuota> HistorialCuotas { get; set; }
       // public List<Actividad> ActividadesInscritas { get; set; }

        public Socio(string nombre, string apellido, string dni, DateTime fechaNacimiento)
            : base(nombre, apellido, dni, fechaNacimiento)
        {
           // HistorialCuotas = new List<Cuota>();
           // ActividadesInscritas = new List<Actividad>();
            FechaInscripcion = DateTime.Now;
            EstadoActivo = true;
        }

        public void RenovarMembresia()
        {
            EstadoActivo = true;
        }
    }
}