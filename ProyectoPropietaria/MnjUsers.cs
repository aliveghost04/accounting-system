﻿using System;
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
    public partial class MnjUsers : Form
    {
        private ContabilidadEntities entities;
        private static MnjUsers instance = null;

        private MnjUsers()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
            loadUsers("");
        }

        public void loadUsers(String filter) {
            int id = 0;
            Int32.TryParse(filter, out id);
            dgvUsers.DataSource = (from em in entities.users
                                   where em.id == id ||
                                   em.name.Contains(filter) ||
                                   em.username.Contains(filter)
                                   select new {
                                       em.id,
                                       em.name,
                                       em.username,
                                       permission = em.permission == 3 ? "Normal" : "Administrador"
                                   }).ToList();

            dgvUsers.Columns[0].HeaderText = "ID";
            dgvUsers.Columns[1].HeaderText = "Nombre";
            dgvUsers.Columns[2].HeaderText = "Usuario";
            dgvUsers.Columns[3].HeaderText = "Permiso";

            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static MnjUsers getInstance() {
            if (instance == null) {
                instance = new MnjUsers();
            }

            return instance;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadUsers(txtSearch.Text.Trim());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvUsers.SelectedRows.Count == 1)
            {
                row = dgvUsers.SelectedRows[0];
            }
            else if (dgvUsers.SelectedCells.Count == 1)
            {
                int i = dgvUsers.SelectedCells[0].RowIndex;
                row = dgvUsers.Rows[i];
            }
            else
            {
                MessageBox.Show(
                    "Debes seleccionar solo 1 usuario a eliminar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {
                DialogResult deleteIt = MessageBox.Show(
                    "¿Estas seguro de eliminar este usuario?",
                    "Confirmar",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (deleteIt != DialogResult.Yes) {
                    return;
                }

                int id = Int32.Parse(row.Cells[0].Value.ToString());
                users localUser = (from em in entities.users
                                   where em.id == id
                                   select em).First();

                if (localUser == null)
                {
                    MessageBox.Show(
                        "El Usuario seleccionado no existe",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    loadUsers("");
                }
                else
                {
                    int adminTotal = entities.users.Count(el => el.permission == 1);

                    if (adminTotal == 1 && localUser.permission == 1)
                    {
                        MessageBox.Show(
                            "No puedes eliminar al único administrador",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                    else {
                        bool delete = true;
                        bool sameAsLogged = false;
                        if (localUser.id == Contabilidad.user.id)
                        {
                            delete = false;
                            sameAsLogged = true;
                            DialogResult response = MessageBox.Show(
                                "Estas a punto de eliminar tu usuario. Esta acción te expulsará del sistema\n\n¿Deseas continuar?",
                                "Confirmar",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question
                            );

                            if (response == DialogResult.Yes) {
                                delete = true;
                            }
                        }

                        if (delete)
                        {
                            entities.users.Remove(localUser);
                            entities.SaveChanges();

                            MessageBox.Show(
                                "¡Usuario eliminado con éxito!",
                                "Información",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            if (sameAsLogged)
                            {
                                IniciarSesion.getInstance().Show();
                                Contabilidad.showLogin = true;
                                Contabilidad.getInstance().Close();
                            }
                            else {
                                loadUsers("");
                            }
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new Usuario().ShowDialog(this);
        }

        private void MnjUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            if (dgvUsers.SelectedRows.Count == 1)
            {
                row = dgvUsers.SelectedRows[0];
            }
            else if (dgvUsers.SelectedCells.Count == 1)
            {
                int i = dgvUsers.SelectedCells[0].RowIndex;
                row = dgvUsers.Rows[i];
            }
            else
            {
                MessageBox.Show(
                    "Debes seleccionar solo 1 usuario a modificar",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

            if (row != null)
            {
                int id = Int32.Parse(row.Cells[0].Value.ToString());
                users localUser = (from em in entities.users
                                   where em.id == id
                                   select em).First();

                Usuario usuario = new Usuario();
                usuario.setUsuario(localUser);
                usuario.ShowDialog(this);
            }


        }
    }
}
