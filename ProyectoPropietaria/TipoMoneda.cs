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
    public partial class TipoMoneda : Form
    {
        private static TipoMoneda instance = null;
        private currencies_types currencyType = null;

        private TipoMoneda()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public static TipoMoneda getInstance() {
            if (instance == null)
            {
                instance = new TipoMoneda();
            }

            return instance;
        }

        public void setCurrencyType(currencies_types currencyType) {
            this.currencyType = currencyType;

            // Fill with attributes
            txtDescription.Text = currencyType.description;
            txtExchangeRate.Text = currencyType.exchange_rate.ToString();

            if (currencyType.state)
            {
                rbActive.Checked = true;
            }
            else {
                rbInactive.Checked = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool saved = false;
            if (currencyType == null)
            {
                Decimal exchangeRate = 0;
                Decimal.TryParse(txtExchangeRate.Text, out exchangeRate);
                currencies_types newCurrencyType = new currencies_types {
                    description = txtDescription.Text,
                    exchange_rate = exchangeRate,
                    state = rbActive.Checked
                };
                saved = MnjTipoMoneda.getInstance().saveCurrencyType(newCurrencyType, true);
            } else {
                Decimal exchangeRate = 0;
                Decimal.TryParse(txtExchangeRate.Text, out exchangeRate);
                currencyType.description = txtDescription.Text;
                currencyType.exchange_rate = exchangeRate;
                currencyType.state = rbActive.Checked;
                saved = MnjTipoMoneda.getInstance().saveCurrencyType(currencyType, false);
            }

            if (saved)
            {
                MessageBox.Show(
                    "Datos almacenados con éxito",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.Close();
            }
        }

        private void TipoMoneda_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult closeIt = MessageBox.Show(
               "¿Estás seguro que quieres cancelar?\n\nSe perderán los cambios",
               "Confirmar",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
           );

            if (closeIt == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
