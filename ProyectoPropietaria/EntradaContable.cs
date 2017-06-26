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

        private static EntradaContable instance = null;

        private EntradaContable()
        {
            InitializeComponent();
        }

        public static EntradaContable getInstance() {
            if (instance == null)
            {
                instance = new EntradaContable();
            }

            return instance;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
