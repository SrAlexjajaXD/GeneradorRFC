using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XB_practica1_RFC.Controllers;

namespace XB_practica1_RFC.Models
{
    internal class RFCModel
    {
        // atributos de instancia
        public string Nombre { get; }
        public string AP { get; }
        public string AM { get; }
        public DateTime FechaN { get; }

        // atributos de control/trabajo
        public string RFC { get; set; }
        public string NombreN { get; set; }
        public string APN { get; set; }
        public string AMN { get; set; }
        public string FN { get; set; }

        // constructor
        public RFCModel(string Nombre, string AP, string AM, DateTime F) {
            this.Nombre = Nombre;
            this.AP = AP;
            this.AM = AM;
            this.FechaN = F;

            // validar datos de entrada
            RFCModelValidator v = new RFCModelValidator(this);
            // normalizar los datos
            RFCModelNormalizator n = new RFCModelNormalizator(this);
            // ejecutar acciones
            RFCController c = new RFCController(this);
        }

        public string getNombreCompleto() {
            return NombreN + " " + APN + " " + AMN;
        }
    }
}
