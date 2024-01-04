using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using XB_practica1_RFC.Models;

namespace XB_practica1_RFC.Controllers
{
    internal class RFCController
    {
        private readonly string[] Prohibidas = {
            "BUEI", "BUEY", "CACA", "CACO", "CAGA", "KOGE", "KAKA", "MAME", "KOJO", "KULO",
            "CAGO", "COGE", "COJE", "COJO", "FETO", "JOTO", "KACO", "KAGO", "MAMO", "MEAR", "MEON",
            "MION", "MOCO", "MULA", "PEDA", "PEDO", "PENE", "PUTA", "PUTO", "QULO", "RATA", "RUIN"
        };

        public static readonly Dictionary<char, string> ValoresHomoClave = new Dictionary<char, string>() {
            {' ', "00" }, {'0', "00" }, {'1', "01" }, {'2', "02" }, {'3', "03" }, {'4', "04" }, {'5', "05" },
            {'6', "06" }, {'7', "07" }, {'8', "08" }, {'9', "09" }, {'&', "10" }, {'A', "11" }, {'B', "12" },
            {'C', "13" }, {'D', "14" }, {'E', "15" }, {'F', "16" }, {'G', "17" }, {'H', "18" }, {'I', "19" },
            {'J', "21" }, {'K', "22" }, {'L', "23" }, {'M', "24" }, {'N', "25" }, {'O', "26" }, {'P', "27" },
            {'Q', "28" }, {'R', "29" }, {'S', "32" }, {'T', "33" }, {'U', "34" }, {'V', "35" }, {'W', "36" },
            {'X', "37" }, {'Y', "38" }, {'Z', "39" }, {'Ñ', "40" }
        };

        public static readonly Dictionary<int, char> CocientesResiduos = new Dictionary<int, char>()
        {
            {0,'1' },  {13,'E' }, {26,'S'}, {1,'2' },  {14,'F' }, {27,'T'}, {2,'3' },  {15,'G' }, {28,'U'},
            {3,'4' },  {16,'H' }, {29,'V'}, {4,'5' },  {17,'I' }, {30,'W'}, {5,'6' },  {18,'J' }, {31,'X'},
            {6,'7' },  {19,'K' }, {32,'Y'}, {7,'8' },  {20,'L' }, {33,'Z'}, {8,'9' },  {21,'M' }, {9,'A' },  
            {22,'N' }, {10,'B' }, {23,'P' },{11,'C' }, {24,'Q' }, {12,'D' },{25,'R' }
        };

        public static readonly Dictionary<char, string> ValoresCodigoV = new Dictionary<char, string>() {
            {'0', "00" }, {'1', "01" }, {'2', "02" }, {'3', "03" }, {'4', "04" }, {'5', "05" }, {'6', "06" }, {'7', "07" },
            {'8', "08" }, {'9', "09" }, {'A', "10" }, {'B', "11" }, {'C', "12" }, {'D', "13" }, {'E', "14" }, {'F', "15" },
            {'G', "16" }, {'H', "17" }, {'I', "18" }, {'J', "19" }, {'K', "20" }, {'L', "21" }, {'M', "22" }, {'N', "23" },
            {'&', "24" }, {'O', "25" }, {'P', "26" }, {'Q', "27" }, {'R', "28" }, {'S', "29" }, {'T', "30" }, {'U', "31" }, 
            {'V', "32" }, {'W', "33" }, {'X', "34" }, {'Y', "35" }, {'Z', "36" }, {' ', "37" }, {'Ñ', "38" }
        };


        public RFCModel modelo { get; set; }

        public RFCController(RFCModel modelo)
        {
            this.modelo = modelo;
            modelo.RFC = "";
            Acciones();
        }

