using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XB_practica1_RFC.Models
{
    internal class RFCModelValidator
    {
        public RFCModel modelo { get; set; }

        public RFCModelValidator(RFCModel modelo) {
            this.modelo = modelo;
            Validar();
        }

        private void Validar() {
            if (modelo.FechaN > DateTime.Now) {
                throw new Exception("no fechas futuras");
            }
            if (modelo.Nombre.Trim().Length == 0) {
                throw new Exception("el nombre debe tener algo");
            }
            if (modelo.AP.Trim().Length == 0 && modelo.AM.Trim().Length == 0)
            {
                throw new Exception("debe tener un apellido por lo menos");
            }
            if (cadenaValida(modelo.Nombre))
            {
                throw new Exception("Nombre Invalido");
            }
            if (modelo.AP.Trim().Length > 0)
                if (cadenaValida(modelo.AP)) {
                    throw new Exception("AP Invalido");
                }
            if (modelo.AM.Trim().Length > 0)
                if (cadenaValida(modelo.AM))
                {
                    throw new Exception("AM Invalido");
                }
        }

        private bool cadenaValida(string cadena) {
            if (!Regex.IsMatch(cadena, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ.,'àèìòùÀÈÌÒÙäëïöüÄËÏÖÜÂÊÎÔÛâêîôû ]*$")) {
                return true;
            }
            return false;
        }
    }
}
