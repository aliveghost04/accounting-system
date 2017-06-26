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
    public partial class CuentaContable : Form
    {
        private static CuentaContable instance = null;
        private countables_accounts countableAccount = null;
        ContabilidadEntities entities = new ContabilidadEntities();

        private CuentaContable()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize() {
            cbType.DataSource = (from em in entities.account_types
                                 where em.state == true
                                 select em).ToList();

            cbType.DisplayMember = "description";

            cbLevel.DataSource = new[] { 1, 2, 3};
        }

        private void loadMayorAccount(int level) {
            level--;

            if (level == 0) {
                cbMajorAccount.Enabled = false;
                cbMajorAccount.DataSource = new int[0];
            } else {
                cbMajorAccount.DataSource = (from em in entities.countables_accounts
                                             where (em.state == true && em.level == level)
                                             select em).ToList();
                cbMajorAccount.DisplayMember = "description";
                cbMajorAccount.Enabled = true;
            }
        }

        public void setCuentaContable(countables_accounts countableAccount) {
            this.countableAccount = countableAccount;
            loadMayorAccount(countableAccount.level);
            txtDescription.Text = countableAccount.description;
            chkbAllowTransactions.Checked = countableAccount.allow_transaction;
            cbLevel.SelectedIndex = countableAccount.level - 1;
            
            for (int x = 0; x < cbMajorAccount.Items.Count; x++) {
                countables_accounts tempCountableAccount =
                    (countables_accounts)cbMajorAccount.Items[x];

                if (tempCountableAccount.id == countableAccount.account_major) {
                    cbMajorAccount.SelectedIndex = x;
                    break;
                }
            }

            if (countableAccount.state)
            {
                rbActive.Checked = true;
                rbInactive.Checked = false;
            }
            else {
                rbActive.Checked = false;
                rbInactive.Checked = true;
            }
            
        }

        public static CuentaContable getInstance() {
            if (instance == null)
            {
                instance = new CuentaContable();
            }

            return instance;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void CuentaContable_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (countableAccount == null)
            {
                countableAccount = new countables_accounts
                {
                    description = txtDescription.Text,
                    account_type = ((account_types)cbType.SelectedItem).id,
                    allow_transaction = chkbAllowTransactions.Checked,
                    level = (int)cbLevel.SelectedItem,
                    balance = 0,
                    state = rbActive.Checked
                };

                if (countableAccount.level != 1)
                {
                    countableAccount.account_major = ((countables_accounts)cbMajorAccount.SelectedItem).id;
                }

                MnjCuentaContable.getInstance().saveAccountCountable(countableAccount);
                this.Close();
            }
            else {
                countableAccount.description = txtDescription.Text;
                countableAccount.account_type = ((account_types)cbType.SelectedItem).id;
                countableAccount.allow_transaction = chkbAllowTransactions.Checked;
                countableAccount.level = (int)cbLevel.SelectedItem;
                countableAccount.state = rbActive.Checked;
                
                if (countableAccount.level != 1)
                {
                    countableAccount.account_major = ((countables_accounts)cbMajorAccount.SelectedItem).id;
                }

                MnjCuentaContable.getInstance().saveAccountCountable(null);
            }

            this.Close();
        }

        private void cbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMayorAccount((int) cbLevel.SelectedItem);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult closeIt = MessageBox.Show(
                "¿Estás seguro que quieres cancelar\n\nSe perderán los cambios?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (DialogResult.Yes == closeIt) {
                this.Close();
            }
        }
    }
}
