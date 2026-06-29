using aether.Controle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace aether
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }

        private void btnLogistica_Click(object sender, EventArgs e)
        {
            Msg.Show("A funcionalidade de Logística ainda não está implementada.");
        }

        private void btnLaboratorio_Click(object sender, EventArgs e)
        {
            Lab labapp = new Lab();
            labapp.Show();
            this.Hide();
        }
    }
}
