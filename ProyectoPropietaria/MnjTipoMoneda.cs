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
    public partial class MnjTipoMoneda : Form
    {
        ContabilidadEntities entities = new ContabilidadEntities();
        private static MnjTipoMoneda instance = null;

        private MnjTipoMoneda()
        {
            InitializeComponent();
            loadCurrenciesTypes();
        }

        private void loadCurrenciesTypes() {
            dgvCurrenciesTypes.DataSource = entities.currencies_types.ToList();

            dgvCurrenciesTypes.Columns[0].HeaderText = "ID";
            dgvCurrenciesTypes.Columns[1].HeaderText = "Descripción";
            dgvCurrenciesTypes.Columns[2].HeaderText = "Tasa de cambio";
            dgvCurrenciesTypes.Columns[3].HeaderText = "Estado";
        }

        private void MnjTipoMoneda_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        public static MnjTipoMoneda getInstance() {
            if (instance == null) {
                instance = new MnjTipoMoneda();
            }

            return instance;
        }

        public void saveCurrencyType(currencies_types currencyType) {
            if (currencyType != null) {
                entities.currencies_types.Add(currencyType);
            }

            entities.SaveChanges();
            loadCurrenciesTypes();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TipoMoneda tipoMoneda = TipoMoneda.getInstance();
            tipoMoneda.Show();
            tipoMoneda.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvCurrenciesTypes.SelectedRows.Count == 1)
            {
                row = dgvCurrenciesTypes.SelectedRows[0];
            } else if (dgvCurrenciesTypes.SelectedCells.Count == 1)
            {
                int i = dgvCurrenciesTypes.SelectedCells[0].RowIndex;
                row = dgvCurrenciesTypes.Rows[i];
            } else
            {
                MessageBox.Show(
                    "Debes seleccionar solo 1 tipo de moneda a modificar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {
                int id = Int32.Parse(row.Cells[0].Value.ToString());
                currencies_types currencyType = (from em in entities.currencies_types
                                                 where em.id == id
                                                 select em).First();
                
                if (currencyType == null)
                {
                    MessageBox.Show(
                        "El Tipo de Moneda seleccionado no existe",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    loadCurrenciesTypes();
                } else
                {
                    TipoMoneda tipoMoneda = TipoMoneda.getInstance();
                    tipoMoneda.setCurrencyType(currencyType);
                    tipoMoneda.Show();
                    tipoMoneda.Focus();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvCurrenciesTypes.SelectedRows.Count == 1)
            {
                row = dgvCurrenciesTypes.SelectedRows[0];
            }
            else if (dgvCurrenciesTypes.SelectedCells.Count == 1)
            {
                int i = dgvCurrenciesTypes.SelectedCells[0].RowIndex;
                row = dgvCurrenciesTypes.Rows[i];
            }
            else
            {
                MessageBox.Show(
                    "Debes seleccionar solo 1 tipo de moneda a eliminar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {
                int id = Int32.Parse(row.Cells[0].Value.ToString());
                currencies_types currencyType = (from em in entities.currencies_types
                                                 where em.id == id
                                                 select em).First();

                if (currencyType == null)
                {
                    MessageBox.Show(
                        "El Tipo de Moneda seleccionado no existe",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    loadCurrenciesTypes();
                }
                else
                {
                    DialogResult deleteIt = MessageBox.Show(
                        "¿Estás seguro que quieres eliminar este tipo de moneda?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (deleteIt == DialogResult.Yes)
                    {
                        entities.currencies_types.Remove(currencyType);
                        entities.SaveChanges();

                        MessageBox.Show(
                            "Tipo de Moneda eliminada con éxito",
                            "Información",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
        }
    }
}
