﻿
namespace Desktop
{
    partial class listClients
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(listClients));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.clientFlp = new System.Windows.Forms.FlowLayoutPanel();
            this.clientsControl1 = new Desktop.ClientsControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.clientFlp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.clientFlp, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.48936F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.510638F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(858, 423);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // clientFlp
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.clientFlp, 2);
            this.clientFlp.Controls.Add(this.clientsControl1);
            this.clientFlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientFlp.Location = new System.Drawing.Point(3, 3);
            this.clientFlp.Name = "clientFlp";
            this.clientFlp.Size = new System.Drawing.Size(852, 381);
            this.clientFlp.TabIndex = 0;
            // 
            // clientsControl1
            // 
            this.clientsControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("clientsControl1.BackgroundImage")));
            this.clientsControl1.Location = new System.Drawing.Point(3, 3);
            this.clientsControl1.Name = "clientsControl1";
            this.clientsControl1.Size = new System.Drawing.Size(189, 188);
            this.clientsControl1.TabIndex = 0;
            // 
            // listClients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 423);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "listClients";
            this.Text = "listClients";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.clientFlp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel clientFlp;
        private ClientsControl clientsControl1;
    }
}