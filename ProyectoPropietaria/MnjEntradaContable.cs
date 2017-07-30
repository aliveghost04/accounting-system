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
    public partial class MnjEntradaContable : Form
    {
        ContabilidadEntities entities;
        private static MnjEntradaContable instance = null;

        private MnjEntradaContable()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
            loadPlacements("");
            managePermission();
        }

        private void managePermission()
        {
            User user = Contabilidad.user;

            if (user.permission == 3)
            {
                btnRevert.Visible = false;
                btnAdd.Visible = false;
            }
        }

        public void loadPlacements(String filter) {
            int id = 0;

            Int32.TryParse(filter, out id);
            dgvPlacements.DataSource = (from em in entities.placements
                                        join em2 in entities.currencies_types
                                        on em.currency_type equals em2.id
                                        where em.id == id || 
                                        em.description.Contains(filter)
                                        select new {
                                            em.id,
                                            em.description,
                                            auxiliary = "Contabilidad",
                                            em.date,
                                            currency = em2.description,
                                            em.exchange_rate,
                                            em.state
                                        }).ToList();

            dgvPlacements.Columns[0].HeaderText = "ID";
            dgvPlacements.Columns[1].HeaderText = "Descripción";
            dgvPlacements.Columns[2].HeaderText = "Auxiliar";
            dgvPlacements.Columns[3].HeaderText = "Fecha";
            dgvPlacements.Columns[4].HeaderText = "Moneda";
            dgvPlacements.Columns[5].HeaderText = "Tasa de cambio";
            dgvPlacements.Columns[6].HeaderText = "Estado";

            dgvPlacements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void mayorize(placements placement, bool revert) {
            var movements = (from em in entities.placements_movements
                             where em.placement_id == placement.id
                             select em).ToList();

            foreach (var movement in movements)
            {
                updateBalance(movement.account, movement.movement_type, movement.amount, revert);
            }

            entities.SaveChanges();
        }

        public void mayorize(placements placement) {
            var movements = (from em in entities.placements_movements
                             where em.placement_id == placement.id
                             select em).ToList();

            foreach (var movement in movements) {
                updateBalance(movement.account, movement.movement_type, movement.amount, false);
            }

            entities.SaveChanges();
        }

        private void updateBalance(int? accountId, String type, Decimal amount, bool? revert) {
            var account = (from em in entities.countables_accounts
                           where em.id == accountId
                           select em).First();
            if (revert == true)
            {
                if (type == "DB")
                {
                    account.balance -= amount;
                }
                else
                {
                    account.balance += amount;
                }
            }
            else
            {
                if (type == "DB")
                {
                    account.balance += amount;
                }
                else
                {
                    account.balance -= amount;
                }
            }

            if (account.level > 1)
            {
                updateBalance(account.account_major, type, amount, revert);
            }
        }

        public static MnjEntradaContable getInstance() {
            if (instance == null)
            {
                instance = new MnjEntradaContable();
            }

            return instance;
        }

        private void MnjEntradaContable_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int accountQuantity =
                (from em in entities.countables_accounts
                 where em.allow_transaction == true && em.state == true
                 select em).Count();

            int currencyQuantity = entities.currencies_types.Count(em => em.state == true);

            if (accountQuantity > 1 && currencyQuantity > 0)
            {
                EntradaContable entradaContable = EntradaContable.getInstance();
                entradaContable.ShowDialog(this);
            } else
            {
                StringBuilder sb = new StringBuilder();
                if (accountQuantity < 2)
                {
                    sb.Append("- No hay suficientes cuentas activas que permitan transacciones\n");
                }

                if (currencyQuantity < 1)
                {
                    sb.Append("- No hay tipos de monedas activas\n");
                }

                MessageBox.Show(
                    "No se puede agregar entradas.\n\n" + sb.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        public void savePlacement(placements placement) {
            if (placement != null)
            {
                entities.placements.Add(placement);
            }

            entities.SaveChanges();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btnSearch.PerformClick();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadPlacements(txtSearch.Text);
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvPlacements.SelectedRows.Count == 1)
            {
                row = dgvPlacements.SelectedRows[0];
            }
            else if (dgvPlacements.SelectedCells.Count == 1)
            {
                int i = dgvPlacements.SelectedCells[0].RowIndex;
                row = dgvPlacements.Rows[i];
            }
            else
            {
                MessageBox.Show(
                    "Debe seleccionar solo 1 asiento a visualizar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {

                EntradaContable tipoCuenta = EntradaContable.getInstance();
                int id = Int32.Parse(row.Cells[0].Value.ToString());
                placements placement = (from em in entities.placements
                                        where em.id == id
                                        select em).First();

                tipoCuenta.setEntradaContable(placement, true);
                tipoCuenta.Show();
                tipoCuenta.Focus();
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvPlacements.SelectedRows.Count == 1)
            {
                row = dgvPlacements.SelectedRows[0];
            }
            else if (dgvPlacements.SelectedCells.Count == 1)
            {
                int i = dgvPlacements.SelectedCells[0].RowIndex;
                row = dgvPlacements.Rows[i];
            }
            else
            {
                MessageBox.Show(
                    "Debe seleccionar solo 1 asiento a reversar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {

                int id = Int32.Parse(row.Cells[0].Value.ToString());

                placements placement = (from em in entities.placements
                                        where em.id == id
                                        select em).First();

                if (placement == null) {
                    MessageBox.Show(
                        "¡Asiento no encontrado!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                } else if (placement.state == "C") {
                    MessageBox.Show(
                        "¡Este asiento ha sido reversado!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                } else
                {
                    DialogResult revertIt = MessageBox.Show(
                        "¿Estas seguro que quieres realizar un reverso a esta entrada\n\nEstos cambios no se pueden deshacer?",
                        "Confirmar",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );

                    if (DialogResult.Yes == revertIt) {
                        bool updated = false;

                        using (var transaction = entities.Database.BeginTransaction())
                        {
                            try
                            {
                                placement.state = "C";
                                entities.SaveChanges();
                                mayorize(placement, true);
                                transaction.Commit();
                                updated = true;
                            }
                            catch (Exception ex) {
                                Console.WriteLine(ex);
                                updated = false;
                                transaction.Rollback();
                            }
                        }

                        if (updated)
                        {
                            MessageBox.Show(
                                "¡Reverso realizado con exito!",
                                "Información",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                            loadPlacements("");
                        }
                        else {
                            MessageBox.Show(
                                "¡Ha ocurrido un error, intente nuevamente más tarde!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }
        }
    }
}
