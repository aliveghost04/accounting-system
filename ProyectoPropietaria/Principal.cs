using ProyectoPropietaria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoPropietaria
{
    public partial class Contabilidad : Form
    {
        private User user;

        public Contabilidad(User user)
        {
            InitializeComponent();
            this.user = user;
            lblName.Text = user.name;
        }

        private void parametrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Parametro().Show();
        }

        private void cuentasContablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new CuentaContable().Show();
        }

        private void tiposDeCuentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new TipoCuenta().Show();
        }

        private void tiposDeMonedasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void entradaContableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new EntradaContable().Show();
        }

        private void cRUDToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void salirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IniciarSesion.getInstance().Show();
            this.Dispose();
        }

        private void Contabilidad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult closeIt = MessageBox.Show(
                    "¿Está seguro que desea salir?",
                    "Salir",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (closeIt == DialogResult.Yes)
                {
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnAccountTypes_Click(object sender, EventArgs e)
        {
            MnjTipoCuenta mnjTipoCuenta = MnjTipoCuenta.getInstance();
            mnjTipoCuenta.Show();
            mnjTipoCuenta.Focus();
        }

        private void btnCurrenciesTypes_Click(object sender, EventArgs e)
        {
            MnjTipoMoneda mnjTipoMoneda = MnjTipoMoneda.getInstance();
            mnjTipoMoneda.Show();
            mnjTipoMoneda.Focus();
        }

        private void btnPlacements_Click(object sender, EventArgs e)
        {

        }

        private void btnCountablesAccounts_Click(object sender, EventArgs e)
        {
            MnjCuentaContable instance = MnjCuentaContable.getInstance();
            instance.Show();
            instance.Focus();
        }
    }
}
