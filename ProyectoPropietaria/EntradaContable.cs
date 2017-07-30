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
    public partial class EntradaContable : Form
    {
        private ContabilidadEntities entities;
        private List<placements_movements> movements;
        private List<countables_accounts> accounts;
        private List<currencies_types> currencies;
        private placements placement;

        private static EntradaContable instance = null;

        private EntradaContable()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
            movements = new List<placements_movements>();

            accounts = (from em in entities.countables_accounts
                        where em.allow_transaction == true && em.state == true
                        select em).ToList();

            cbAccount.DataSource = accounts;

            cbAccount.DisplayMember = "description";

            currencies = (from em in entities.currencies_types
                          where em.state == true
                          select em).ToList();

            cbCurrency.DataSource = currencies;

            cbCurrency.DisplayMember = "description";

            cbMovementType.DataSource = new string[] { "DB", "CR" };
        }

        public static EntradaContable getInstance() {
            if (instance == null)
            {
                instance = new EntradaContable();
            }

            return instance;
        }

        private void fillMovements(placements placement) {

            if (placement != null)
            {
                movements = (from em in entities.placements_movements
                             join em2 in entities.countables_accounts
                             on em.account equals em2.id
                             where em.placement_id == placement.id
                             select em).ToList();
            }

            dgvMovements.DataSource = null;
            dgvMovements.DataSource = movements.Select(
                o => new
                {
                    Column1 = ((countables_accounts)accounts.Where(e => e.id == o.account).First()).description,
                    Column2 = o.movement_type,
                    Column3 = o.amount
                }
            ).ToList();

            dgvMovements.Columns[0].HeaderText = "Cuenta";
            dgvMovements.Columns[1].HeaderText = "Tipo Movimiento";
            dgvMovements.Columns[2].HeaderText = "Monto";
            dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool created = false;
            placements placement = new placements()
            {
                description = txtDescription.Text,
                auxiliary_id = 1,
                date = DateTime.Now,
                currency_type = ((currencies_types)cbCurrency.SelectedItem).id,
                exchange_rate = ((currencies_types)cbCurrency.SelectedItem).exchange_rate,
                state = "R"
            };

            String errors = validate(placement);

            if (errors.Length > 0) {
                MessageBox.Show(
                    errors,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            using (var transaction = entities.Database.BeginTransaction()) {
                try
                {
                    entities.placements.Add(placement);
                    
                    foreach (var el in movements)
                    {
                        el.placement_id = placement.id;
                        entities.placements_movements.Add(el);
                    }
                    entities.SaveChanges();

                    MnjEntradaContable.getInstance().mayorize(placement);
                    
                    transaction.Commit();
                    created = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                }
            };

            if (created)
            {
                MessageBox.Show(
                    "Asiento registrado con éxito",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                MnjEntradaContable.getInstance().loadPlacements("");

                this.Close();
            }
            else {
                MessageBox.Show(
                    "El asiento no pudo ser registrado",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            decimal amount = 0;
            Decimal.TryParse(txtAmount.Text, out amount);

            placements_movements movement = new placements_movements()
            {
                account = ((countables_accounts)cbAccount.SelectedItem).id,
                movement_type = cbMovementType.Text,
                amount = amount
            };

            String errors = validateMovement(movement);

            if (errors.Length > 0)
            {
                MessageBox.Show(
                    errors,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                movements.Add(movement);
                fillMovements(null);
                cleanMovementsFields();
            }
        }

        private void cleanMovementsFields() {
            txtAmount.Text = "";
            cbAccount.SelectedIndex = 0;
            cbMovementType.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (placement == null)
            {
                DialogResult closeIt = MessageBox.Show(
                    "¿Estás seguro que quieres cancelar\n\nSe perderán los cambios?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (DialogResult.Yes == closeIt)
                {
                    this.Close();
                }
            }
            else {
                this.Close();
            }
        }

        public void setEntradaContable(placements placement) {
            this.placement = placement;
            txtDescription.Text = placement.description;

            setCurrency(placement.currency_type);
            fillMovements(placement);
        }

        public void setEntradaContable(placements placement, bool readOnly)
        {
            this.setEntradaContable(placement);
            // read only options to do
            txtDescription.ReadOnly = true;
            cbCurrency.Enabled = false;

            lblAccount.Visible = false;
            lblAmount.Visible = false;
            lblMovementType.Visible = false;
            cbAccount.Visible = false;
            cbMovementType.Visible = false;
            btnAdd.Visible = false;
            txtAmount.Visible = false;

            btnCancel.Text = "Cerrar";
            btnSave.Visible = false;
            btnDelete.Visible = false;
        }

        private void setCurrency(int currencyId) {
            int index = -1;

            for (int x = 0; x < currencies.Count; x++) {
                if (currencies[x].id == currencyId) {
                    index = x;
                    break;
                }
            }

            if (index == -1)
            {
                currencies_types currency = (from em in entities.currencies_types
                                             where em.id == currencyId
                                             select em).First();

                currencies.Add(currency);
                index = (currencies.Count - 1);
            }

            cbCurrency.SelectedIndex = index;
        }

        private void EntradaContable_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private String validateMovement(placements_movements movement) {
            StringBuilder sb = new StringBuilder();

            if (movement.amount <= 0)
            {
                sb.Append("- El monto introducido debe ser mayor a 0\n");
            }

            bool alreadyMovement = movements.Count(em => em.account == movement.account) > 0;

            if (alreadyMovement)
            {
                sb.Append("- Ya existe un movimiento con esta cuenta\n");
            }

            return sb.ToString();
        }

        private String validate(placements placement)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var prop in placement.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(placement, ((string)prop.GetValue(placement)).Trim());
                }
            }

            if (placement.description.Length == 0)
            {
                sb.Append("- El campo descripción es obligatorio\n");
            }

            decimal totalDebito = 0;
            decimal totalCredito = 0;

            foreach (var movement in movements) {
                if (movement.movement_type == "DB")
                {
                    totalDebito += movement.amount;
                }
                else {
                    totalCredito += movement.amount;
                }
            }

            if (totalCredito <= 0) {
                sb.Append("- El total de créditos debe ser mayor a 0\n");
            }

            if (totalDebito <= 0) {
                sb.Append("- El total de débitos debe ser mayor a 0\n");
            }

            if ((totalCredito - totalDebito) != 0) {
                sb.Append("- Los montos de débito y crédito no concuerdan\n");
            }

            return sb.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int row = -1;

            if (dgvMovements.SelectedRows.Count == 1)
            {
                row = dgvMovements.SelectedRows[0].Index;
            }
            else if (dgvMovements.SelectedCells.Count == 1)
            {
                row = dgvMovements.SelectedCells[0].RowIndex;
            }
            else
            {
                MessageBox.Show(
                    "Debe seleccionar solo 1 movimiento a eliminar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row >= 0)
            {
                movements.RemoveAt(row);
                fillMovements(null);
            }
        }
    }
}
