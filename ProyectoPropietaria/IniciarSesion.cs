using ProyectoPropietaria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoPropietaria
{
    public partial class IniciarSesion : Form
    {
        private ContabilidadEntities entities;
        private static IniciarSesion instance = null;
        
        private IniciarSesion()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String password = txtPassword.Text;

            using (MD5 md5 = MD5.Create()) {
                byte[] hash = Encoding.ASCII.GetBytes(password);
                hash = md5.ComputeHash(hash);

                StringBuilder sb = new StringBuilder();
                foreach (var x in hash) {
                    sb.Append(x.ToString("x2"));
                }

                password = sb.ToString();
            }
            
            var query = from em in entities.users
                        where (
                            em.username.Equals(txtUsername.Text) &&
                            em.password.Equals(password)
                        )
                        select em;

            if (query.Count() == 1)
            {
                users user = query.First();
                Contabilidad instance = Contabilidad.getInstance();
                instance.setUser(new User
                {
                    id = user.id,
                    name = user.name,
                    permission = user.permission
                });
                instance.Show();

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