        private void Acciones() {
            if (modelo.APN != String.Empty && modelo.AMN != String.Empty)
            {
                // tiene los 2 apellidos
                if (modelo.APN.Length > 2)
                {
                    // si el aP > 2
                    /* REGLA 1ª.  */
                    modelo.RFC += modelo.APN[0].ToString();
                    modelo.RFC += primeraVocal(modelo.APN);
                    modelo.RFC += modelo.AMN[0].ToString();
                    modelo.RFC += modelo.NombreN[0].ToString();

                }
                else
                {
                    // si AP <= 2
                    /* REGLA 4ª. */
                    modelo.RFC += modelo.APN[0].ToString();
                    modelo.RFC += modelo.AMN[0].ToString();
                    modelo.RFC += modelo.NombreN[0].ToString();
                    modelo.RFC += modelo.NombreN[1].ToString();
                }
            }
            else if (modelo.APN != String.Empty)
            {
                /*
                 * REGLA 7ª. En los casos en que la persona física tenga un solo apellido, 
                 * se conformará con la primera y segunda letras del apellido paterno o materno,
                 * más la primera y segunda letras del nombre.
                 */
                modelo.RFC += modelo.APN[0].ToString();
                modelo.RFC += modelo.APN[1].ToString();
                modelo.RFC += modelo.NombreN[0].ToString();
                modelo.RFC += modelo.NombreN[1].ToString();
            }
            else if (modelo.AMN != String.Empty)
            {
                modelo.RFC += modelo.AMN[0].ToString();
                modelo.RFC += modelo.AMN[1].ToString();
                modelo.RFC += modelo.NombreN[0].ToString();
                modelo.RFC += modelo.NombreN[1].ToString();
            }
            else {
                throw new Exception("Es necesario un apellido");
            }


            // aqui regla 9
            quitarProhibidas();


            /* REGLA 2. */
            modelo.RFC += modelo.FN;

            // tarea :D
            // PROCEDIMIENTO PARA OBTENER LA CLAVE DIFERENCIADORA DE HOMONIMIA
            modelo.RFC+=generarHomoClave();

            modelo.RFC+=generarNumClave();

        }

        private String generarHomoClave() { 
            string clave="",valores="",nombre=modelo.getNombreCompleto();
            int suma=0,cos,res;
            for (int i=0; i<nombre.Length;i++) {
                for (int j=0; j<ValoresHomoClave.Count;j++) {
                    if (nombre[i] == ValoresHomoClave.ElementAt(j).Key) { 
                        valores+=ValoresHomoClave.ElementAt(j).Value;
                    }
                }
            }
            for (int i=0;i<valores.Length-1;i++) {
                suma += (Int32.Parse((valores[i].ToString() + valores[i+1].ToString()))) * Int32.Parse(valores[i+1].ToString());
            }
            suma=suma%1000;
            cos=suma/34;
            res=suma%34;

            return clave = CocientesResiduos[cos].ToString() + CocientesResiduos[res+1].ToString();
        }



        private String generarNumClave() { 
            string rfc=modelo.RFC;
            string valores="";
            string codigo="";
            int p=13;
            int s=0;
            for (int i=0; i<rfc.Length;i++) {
                for (int j=0; j<ValoresCodigoV.Count;j++) {
                    if (rfc[i] == ValoresCodigoV.ElementAt(j).Key) { 
                        valores+=ValoresCodigoV.ElementAt(j).Value;
                    }
                }
            }
            for (int i=0; i<valores.Length-1;i+=2) {
                s += Int32.Parse(valores[i].ToString()+ valores[i+1].ToString()) * p;
                p--;
            }
            s %= 11;
            if (s == 0) {
                codigo = "0";
            }
            if (s > 0) {
                codigo = Convert.ToString(11-s);
            }
            if (s == 10) {
                codigo = "A";
            }
            return codigo;
        }

        private void quitarProhibidas() {
            for (int x = 0; x < Prohibidas.Length; x++) {
                if (Prohibidas[x] == modelo.RFC) {
                    char[] c = modelo.RFC.ToCharArray();
                    c[3] = 'X';
                    modelo.RFC = c.ToString();
                }
            }
        }

        private string primeraVocal(string p) {
            for (int x = 1; x < p.Length; x++) {
                if (p[x] == 'A' || p[x] == 'E' || p[x] == 'I' || p[x] == 'O' || p[x] == 'U') { 
                    return p[x].ToString();
                }
            }
            return "X";
        }
    }
}
