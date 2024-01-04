using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XB_practica1_RFC.Models
{
    internal class RFCModelNormalizator
    {
        private readonly string[] pronombres = {
            "DE","LA","LAS","MC","VON","DEL","LOS","Y","MAC","VAN","MI"
        };

        public RFCModel modelo { get; set; }

        public RFCModelNormalizator(RFCModel modelo)
        {
            this.modelo = modelo;
            Normalizar();
        }

        private void Normalizar() {
            modelo.NombreN = quitarAcentosSimbolos(modelo.Nombre);
            modelo.APN = quitarAcentosSimbolos(modelo.AP);
            modelo.AMN = quitarAcentosSimbolos(modelo.AM);
            
            modelo.NombreN = filtrarNombres(modelo.NombreN);
            modelo.NombreN = removerPronombres(modelo.NombreN);
            modelo.APN = removerPronombres(modelo.APN);
            modelo.AMN = removerPronombres(modelo.AMN);

            modelo.FechaN.ToShortDateString();
            modelo.FN = modelo.FechaN.ToString("yyMMdd");
        }

        // MARIA DE ARCO - JUAREZ Y MATA - FIGUEROA
        // DE ARCO - JUAREZ Y MATA - FIGUEROA
        // ARCO JUAREZ MATA FIGUEROA

        private string removerPronombres(string p) {
            string x = p;
            foreach (string pnm in pronombres) {
                x = x.Replace(pnm + " ", "");
            }
            return x;
        }

        private string filtrarNombres(string nombres) {
            string auxNombre = nombres.Normalize().Trim();
            if (auxNombre.Contains(" "))
            {
                if (auxNombre.StartsWith("MARIA ") || auxNombre.StartsWith("JOSE "))
                {
                    string[] palabras = auxNombre.Split(' ');
                    auxNombre = "";
                    for (int x = 1; x < palabras.Length; x++) {
                        auxNombre += palabras[x];
                        if (x < (palabras.Length - 1))
                            auxNombre += " ";
                    }
                    return auxNombre;
                }
            }
            return nombres;
        }






        private string quitarAcentosSimbolos(string palabra) {
            string p = palabra.Normalize(NormalizationForm.FormD);
            var co = p.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != 
                     UnicodeCategory.NonSpacingMark);
            return new string(co.ToArray()).ToUpper().Replace(",","").Replace(".","").Replace("'","");
        }
    }
}
