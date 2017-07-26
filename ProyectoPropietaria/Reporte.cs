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
    public partial class Reporte : Form
    {
        ContabilidadEntities entities;
        public Reporte()
        {
            InitializeComponent();
            entities = new ContabilidadEntities();

            List<countables_accounts> accounts = entities.countables_accounts.ToList();
            accounts.Insert(0, new countables_accounts { description = "---- Todas ----"});
            cbAccounts.DataSource = accounts;
            cbAccounts.DisplayMember = "description";

            parameters parameter = entities.parameters.First();
            lblRnc.Text = parameter.rnc;
            lblCloseMonth.Text = new DateTime(2010, parameter.month_close, 5).ToString("MMMM");
            lblMonth.Text = new DateTime(2010, parameter.month, 5).ToString("MMMM");
            lblYear.Text = parameter.year.ToString();
        }

        private void loadMovements(String account, String description, DateTime fromDate, DateTime toDate) {
            dgvMovements.DataSource = null;
            dgvMovements.DataSource = (from em in entities.placements_movements
                                       join em2 in entities.placements
                                       on em.placement_id equals(em2.id)
                                       join em3 in entities.countables_accounts
                                       on em.account equals(em3.id)
                                       where em3.description.Contains(account) &&
                                       em2.description.Contains(description) &&
                                       em2.date >= fromDate && 
                                       em2.date <= toDate
                                       select new {
                                           description = em2.description,
                                           em2.date,
                                           account = em3.description,
                                           em.movement_type,
                                           em.amount,
                                           em2.exchange_rate,
                                           total = (em.amount * em2.exchange_rate),
                                           em2.state
                                       }).ToList();

         
            dgvMovements.Columns[0].HeaderText = "Descripción";
            dgvMovements.Columns[1].HeaderText = "Fecha";
            dgvMovements.Columns[2].HeaderText = "Cuenta";
            dgvMovements.Columns[3].HeaderText = "Tipo";
            dgvMovements.Columns[4].HeaderText = "Monto";
            dgvMovements.Columns[5].HeaderText = "Tasa Cambio";
            dgvMovements.Columns[6].HeaderText = "Total";
            dgvMovements.Columns[7].HeaderText = "Estado";

            dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Reporte_Load(object sender, EventArgs e)
        {
            loadMovements("", "", DateTime.Now, DateTime.Now);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            String account = "";
            countables_accounts cAccount = ((countables_accounts)cbAccounts.SelectedItem);
            if (cAccount.id != 0)
            {
                account = cAccount.description;
            }

            loadMovements(account, txtDescription.Text, dtFrom.Value, dtTo.Value);
        }
    }
}
