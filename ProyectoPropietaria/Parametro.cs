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
    public partial class Parametro : Form
    {
        ContabilidadEntities entities;
        public Parametro()
        {
            InitializeComponent();
            entities = new ContabilidadEntities();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private String validate(parameters parameter) {
            StringBuilder sb = new StringBuilder();

            foreach (var prop in parameter.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(parameter, ((string)prop.GetValue(parameter)).Trim());
                }
            }

            if (parameter.rnc.Length == 0) {
                sb.Append("- El campo RNC es requerido\n");
            }

            return sb.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            parameters parameter = new parameters {
                rnc = txtRnc.Text,
                year = Int32.Parse(dtYear.Text),
                month = Int32.Parse(dtMonth.Text),
                month_close = Int32.Parse(dtCloseMonth.Text),
                processed = "S"
            };

            String errors = validate(parameter);

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
                entities.parameters.Add(parameter);
                entities.SaveChanges();

                MessageBox.Show(
                    "¡Datos almacenados con éxito!",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.Close();
            }
        }
    }
}
