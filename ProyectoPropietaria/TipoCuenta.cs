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
    public partial class TipoCuenta : Form
    {
        private account_types accountType = null;
        private static TipoCuenta instance = null;

        private TipoCuenta()
        {
            InitializeComponent();
            cbType.DataSource = new[] { "DB", "CR" };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (accountType == null)
            {
                accountType = new account_types
                {
                    description = txtDescription.Text,
                    type = cbType.Text,
                    state = rbActive.Checked
                };
                MnjTipoCuenta.getInstance().saveAccountType(accountType);
            }
            else {
                accountType.description = txtDescription.Text;
                accountType.type = cbType.Text;
                accountType.state = rbActive.Checked;
                MnjTipoCuenta.getInstance().saveAccountType(null);
            }

            MessageBox.Show(
                "Datos almacenados con éxito",
                "Información",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult closeIt = MessageBox.Show(
                "¿Estás seguro que quieres cancelar?\n\nSe perderán los cambios",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (closeIt == DialogResult.Yes) {
                this.Close();
            }
        }

        public static TipoCuenta getInstance() {
            if (instance == null) {
                instance = new TipoCuenta();
            }

            return instance;
        }

        public void setTipoCuenta(account_types accountType)
        {
            this.accountType = accountType;

            // Fill fields
            txtDescription.Text = accountType.description;
            cbType.Text = accountType.type;

            if (accountType.state)
            {
                rbActive.Checked = true;
                rbInactive.Checked = false;
            }
            else
            {
                rbInactive.Checked = true;
                rbActive.Checked = false;
            }

            // End fill fields
        }

        private void TipoCuenta_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }
    }
}
