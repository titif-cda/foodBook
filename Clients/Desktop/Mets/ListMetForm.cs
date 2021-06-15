﻿using BLLC.Services;
using BO.DTO.Requests;
using BO.DTO.Responses;
using BO.Entity;
using Desktop.Mets;
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
    public partial class ListMetForm : Form
    {
        private readonly IRestaurantService _restaurantService;
        private BindingSource bindingSource = new BindingSource();
        private int currentPage = 1;
        private int defaultPageSize = 15;
        private int maxPage;
        private Met selectedMet;

        public ListMetForm()
        {
            _restaurantService = new RestaurantService();
            InitializeComponent();
            LoadMets(false);

            metDtGv.ReadOnly = true;
        }



        private async void LoadMets(bool clearSelection = false)
        {
            var metsPageTask = _restaurantService.GetAllMet(new PageRequest(currentPage, defaultPageSize));
            //var metPage = await metsPageTask;

            PageResponse<Met> metPage = await metsPageTask;
            if (metPage == null)
            {
                MessageBox.Show("erreur ");
                return;
            }
            else if(metPage.TotalPages < currentPage)
            {
                PreviousPage();
            }
            maxPage = metPage.TotalPages.GetValueOrDefault();
            bindingSource.DataSource = metPage.Data;
            metDtGv.DataSource = bindingSource;
            metDtGv.Columns["Id"].Visible = false;
            metDtGv.Columns["Description"].Visible = false;
            metDtGv.Columns["TypeRepas"].HeaderText = "Type de Repas";

            if(clearSelection)
            {
                metDtGv.ClearSelection();
            }
        }

        private void AddMetBtn_Click(object sender, EventArgs e)
        {
            var addMetForm = new AddMetForm();
            addMetForm.ShowDialog();
            if(addMetForm.DialogResult == DialogResult.OK)
            {
               // RefreshPage();
                LastPage();

            }
        }

        private void NextMetBtn_Click(object sender, EventArgs e)
        {
            NextPage();
        }

        private void PreviousMetBtn_Click(object sender, EventArgs e)
        {
            PreviousPage();
        }
        private void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                RefreshPage();
            }
        }
        private void NextPage()
        {
            if (currentPage < maxPage)
            {
                currentPage++;
                RefreshPage();
            }
        }
        private void LastPage()
        {
           currentPage = maxPage;
           RefreshPage();
        }
        private void RefreshPage()
        {
            CurentPageMetLbl.Text = currentPage.ToString();
            //currentPageLabel.Text = currentPageLabel.ToString();
            this.LoadMets();
        }

        private void RefreshMetBtn_Click(object sender, EventArgs e)
        {
            RefreshPage();
        }

        private void metDtGv_CurrentCellChanged(object sender, EventArgs e)
        {
            // CurrentMetLbl.Text = metDtGv.DataSource.g
        }

        private async void metDtGv_Click(object sender, EventArgs e)
        {
        }

        private async void DeleteMetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Met met = metDtGv.CurrentRow.DataBoundItem as Met;
                await _restaurantService.DeleteMet(met);
                MessageBox.Show("Le plat : " + met.Libelle + " a été supprimé");
                RefreshPage();
            }
            catch (Exception)
            {

                MessageBox.Show("Le plat selectionné n'a pas été supprimé");
            }

        }

        private async void metDtGv_SelectionChanged(object sender, EventArgs e)
        {
            if (metDtGv.CurrentRow != null && selectedMet?.Id != (metDtGv.CurrentRow.DataBoundItem as Met).Id)
            {
                Met metSelected = (metDtGv.CurrentRow.DataBoundItem as Met);
                selectedMet = metSelected;
                int? metId = metSelected?.Id;
                if (metId != null)
                {
                    Met met = await _restaurantService.GetDetailsMet(metId.Value);

                    CurrentMetLbl.Text = met.Libelle;
                    CurrentDescLabel.Text = met.Description;
                    typeLbl.Text = met.TypeRepas?.Libelle;
                    if (met.ListDesIngredients?.Count() > 0)
                        CurrentIngredientsLbl.Text = met.ListDesIngredients.Select(ingredientMet => $"{ingredientMet.Ingredient.Nom}( {ingredientMet.Quantite} )").Aggregate((s1, s2) => $"{s1}, {s2}");
                    else
                    {
                        CurrentIngredientsLbl.Text = "Aucun ingrédient renseigné";
                    }
                }
            }
        }

        private void RefreshMetBtn_Click_1(object sender, EventArgs e)
        {
            RefreshPage();
        }
    }
}
