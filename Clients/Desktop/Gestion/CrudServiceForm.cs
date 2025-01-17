﻿using BLLC.Services;
using BO.DTO.Requests;
using BO.DTO.Responses;
using BO.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop.Gestion
{
    public partial class CrudServiceForm : Form
    {
        private readonly IRestaurantService _restaurantService = new RestaurantService();
        private BindingSource bindingSourceplats = new BindingSource();
        public bool isCreation = false;
        private int currentPage = 1;
        private int maxPage;
        private int defaultPageSize = 20;
        private int IdTypePlat;
        bool existDate = false;
        bool existMidi = false;
        Service currentService;
        
        public CrudServiceForm(Service service = null)
        {
            InitializeComponent();
            Initialize(service);
        }

        public async void Initialize(Service service = null)
        {
            await LoadCheckBoxListBox();
            if (service == null)
            {
                currentService = new Service();
                //creation
                isCreation = true;
                //creation
                currentService.Date = serviceDateTP.Value;
                if (MidiCheckBox.Checked)
                {
                    SoirCheckBox.Checked = false;
                    currentService.Midi = true;
                }
                else
                {
                    currentService.Midi = false;
                    MidiCheckBox.Checked = false;
                }

            }
            else
            {
                //modif
                currentService = service;
                ActionCrudServiceBtn.Text = "Modifier";
                //label1.Text = service.Date?.ToString("dd MMMM yyyy");
                serviceDateTP.Value = service.Date ?? DateTime.Now;
                // service
                if (service.Midi)
                {
                    MidiCheckBox.Checked = true;
                    SoirCheckBox.Checked = false;
                }
                else
                {
                    SoirCheckBox.Checked = true;
                    MidiCheckBox.Checked = false;
                }
                //focus sur le plat parmi la liste des entree, plats ou dessert
                entreeCBox.SelectedItem = service.ListPlats.Where(m => m.TypeRepas.Id == 1).FirstOrDefault();
                platCBox.SelectedItem = service.ListPlats.Where(m => m.TypeRepas.Id == 2).FirstOrDefault();
                dessertCBox.SelectedItem = service.ListPlats.Where(m => m.TypeRepas.Id == 3).FirstOrDefault();


            }
        }
        public Service Compute()
        {
            bool ckboxMidi = false;
            if (MidiCheckBox.Checked)
            {
                ckboxMidi = true;
            }

            int? id = null;
            if (!isCreation)
            {
                id = currentService.Id;
            }
            return new Service()
            {
                
                Id = Convert.ToInt32(id),
                Date = serviceDateTP.Value,
                Midi = ckboxMidi,

                ListPlats = new List<Met>() { 
                    entreeCBox.SelectedItem as Met,
                    platCBox.SelectedItem as Met,
                    dessertCBox.SelectedItem as Met
                }

            };
        }

        private async Task LoadCheckBoxListBox()
        {
            var filter = new FilterMetPaged(currentPage, 1000000);

            PageResponse<Met> met = await _restaurantService.GetAllMet(filter);
            bindingSourceplats.DataSource = met.Data;

            //met.Data.Select
            //var test = met.Data.Select(m => new { m.Libelle, TypeRepasLibelle = m.TypeRepas.Libelle }).ToList();
            var listEntree = met.Data.Where(m => m.TypeRepas.Libelle == "Entree").ToList();
            var listPlat = met.Data.Where(m => m.TypeRepas.Libelle == "Plat").ToList();
            var listDessert = met.Data.Where(m => m.TypeRepas.Libelle == "Dessert").ToList();
            entreeCBox.DataSource = listEntree;
            platCBox.DataSource = listPlat;
            dessertCBox.DataSource = listDessert;
            //dessertCBox.DataSource = bindingSourceDessert;
            dessertCBox.DisplayMember = "Libelle";
        }

        private async void ActionCrudServiceBtn_Click(object sender, EventArgs e)
        {
            var servicesDatabase = _restaurantService.GetAllService(new PageRequest(currentPage, defaultPageSize));
            PageResponse<Service> servicesData = await servicesDatabase;
            
            //Service serviceExistant = await servicesDatabase ;
            currentService = Compute();
            


            if (isCreation)
            {
                List<Service> servicesBdd = servicesData.Data.ToList();
                foreach (var s in servicesBdd)
                {
                    if (s.Midi == currentService.Midi && s.Date.GetValueOrDefault().Date == currentService.Date.GetValueOrDefault().Date)
                    {
                        var d = MessageBox.Show("Le service existe déjà");
                        if(d == DialogResult.OK)
                            return;
                    }
                }
                var service = await _restaurantService.CreateService(currentService);
                
                string serviceLibelle;
                
                if (MidiCheckBox.Checked)
                    {
                        SoirCheckBox.Checked = false;
                        serviceLibelle = "Midi";
                        DialogResult = DialogResult.OK;
                        MessageBox.Show("La service du " + serviceLibelle + " le " + service.Date?.ToString("dd MMMM yyyy") + " a été créé");
                    }
                    else if (SoirCheckBox.Checked)
                    {
                        MidiCheckBox.Checked = false;
                        serviceLibelle = "soir";
                        DialogResult = DialogResult.OK;
                        MessageBox.Show("La service du " + serviceLibelle + " le " + service.Date?.ToString("dd MMMM yyyy") + " a été créé");
                    }
            }
            else
            {
                if (currentService == null)
                {
                    MessageBox.Show("La modification du service du  : " + currentService.Date?.ToString("dd MMMM yyyy") + " a échoué");
                }
                else
                {
                    currentService = await _restaurantService.PutService(currentService);
                    DialogResult = DialogResult.OK;
                    MessageBox.Show("La service du "  + currentService.Date?.ToString("dd MMMM yyyy") + " a été créé");
                }
            }
        }

        private async void DeleteBtnCrudService_Click(object sender, EventArgs e)
        {
            
            if (!isCreation)
            {
                
                DialogResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce service ?",
                                                        "Attention",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question,
                                                        MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    currentService = Compute();
                   
                    try
                    {
                        await _restaurantService.DeleteService(currentService);
                        MessageBox.Show("Le service du : " + currentService.Date?.ToString("dddd dd MMM", CultureInfo.CreateSpecificCulture("fr-FR")) + " à été supprimé");
                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Le service n'a pas été supprimé");
                        throw;
                    }
                }
                
                DialogResult = DialogResult.OK;
            }
        }

        private void MidiCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (MidiCheckBox.Checked)
            {
                SoirCheckBox.Checked = false;
            }
        }

        private void SoirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SoirCheckBox.Checked)
            {
                MidiCheckBox.Checked = false;
            }

        }
    }

}
