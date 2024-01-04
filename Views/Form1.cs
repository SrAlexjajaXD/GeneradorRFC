using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XB_practica1_RFC.Models;

namespace XB_practica1_RFC
{
    public partial class form : Form
    {
        public form()
        {
            InitializeComponent();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*char letra = e.KeyChar;
            if (char.IsPunctuation(letra)) {
                MessageBox.Show("Puntuacion " + letra);
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                RFCModel modelo = new RFCModel(
                    txtNombre.Text,
                    txtAP.Text,
                    txtAM.Text,
                    dateTimePicker1.Value
                );
                MessageBox.Show(modelo.RFC);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
