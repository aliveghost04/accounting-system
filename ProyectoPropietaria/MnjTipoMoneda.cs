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
    public partial class MnjTipoMoneda : Form
    {
        ContabilidadEntities entities;
        private static MnjTipoMoneda instance = null;

        private MnjTipoMoneda()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
            loadCurrenciesTypes("");
            managePermission();
        }

        private void managePermission()
        {
            User user = Contabilidad.user;

            if (user.permission == 3)
            {
                btnDelete.Visible = false;
                btnModify.Visible = false;
                btnAdd.Visible = false;
            }
        }

        private void loadCurrenciesTypes(String filter) {
            int id = 0;

            Int32.TryParse(filter, out id);

            dgvCurrenciesTypes.DataSource = 
                (from em in entities.currencies_types
                 where em.id == id ||
                 em.description.Contains(filter)
                 select new {
                    em.id,
                    em.description,
                    em.exchange_rate,
                    em.state
                }).ToList();

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

        public bool saveCurrencyType(currencies_types currencyType, bool isNew) {
            // Validating
            string errors = validate(currencyType);

            if (errors.Length > 0)
            {
                MessageBox.Show(
                    errors,
                    "Error de validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            // End validtion

            if (isNew)
            {
                entities.currencies_types.Add(currencyType);
            }
            
            entities.SaveChanges();
            loadCurrenciesTypes("");
            return true;
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
                    loadCurrenciesTypes("");
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
                    loadCurrenciesTypes("");
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
                        try
                        {
                            entities.SaveChanges();

                            MessageBox.Show(
                                "¡Tipo de Moneda eliminada con éxito!",
                                "Información",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        catch (Exception ex) {
                            MessageBox.Show(
                                "¡Este tipo de moneda se encuentra en uso!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            ConnectionDB.getInstance().resetConnection();
                        }
                    }
                    loadCurrenciesTypes("");
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadCurrenciesTypes(txtSearch.Text);
        }

        private string validate(currencies_types currency) {
            StringBuilder sb = new StringBuilder();

            foreach (var prop in currency.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(currency, ((string)prop.GetValue(currency)).Trim());
                }
            }

            if (currency.description.Length == 0) {
                sb.Append("- El campo descripción es obligatorio\n");
            }

            if (currency.exchange_rate <= 0)
            {
                sb.Append("- El campo tasa de cambio es obligatorio y debe ser mayor que 0\n");
            }

            return sb.ToString();
        }
    }
}
