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
        parameters parameter;

        public Parametro()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
            try
            {
                parameter = entities.parameters.First();
            } catch (Exception e) {
                parameter = null;
            }
            loadParamater();
        }

        public void loadParamater() {
            if (parameter != null)
            {
                txtRnc.Text = parameter.rnc;
                dtMonth.Value = new DateTime(2017, parameter.month, 26);
                dtCloseMonth.Value = new DateTime(2017, parameter.month_close, 26);
                dtYear.Value = new DateTime(parameter.year, 7, 26);
            }
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
            String errors = "";
            parameters parameterLocal = null;
            if (parameter == null)
            {
                parameterLocal = new parameters
                {
                    rnc = txtRnc.Text,
                    year = Int32.Parse(dtYear.Text),
                    month = Int32.Parse(dtMonth.Text),
                    month_close = Int32.Parse(dtCloseMonth.Text),
                    processed = "S"
                };
                errors = validate(parameterLocal);
            } else
            {
                parameter = entities.parameters.First(el => el.id == parameter.id);
                parameter.rnc = txtRnc.Text;
                parameter.year = Int32.Parse(dtYear.Text);
                parameter.month = Int32.Parse(dtMonth.Text);
                parameter.month_close = Int32.Parse(dtCloseMonth.Text);
                parameter.processed = "S";
                errors = validate(parameter);
            }
            
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
                if (parameter == null)
                {
                    entities.parameters.Add(parameterLocal);
                }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult cancelIt = MessageBox.Show(
                "¿Estás seguro que quieres cancelar\n\nSe perderán los cambios?",
                "Confirmar",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (cancelIt == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
