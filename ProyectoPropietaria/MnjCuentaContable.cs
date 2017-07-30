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
    public partial class MnjCuentaContable : Form
    {
        ContabilidadEntities entities;
        private static MnjCuentaContable instance = null;

        private MnjCuentaContable()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
            loadCountablesAccounts("");
            managePermission();
        }

        private void managePermission() {
            User user = Contabilidad.user;

            if (user.permission == 3) {
                btnDelete.Visible = false;
                btnModify.Visible = false;
                btnSave.Visible = false;
            }
        }

        private void loadCountablesAccounts(String filter)
        {
            int id = 0;

            Int32.TryParse(filter, out id);
            dgvCountablesAccounts.DataSource =
                (from em in entities.countables_accounts
                join em2 in entities.account_types
                on em.account_type equals em2.id
                join em3 in entities.countables_accounts 
                on em.account_major equals em3.id into gj
                from em4 in gj.DefaultIfEmpty()
                where em.id == id || em.description.Contains(filter)
                select new {
                    id = em.id,
                    description = em.description,
                    accountType = em2.description,
                    allowTransaction = em.allow_transaction,
                    level = em.level,
                    accountMayor = em4 == null ? String.Empty : em4.description,
                    balance = em.balance,
                    state = em.state
                }).ToList();

            dgvCountablesAccounts.Columns[0].HeaderText = "ID";
            dgvCountablesAccounts.Columns[1].HeaderText = "Descripción";
            dgvCountablesAccounts.Columns[2].HeaderText = "Tipo Cuenta";
            dgvCountablesAccounts.Columns[3].HeaderText = "Permite movimiento";
            dgvCountablesAccounts.Columns[4].HeaderText = "Nivel";
            dgvCountablesAccounts.Columns[5].HeaderText = "Cuenta mayor";
            dgvCountablesAccounts.Columns[6].HeaderText = "Balance";
            dgvCountablesAccounts.Columns[7].HeaderText = "Estado";
            dgvCountablesAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public bool saveAccountCountable(countables_accounts accountCountable, bool isNew) {

            // Validation
            string errors = validate(accountCountable);

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

            if (isNew)
            {
                entities.countables_accounts.Add(accountCountable);
            }

            try
            {
                entities.SaveChanges();

                MessageBox.Show(
                    "¡Cambios guardados con éxito!",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                loadCountablesAccounts("");
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(
                    "¡No se pudo guardar los cambios!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                loadCountablesAccounts("");
                return false;
            }
        }

        public static MnjCuentaContable getInstance() {
            if (instance == null)
            {
                instance = new MnjCuentaContable();
            }

            return instance;
        }

        private void MnjCuentaContable_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (entities.account_types.Count(em => em.state == true) > 0)
            {
                CuentaContable cuentaContable = CuentaContable.getInstance();
                cuentaContable.ShowDialog(this);
                cuentaContable.Focus();
            } else
            {
                MessageBox.Show(
                    "No se puede agregar cuentas. \nNo hay tipos de cuentas disponibles",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvCountablesAccounts.SelectedRows.Count == 1) {
                row = dgvCountablesAccounts.SelectedRows[0];
            } else if (dgvCountablesAccounts.SelectedCells.Count == 1) {
                int i = dgvCountablesAccounts.SelectedCells[0].RowIndex;
                row = dgvCountablesAccounts.Rows[i];
            } else {
                MessageBox.Show(
                    "Debe seleccionar solo 1 tipo de cuenta a modificar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {
                int id = Int32.Parse(row.Cells[0].Value.ToString());
                CuentaContable cuentaContable = CuentaContable.getInstance();
                countables_accounts countableAccount =
                    (from em in entities.countables_accounts
                     where em.id == id
                     select em).First();

                cuentaContable.setCuentaContable(countableAccount);
                cuentaContable.ShowDialog(this);
                cuentaContable.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvCountablesAccounts.SelectedRows.Count == 1)
            {
                row = dgvCountablesAccounts.SelectedRows[0];
            }
            else if (dgvCountablesAccounts.SelectedCells.Count == 1)
            {
                int i = dgvCountablesAccounts.SelectedCells[0].RowIndex;
                row = dgvCountablesAccounts.Rows[i];
            }
            else
            {
                MessageBox.Show(
                    "Debe seleccionar solo 1 tipo de cuenta a modificar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {
                DialogResult deleteIt = MessageBox.Show(
                    "¿Estás seguro de eliminar esta cuenta?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (deleteIt == DialogResult.Yes)
                {
                    int id = Int32.Parse(row.Cells[0].Value.ToString());
                    countables_accounts countableAccount =
                        (from em in entities.countables_accounts
                         where em.id == id
                         select em).First();

                    if (countableAccount == null) {
                        MessageBox.Show(
                            "¡Cuenta no encontrada!",
                            "Información",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    } else
                    {
                        try
                        {
                            entities.countables_accounts.Remove(countableAccount);
                            entities.SaveChanges();
                                
                            MessageBox.Show(
                                "¡Cuenta Eliminada con éxito!",
                                "Información",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            loadCountablesAccounts("");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                "La cuenta no pudo ser eliminada porque se encuentra en uso",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            ConnectionDB.getInstance().resetConnection();
                        }
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadCountablesAccounts(txtSearch.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btnSearch.PerformClick();
            }
        }
    
        public String validate(countables_accounts countableAccount)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var prop in countableAccount.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(countableAccount, ((string)prop.GetValue(countableAccount)).Trim());
                }
            }

            if (countableAccount.description.Length == 0)
            {
                sb.Append("- El campo descripción es obligatorio\n");
            }

            if (countableAccount.level > 1 && countableAccount.account_major == null)
            {
                sb.Append("El campo cuenta mayor es obligatorio\n");
            }
            return sb.ToString();
        }
    }
}
