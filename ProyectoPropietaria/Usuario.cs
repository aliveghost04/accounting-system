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
    public partial class Usuario : Form
    {
        ContabilidadEntities entities;
        private users user;
        private List<UserLevel> userLevels;

        public Usuario()
        {
            InitializeComponent();
            entities = new ContabilidadEntities();
            userLevels = new List<UserLevel>() {
                new UserLevel { name = "Administrador", value = 1 },
                new UserLevel { name = "Normal", value = 3 }
            };
            cbLevel.DataSource = userLevels;

            cbLevel.DisplayMember = "name";
        }

        public void setUsuario(users user) {
            this.user = user;
            txtName.Text = user.name;
            txtUsername.Text = user.username;
            cbLevel.SelectedIndex = userLevels.FindIndex(el => el.value == user.permission);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            users userLocal = null;

            if (user == null)
            {
                userLocal = new users
                {
                    name = txtName.Text,
                    created_at = DateTime.Now,
                    permission = ((UserLevel)cbLevel.SelectedItem).value,
                    username = txtUsername.Text
                };

                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = Encoding.ASCII.GetBytes(txtPassword.Text);
                    hash = md5.ComputeHash(hash);

                    StringBuilder sb = new StringBuilder();
                    foreach (var x in hash)
                    {
                        sb.Append(x.ToString("x2"));
                    }

                    userLocal.password = sb.ToString();
                }
            } else
            {
                user = entities.users.First(el => el.id == user.id);
                user.name = txtName.Text;
                user.updated_at = DateTime.Now;
                user.permission = ((UserLevel)cbLevel.SelectedItem).value;
                user.username = txtUsername.Text;

                if (txtPassword.Modified)
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] hash = Encoding.ASCII.GetBytes(txtPassword.Text);
                        hash = md5.ComputeHash(hash);

                        StringBuilder sb = new StringBuilder();
                        foreach (var x in hash)
                        {
                            sb.Append(x.ToString("x2"));
                        }

                        user.password = sb.ToString();
                    }
                }
            }

            String errors = "";
            if (userLocal == null)
            {
                errors = validate(user);
            } else
            {
                errors = validate(userLocal);
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
            else {
                if (userLocal != null)
                {
                    entities.users.Add(userLocal);
                }
                entities.SaveChanges();

                MessageBox.Show(
                    "¡Usuario guardado con éxito!",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                MnjUsers.getInstance().loadUsers("");
                this.Close();
            }
        }

        private String validate(users user) {
            StringBuilder sb = new StringBuilder();

            foreach (var prop in user.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(user, ((string)prop.GetValue(user)).Trim());
                }
            }

            if (user.name.Length == 0) {
                sb.Append("- El campo nombre es requerido\n");
            } else if (user.name.Length < 2) {
                sb.Append("- La longitud del nombre debe ser igual o mayor a 2\n");
            }

            if (user.password.Length == 0)
            {
                sb.Append("- El campo contraseña es requerido\n");
            }
            else if (user.password.Length < 5)
            {
                sb.Append("- La longitud de la contraseña debe ser igual o mayor a 4\n");
            }

            if (user.username.Length == 0)
            {
                sb.Append("- El campo usuario es requerido\n");
            }
            else if (user.name.Length < 5)
            {
                sb.Append("- La longitud del usuario debe ser igual o mayor a 5\n");
            } else
            {
                bool usernameTaken = entities.users.Count(
                    el => el.username == user.username
                ) > 0;

                if (usernameTaken && user.id == 0) {
                    sb.Append("- El usuario especificado se encuentra en uso");
                }
            }

            return sb.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult cancelIt = MessageBox.Show(
                "¿Estás seguro que quieres cancelar\n\nSe perderán los cambios?",
                "Confirmar",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (cancelIt == DialogResult.Yes) {
                this.Close();
            }
        }
    }

    class UserLevel {
        public String name { get; set; }
        public int value { get; set; }
    }
}
