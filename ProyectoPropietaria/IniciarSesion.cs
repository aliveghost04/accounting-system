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
    public partial class IniciarSesion : Form
    {
        private ContabilidadEntities entities = new ContabilidadEntities();
        private static IniciarSesion instance = null;
        
        private IniciarSesion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var query = from em in entities.users
                            where (
                                em.username.Equals(txtUsername.Text) &&
                                em.password.Equals(txtPassword.Text)
                            )
                            select em;

            if (query.Count() == 1)
            {
                new Contabilidad(new User() {
                    name = "Erick",
                    permission = 3
                }).Show();

                resetFields();
                this.Hide();
            }
            else {
                MessageBox.Show(
                    "Usuario y/o Contraseña incorrecta", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
            
        }

        private void resetFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        public static IniciarSesion getInstance() {
            if (instance == null) {
                instance = new IniciarSesion();
            }

            return instance;
        }

        private void IniciarSesion_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
    }
}
