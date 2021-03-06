﻿using ProyectoPropietaria.Models;
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
    public partial class Contabilidad : Form
    {
        public static User user;
        public static bool showLogin = false;
        private static Contabilidad instance;
        private ContabilidadEntities entities;

        private Contabilidad()
        {
            InitializeComponent();
            entities = ConnectionDB.getInstance().getEntities();
        }

        public void setUser(User user) {
            Contabilidad.user = user;
            lblName.Text = user.name;
            managePermission();
        }

        public static Contabilidad getInstance() {
            if (instance == null)
            {
                instance = new Contabilidad();
            }

            return instance;
        }

        private void managePermission()
        {
            if (user.permission == 3)
            {
                configSTMI.Visible = false;
            }
        }

        private void parametrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Parametro().ShowDialog(this);
        }

        private void cuentasContablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new CuentaContable().Show();
        }

        private void tiposDeCuentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new TipoCuenta().Show();
        }

        private void tiposDeMonedasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void entradaContableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new EntradaContable().Show();
        }

        private void cRUDToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void salirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IniciarSesion.getInstance().Show();
            showLogin = true;
            this.Close();
        }

        private void Contabilidad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !showLogin)
            {
                DialogResult closeIt = MessageBox.Show(
                    "¿Está seguro que desea salir?",
                    "Salir",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (closeIt == DialogResult.Yes)
                {
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else {
                instance = null;
                showLogin = false;
            }
        }

        private void btnAccountTypes_Click(object sender, EventArgs e)
        {
            MnjTipoCuenta mnjTipoCuenta = MnjTipoCuenta.getInstance();
            mnjTipoCuenta.ShowDialog(this);
            mnjTipoCuenta.Focus();
        }

        private void btnCurrenciesTypes_Click(object sender, EventArgs e)
        {
            MnjTipoMoneda mnjTipoMoneda = MnjTipoMoneda.getInstance();
            mnjTipoMoneda.ShowDialog(this);
            mnjTipoMoneda.Focus();
        }

        private void btnPlacements_Click(object sender, EventArgs e)
        {
            MnjEntradaContable mnjEntradaContable = MnjEntradaContable.getInstance();
            mnjEntradaContable.ShowDialog(this);
            mnjEntradaContable.Focus();
        }

        private void btnCountablesAccounts_Click(object sender, EventArgs e)
        {
            MnjCuentaContable mnjCuentaContable = MnjCuentaContable.getInstance();
            mnjCuentaContable.ShowDialog(this);
            mnjCuentaContable.Focus();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (entities.parameters.Count() == 1)
            {
                new Reporte().ShowDialog(this);
            }
            else
            {
                MessageBox.Show(
                    "No se puede ver reportes. \nNo se han configurado los parametros del sistema",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MnjUsers.getInstance().ShowDialog(this);
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Sistema de Contabilidad\nHecho por Erick Jiménez\nCopyright © 2017",
                "Acerca de",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
