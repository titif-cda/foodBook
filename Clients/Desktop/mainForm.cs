﻿using BLLC.Services;
using Desktop.Gestion;
using Desktop.Ingredients;
using Desktop.Mets;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop
{
    public partial class MainForm : Form
    {
       
        private LoginForm loginFormInst;
        public MainForm()
        {


            InitializeComponent();
           
            Text = string.Empty;
            ControlBox = false;
            DoubleBuffered = true;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            NavFormPanel(new AccueilForm());

            Hide();
           
            using (loginFormInst = new LoginForm())
            {
                DialogResult drlogin = loginFormInst.ShowDialog();
                while (drlogin != DialogResult.OK)
                {
                    loginFormInst.ShowDialog();
                }
            }
            Show();
            
        }
        //permet de manipuler la fenetre 
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hwnd, int wmsg, int wparam, int lparam);


        private void NavFormPanel(object Formulaire)
        {
            if (this.ContenuPnl.Controls.Count > 0)
                this.ContenuPnl.Controls.RemoveAt(0);
            Form form = Formulaire as Form;
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            this.ContenuPnl.Controls.Add(form);
            this.ContenuPnl.Tag = form;
            form.Show();
        }
        
        #region Gestion des Icones
        private void RestoreBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            RestoreBtn.Visible = false;
            MaximizeBtn.Visible = true;
        }

        private void MaximizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
            RestoreBtn.Visible = true;
            MaximizeBtn.Visible = false;
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            //MainFormPBox.Visible = false;
            this.WindowState = FormWindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void pnlMenuHorizontal_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void MainFormPBox_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        /// <summary>
        /// Click sur bouton ingredients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IngredientBtn_Click(object sender, EventArgs e)
        {
            NavFormPanel(new ListIngredientsForm());
        }
        /// <summary>
        /// Click sur bouton plats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Plats_Click(object sender, EventArgs e)
        {
            NavFormPanel(new ListMetForm());
        }
        /// <summary>
        /// Click sur bouton gestion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gestionBtn_Click(object sender, EventArgs e)
        {
            NavFormPanel(new GestionForm());
            //NavFormPanel(new CommandeForm());
        }

        private void clientBtn_Click(object sender, EventArgs e)
        {
            NavFormPanel(new ListClientsForm());
        }
    }
}
