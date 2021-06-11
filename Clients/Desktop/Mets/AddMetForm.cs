﻿using BLLC.Services;
using BO.DTO.Requests;
using BO.DTO.Responses;
using BO.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop.Mets
{
    public partial class AddMetForm : Form
    {
        private readonly IRestaurantService _restaurantService;

        //Binding sources
        private BindingSource bindingSourceDeLaListeDesIngredients = new BindingSource();
        private BindingSource bindingSourceTypeRepas = new BindingSource();
        private BindingSource bindingSourceListeIngredientParPlats = new BindingSource();

        //Initialise une liste d'ingrédients côté client.
        private readonly List<MetsIngredients> AllIngredientsListe = new();
       
        //Initialise les parametres du DTO 
        private int currentPage = 1;
        private int defaultPageSize = 15;
        private int maxPage;
        
        
        public AddMetForm()
        {
            _restaurantService = new RestaurantService();
            InitializeComponent();
            LoadIngredient();
            LoadListBox();
            ListeIngredientsDtGv.ReadOnly = true;
            ListeIngredientparPlatDGV.ReadOnly = true;

        }

        private async void LoadIngredient()
        {

            Task<PageResponse<Ingredient>> ingredientPageTask = _restaurantService.GetAllIngredients(new PageRequest(currentPage, defaultPageSize));
            try
            {
                PageResponse<Ingredient> ingredientPage = await ingredientPageTask;
                if (ingredientPage == null)
                {
                    ErrorMessage();
                    return;
                }
                maxPage = ingredientPage.TotalPages.GetValueOrDefault();
                bindingSourceDeLaListeDesIngredients.DataSource = ingredientPage.Data;
                ListeIngredientsDtGv.DataSource = bindingSourceDeLaListeDesIngredients;
                ListeIngredientsDtGv.Columns["Id"].Visible = false;
               
            }
            catch
            {
                ErrorMessage();
            }


        }

        private void LoadListOfIngredient(MetsIngredients ingredientsParMets)
        {
            ListeIngredientparPlatDGV.DataSource = null;
            if (ingredientsParMets != null)
            {
                AllIngredientsListe.Add(ingredientsParMets);
            }

            bindingSourceListeIngredientParPlats.DataSource = AllIngredientsListe;
            ListeIngredientparPlatDGV.DataSource = bindingSourceListeIngredientParPlats;
            //Cache les colonnes non désirées
            ListeIngredientparPlatDGV.Columns["Ingredient"].DisplayIndex = 0;
            ListeIngredientparPlatDGV.Columns["Quantite"].DisplayIndex = 1;
            ListeIngredientparPlatDGV.Columns["IdMet"].Visible = false;
           
        }
        private void ErrorMessage()
        {
            MessageBox.Show("Erreur: Comunication impossible avec le service distant.");
            return;
        }

        private void ListeIngredientsDtGv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private async void AddMetBtn_Click(object sender, EventArgs e)
        {
            Met newMet = new Met();
                newMet = new();
            {
                newMet.Libelle = NomMetTBox.Text;
                newMet.Description = DescriptionMetRTBox.Text;
                //Recupere l'id de la valeur selectionnée dans la combobox
                TypeRepasCBox.ValueMember = "Id";
                newMet.TypeRepas = new TypeRepas();
                newMet.TypeRepas.Id =int.Parse(TypeRepasCBox.SelectedValue.ToString());
               
                newMet.ListDesIngredients = AllIngredientsListe;
                
            }
            Task<Met> metTask = _restaurantService.CreateMet(newMet);

        }

        private async void LoadListBox()
        {
            Task<PageResponse<TypeRepas>> typeRepasPageTask = _restaurantService.GetAllTypeRepas(new PageRequest(currentPage, defaultPageSize));
            PageResponse<TypeRepas> typeRepas = await typeRepasPageTask;
            bindingSourceTypeRepas.DataSource = typeRepas.Data;
            TypeRepasCBox.DataSource = bindingSourceTypeRepas;
            TypeRepasCBox.DisplayMember = "Libelle";
            
        }

        private void AddToListForMetBtn_Click(object sender, EventArgs e)
        {
            //var idIngr = newIngredient.IdIngredient;
            MetsIngredients newMetIngredient = new();
            newMetIngredient.Ingredient = ListeIngredientsDtGv.Rows[ListeIngredientsDtGv.CurrentRow.Index].DataBoundItem as Ingredient;
            //var selectedRow = ListeIngredientsDtGv.CurrentRow;
            //selectedRow.DataGridView.CurrentRow.en = true;
            //ListeIngredientsDtGv.Rows[ListeIngredientsDtGv.CurrentRow.Index].Visible = false;
            //selectedRow.Visible = false;
            if (this.ListeIngredientsDtGv.SelectedRows.Count > 0)
            {
                ListeIngredientsDtGv.Rows.RemoveAt(this.ListeIngredientsDtGv.CurrentRow.Index);
            }

            newMetIngredient.Quantite = float.Parse(QuantiteIngredientParMetTxBox.Text);
            LoadListOfIngredient(newMetIngredient);

        }

        private void DeleteFromListForMetBtn_Click(object sender, EventArgs e)
        {
            //var idIngr = newIngredient.IdIngredient;
            MetsIngredients newMetParIngredient = new();
           // newMetParIngredient.Ingredient = ListeIngredientsDtGv.Rows[ListeIngredientsDtGv.CurrentRow.Index].DataBoundItem as Ingredient;
            
            if (this.ListeIngredientsDtGv.SelectedRows.Count > 0)
            {

                bindingSourceDeLaListeDesIngredients.Add((ListeIngredientparPlatDGV.CurrentRow.DataBoundItem as MetsIngredients).Ingredient);
                ListeIngredientparPlatDGV.Rows.RemoveAt(this.ListeIngredientparPlatDGV.CurrentRow.Index);
            }
            LoadListOfIngredient(newMetParIngredient);
        }
    }
}
