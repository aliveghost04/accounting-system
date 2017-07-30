using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoPropietaria
{
    public partial class Reporte : Form
    {
        ContabilidadEntities entities;
        parameters parameter;
        public Reporte()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();

            List<countables_accounts> accounts = entities.countables_accounts.ToList();
            accounts.Insert(0, new countables_accounts { description = "---- Todas ----"});
            cbAccounts.DataSource = accounts;
            cbAccounts.DisplayMember = "description";

            parameter = entities.parameters.First();
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

            DateTime dateFrom = new DateTime(dtFrom.Value.Year, dtFrom.Value.Month, dtFrom.Value.Day);
            DateTime dateTo = new DateTime(dtTo.Value.Year, dtTo.Value.Month, dtTo.Value.Day);
            dateTo = dateTo.AddMilliseconds(-1);
            dateTo = dateTo.AddDays(1);
            loadMovements(account, txtDescription.Text, dateFrom, dateTo);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.CheckPathExists = true;
            sfd.DefaultExt = ".csv";
            sfd.Filter = "Comma separated value (*.csv)|*.csv";
            sfd.FilterIndex = 0;
            sfd.AddExtension = true;
            sfd.FileName = lblRnc.Text + "_" + DateTime.Now.ToString("yyy-MM-dd-H-m-s");
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FileName.Length > 0)
                {
                    StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8);
                    sw.WriteLine("RNC, Mes Cierre, Mes, Año");
                    sw.WriteLine(lblRnc.Text + "," + lblCloseMonth.Text + "," + lblMonth.Text + "," + lblYear.Text + "\n");
                    sw.WriteLine("Descripción,Fecha,Cuenta,Tipo,Monto,Tasa de Cambio,Total,Estado");

                    foreach (DataGridViewRow el in dgvMovements.Rows)
                    {
                        sw.Write(el.Cells[0].Value + ",");
                        sw.Write(el.Cells[1].Value + ",");
                        sw.Write(el.Cells[2].Value + ",");
                        sw.Write(el.Cells[3].Value + ",");
                        sw.Write(el.Cells[4].Value + ",");
                        sw.Write(el.Cells[5].Value + ",");
                        sw.Write(el.Cells[6].Value + ",");
                        sw.WriteLine(el.Cells[7].Value);
                    }
                    sw.Close();

                    MessageBox.Show(
                        "¡Datos exportados con éxito!",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Debe seleccionar un directorio a guardar",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }
    }
}
