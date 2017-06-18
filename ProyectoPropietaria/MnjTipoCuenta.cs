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
    public partial class MnjTipoCuenta : Form
    {
        ContabilidadEntities entities = new ContabilidadEntities();
        private static MnjTipoCuenta instance = null;

        private MnjTipoCuenta()
        {
            InitializeComponent();
            loadAccountTypes();
        }

        private void loadAccountTypes() {
            dgvAccountTypes.DataSource = (from account_types in entities.account_types
                                         select new {
                                             account_types.id,
                                             account_types.description,
                                             account_types.type,
                                             account_types.state
                                         }).ToList();

            dgvAccountTypes.Columns[0].HeaderText = "ID";
            dgvAccountTypes.Columns[1].HeaderText = "Descripción";
            dgvAccountTypes.Columns[2].HeaderText = "Origen";
            dgvAccountTypes.Columns[3].HeaderText = "Estado";
            dgvAccountTypes.AutoResizeColumns();
        }

        private void MnjTipoCuenta_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TipoCuenta.getInstance().Show();
        }

        public void saveAccountType(account_types accountType) {
            if (accountType != null)
            {
                entities.account_types.Add(accountType);
            }
            
            entities.SaveChanges();
            loadAccountTypes();
        }

        public static MnjTipoCuenta getInstance() {
            if (instance == null) {
                instance = new MnjTipoCuenta();
            }

            return instance;
        }

        private void MnjTipoCuenta_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;
            
            if (dgvAccountTypes.SelectedRows.Count == 1)
            {
                row = dgvAccountTypes.SelectedRows[0];
            }
            else if (dgvAccountTypes.SelectedCells.Count == 1) {
                int i = dgvAccountTypes.SelectedCells[0].RowIndex;
                row = dgvAccountTypes.Rows[i];
            } else {
                MessageBox.Show(
                    "Debe seleccionar solo 1 tipo de cuenta a modificar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null) {

                TipoCuenta tipoCuenta = TipoCuenta.getInstance();
                int id = Int32.Parse(row.Cells[0].Value.ToString());
                tipoCuenta.setTipoCuenta(
                    (
                        from em in entities.account_types
                        where em.id == id
                        select em
                    ).First()
                );
                tipoCuenta.Show();
                tipoCuenta.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvAccountTypes.SelectedRows.Count == 1)
            {
                row = dgvAccountTypes.SelectedRows[0];
            }
            else if (dgvAccountTypes.SelectedCells.Count == 1)
            {
                int i = dgvAccountTypes.SelectedCells[0].RowIndex;
                row = dgvAccountTypes.Rows[i];
            }
            else
            {
                MessageBox.Show(
                    "Debe seleccionar solo 1 tipo de cuenta a eliminar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null) {
                int id = Int32.Parse(row.Cells[0].Value.ToString());

                account_types accountType = (from em in entities.account_types
                                             where em.id == id
                                             select em).First();

                if (accountType == null)
                {
                    MessageBox.Show(
                        "Tipo de Cuenta no encontrada",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else {
                    DialogResult deleteIt = MessageBox.Show(
                        "¿Estás seguro que quieres eliminar este tipo de cuenta?",
                        "Confirmar",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (deleteIt == DialogResult.Yes)
                    {
                        entities.account_types.Remove(accountType);
                        entities.SaveChanges();

                        MessageBox.Show(
                            "Tipo de Cuenta eliminada con éxito",
                            "Información",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        loadAccountTypes();
                    }
                }
            }

        }
    }
}
