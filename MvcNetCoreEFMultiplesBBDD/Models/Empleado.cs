using System;

namespace MvcNetCoreEFMultiplesBBDD.Models
{
    public class Empleado
    {
        public string Apellido { get; set; } = string.Empty;

        public string Oficio { get; set; } = string.Empty;

        public int Dir { get; set; }

        public int Salario { get; set; }

        public int Comision { get; set; }

        public string NombreDept { get; set; } = string.Empty;
    }
}
